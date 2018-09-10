using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSG;
using Zenject;

namespace pg.core
{
    /*
     * PromiseLoader manages a collection of promises:
     * -Each promise can be given a weight to be used in the overal percent complete
     * -If a promise reports progress it will mark that percentage of the weight finished
     *
     * HOW TO USE:
     *  1.  The PromiseLoader is a transient injection, so inject a new instance wherever you need one.
     *      Alternatively, you could also just create an instance and call Initialize().
     *      If you are reusing a PromiseLoader, make sure to call Reset()
     *  2.  Use any combination of AddLoaderPromise/AddLoaderPromises methods to add which promises
     *      you want to track. If you have several promises/promise arrays but can determine the total length,
     *      use SetPromiseCapacity to optimize creation of the internal lists being used.
     *  3.  When all promises have been added call StartLoading() passing in a timeout. The promise returned will
     *      report the overall progress, taking promise weights into account. The main promise resolves if all
     *      promises marked "mustResolve" succeed and all other promises resolve or reject.
     *      
     *      Note:
     *      It is possible to add promises afer calling StartLoading(), but you run the risk of load completion
     *      before the point where you add them. So only do so in very controlled edge cases.
     */

    public class PromiseLoader : IInitializable
    {
        //Determines whether progress reporting can go backwards.
        //Would only happen when a promise reports a loss or a 
        //new promise was added post StartLoading()
        public bool AllowPercentDrops = false;

        private int _loadCount;
        public int LoadCount
        {
            get { return _loadCount; }
        }

        private int _loadMustResolveCount;

        private List<float> _loadWeights;


        //How many promises have been marked resolved
        private int _loadSuccessCount;
        public int LoadSuccessCount
        {
            get { return _loadSuccessCount; }
        }

        //How many promises marked "mustResolve" have been marked resolved
        private int _loadMustResolveSuccessCount;
        public int LoadMustResolveSuccessCount
        {
            get { return _loadMustResolveSuccessCount; }
        }

        //How many promises have been marked rejected
        private int _loadFailedCount;
        public int LoadFailedCount
        {
            get { return _loadFailedCount; }
        }

        //How many promises marked "mustResolve" have been marked rejected
        private int _loadRequiredFailedCount;
        public int LoadRequiredFailedCount
        {
            get { return _loadRequiredFailedCount; }
        }

        //How many promises have been marked resolved or rejected
        private int _loadFinishedCount;
        public int LoadFinishedCount
        {
            get { return _loadFinishedCount; }
        }

        private List<float> _loadWeightsFinished;


        private int _loadId;
        private Promise _loadPromise;
        private bool _loadPercDirty;
        private float _loadedPerc;

        [Inject] private readonly CoroutineRunner _coroutineRunner;

        public void Initialize()
        {
            _loadWeights = null;
            _loadWeightsFinished = null;
            _loadId = 0;
            _loadPromise = null;

            Reset();
        }

        //Call before reusing an instance of a PromiseLoader
        public void Reset()
        {
            if (_loadPromise != null)
            {
                Debug.LogWarning(string.Format("{0} , trying to reset a PromiseLoader that is still in use.", this));
                return;
            }

            _loadCount = 0;
            _loadMustResolveCount = 0;

            if (_loadWeights != null)
            {
                _loadWeights.Clear();
            }

            _loadSuccessCount = 0;
            _loadMustResolveSuccessCount = 0;
            _loadFailedCount = 0;
            _loadRequiredFailedCount = 0;
            _loadFinishedCount = 0;

            if (_loadWeightsFinished != null)
            {
                _loadWeightsFinished.Clear();
            }

            _loadPercDirty = true;
            _loadedPerc = 0f;
        }

        //Returns the weighted percentage of loading progress
        public float GetLoadedPerc()
        {
            CalculateLoadPercIfNeeded();
            return _loadedPerc;
        }

        //Return the percentage of promises that were successful
        public float GetLoadedSuccessPerc()
        {
            return (float) _loadSuccessCount / (float) _loadCount;
        }

        //Return the percentage of promises that failed
        public float GetLoadedFailedPerc()
        {
            return (float) _loadFailedCount / (float) _loadCount;
        }

        //Initializes internal lists for an expected number of promises
        public void SetPromiseCapacity(int cap)
        {
            if (_loadWeights == null)
            {
                _loadWeights = new List<float>(cap);
            }
            else if (_loadWeights.Capacity < cap)
            {
                _loadWeights.Capacity = cap;
            }

            if (_loadWeightsFinished == null)
            {
                _loadWeightsFinished = new List<float>(cap);
            }
            else if (_loadWeightsFinished.Capacity < cap)
            {
                _loadWeightsFinished.Capacity = cap;
            }
        }

        //Add a single promise to the PromiseLoader
        public void AddLoaderPromise(IBasePromise promise, float weight = 1f, bool mustResolve = true)
        {
            if (_loadWeights == null)
            {
                _loadWeights = new List<float>();
            }

            if (_loadWeightsFinished == null)
            {
                _loadWeightsFinished = new List<float>();
            }

            InternalAddLoaderPromise(promise, weight, mustResolve);
        }

        //Add an array of promises to the PromiseLoader
        public void AddLoaderPromises(IBasePromise[] promises, float weightPerPromise = 1f, bool mustResolve = true)
        {
            CreateLoadArraysForIncoming(promises.Length);
            foreach (IPromise promise in promises)
            {
                InternalAddLoaderPromise(promise, weightPerPromise, mustResolve);
            }
        }

        //Add a list of promises to the PromiseLoader
        public void AddLoaderPromises(List<IBasePromise> promises, float weightPerPromise = 1f, bool mustResolve = true)
        {
            CreateLoadArraysForIncoming(promises.Count);
            foreach (IPromise promise in promises)
            {
                InternalAddLoaderPromise(promise, weightPerPromise, mustResolve);
            }
        }

        private void CreateLoadArraysForIncoming(int incomingPromiseCount)
        {
            int neededCapacity;
            if (_loadWeights == null)
            {
                neededCapacity = incomingPromiseCount;
                _loadWeights = new List<float>(incomingPromiseCount);
            }
            else
            {
                neededCapacity = _loadWeights.Count + incomingPromiseCount;
                if (neededCapacity > _loadWeights.Capacity)
                {
                    _loadWeights.Capacity = neededCapacity;
                }
            }

            if (_loadWeightsFinished == null)
            {
                _loadWeightsFinished = new List<float>(neededCapacity);
            }
            else if (_loadWeightsFinished.Capacity < neededCapacity)
            {
                _loadWeightsFinished.Capacity = neededCapacity;
            }
        }

        private void InternalAddLoaderPromise(IBasePromise promise, float weight = 1f, bool mustResolve = true)
        {
            weight = Mathf.Max(0.0001f, weight);

            _loadCount++;

            int weightIndex = _loadWeights.Count;
            _loadWeights.Add(weight);
            _loadWeightsFinished.Add(0f);
            _loadPercDirty = true;

            if (mustResolve)
            {
                _loadMustResolveCount++;
            }

            promise.BaseThen(
                () => { OnPromiseSuccess(_loadId, weightIndex, mustResolve); },
                exception => { OnPromiseFailed(_loadId, exception, weightIndex, mustResolve); },
                progress => { OnPromiseProgress(_loadId, weightIndex, progress); }
            );
        }

        //Start reporting the combined progress to a central promise
        public IPromise StartLoading(float timeOut = 60f)
        {
            Promise tmpPromise = new Promise();
            _loadPromise = tmpPromise;
            //Check if promises are already resolved
            CheckForLoadingFinished();
            if (_loadPromise != null)
            {
                _coroutineRunner.StartCoroutine(LoadingTimeOutCoroutine(timeOut));
            }

            return tmpPromise;
        }

        IEnumerator LoadingTimeOutCoroutine(float timeOut)
        {
            yield return null;
            while (true)
            {
                timeOut -= Time.deltaTime;
                if (timeOut <= 0f || _loadPromise == null)
                {
                    break;
                }

                CalculateLoadPercIfNeeded();

                yield return null;
            }

            if (_loadPromise != null)
            {
                if (_loadMustResolveSuccessCount >= _loadMustResolveCount)
                {
                    MarkLoaderPromiseFinished();
                }
                else
                {
                    CalculateLoadPercIfNeeded();

                    _loadPromise.Reject(new Exception(string.Format("{0} , loader promise timed out!", this)));
                    _loadPromise = null;
                    _loadId++;
                }
            }
        }

        private void OnPromiseSuccess(int loadId, int weightIndex, bool mustResolve)
        {
            if (_loadId != loadId)
            {
                //Ignore since it was for a previous load that has already finished
                return;
            }

            Debug.Log(string.Format("{0} , loader promise success!", this));
            _loadSuccessCount++;
            if (mustResolve)
            {
                _loadMustResolveSuccessCount++;
            }

            _loadFinishedCount++;
            _loadWeightsFinished[weightIndex] = _loadWeights[weightIndex];
            _loadPercDirty = true;
            CheckForLoadingFinished();
        }

        private void OnPromiseFailed(int loadId, Exception exception, int weightIndex, bool mustResolve)
        {
            if (_loadId != loadId)
            {
                //Ignore since it was for a previous load that has already finished
                return;
            }

            _loadFailedCount++;
            _loadFinishedCount++;

            if (mustResolve)
            {
                Debug.LogWarning(string.Format("{0} , required loader promise failed! Exception: {1}", this,
                    exception));
                _loadRequiredFailedCount++;
            }
            else
            {
                Debug.Log(string.Format("{0} , loader promise failed! Exception: {1}", this, exception));
                //Only count the weight for promises allowed to fail.
                _loadWeightsFinished[weightIndex] = _loadWeights[weightIndex];
                _loadPercDirty = true;
            }

            CheckForLoadingFinished();
        }

        private void OnPromiseProgress(int loadId, int weightIndex, float progress)
        {
            if (_loadId != loadId)
            {
                //Ignore since it was for a previous load that has already finished
                return;
            }

            _loadWeightsFinished[weightIndex] = _loadWeights[weightIndex] * progress;
            _loadPercDirty = true;
        }

        private void CheckForLoadingFinished()
        {
            if (_loadPromise != null)
            {
                if (_loadRequiredFailedCount > 0)
                {
                    CalculateLoadPercIfNeeded();

                    _loadPromise.Reject(new Exception(
                        string.Format("{0} , a required promise failed before the loader promise was created!", this)));
                    _loadPromise = null;
                    _loadId++;
                }
                else if (_loadFinishedCount >= _loadCount)
                {
                    MarkLoaderPromiseFinished();
                }
            }
        }

        private void MarkLoaderPromiseFinished()
        {
            Debug.Log(string.Format("{0} , promise loader finished!", this));

            _loadedPerc = 1f;
            _loadPercDirty = false;
            _loadPromise.ReportProgress(1f);

            _loadPromise.Resolve();
            _loadPromise = null;
            _loadId++;
        }

        private void CalculateLoadPercIfNeeded()
        {
            if (_loadPercDirty)
            {
                float totalWeightFinished = 0f;
                float totalWeight = 0f;
                for (int i = 0; i < _loadWeights.Count; i++)
                {
                    totalWeightFinished += _loadWeightsFinished[i];
                    totalWeight += _loadWeights[i];
                }

                float newPerc = Mathf.Min(totalWeightFinished / totalWeight, 1f);
                if (newPerc > _loadedPerc || AllowPercentDrops)
                {
                    _loadedPerc = newPerc;
                    _loadPromise.ReportProgress(newPerc);
                }

                _loadPercDirty = false;
            }
        }
    }
}
