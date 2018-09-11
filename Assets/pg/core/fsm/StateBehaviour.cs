using UniRx;
using UnityEngine;

namespace pg.core
{
    public class StateBehaviour
    {
        protected CompositeDisposable _disposables;

        public virtual void OnStateEnter()
        {
            Debug.Log(string.Format("{0} , OnStateEnter()", this));

            _disposables = new CompositeDisposable();
        }

        public virtual void OnStateExit()
        {
            Debug.Log(string.Format("{0} , OnStateExit()", this));

            _disposables.Dispose();
        }

        public virtual bool IsValidOpenState()
        {
            return false;
        }

        public virtual void Tick()
        {

        }
    }
}
