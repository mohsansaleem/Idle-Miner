using pg.core.command;
using pg.im.model;
using pg.im.model.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using RSG;
using Zenject;
using System.IO;
using pg.im.view;

namespace pg.im.command
{
    public class LoadStaticDataCommand : BaseCommand
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        public void Execute(Promise onCompletePromise)
        {
            var sequence = Promise.Sequence(
                () => LoadMetaJson(Constants.MetaDataFile)
                // Add other Jsons or asset bundles etc.
            );

            sequence
                .Then(() =>
                    {
                        Debug.Log(string.Format("{0} , static data load completed!", this));
                        onCompletePromise.Resolve();
                    }
                )
                .Catch(e =>
                    {
                        Debug.LogError(string.Format("{0} : failed to load static data, error message = {1}\n{2}", this,
                            e.Message, e.StackTrace));
                        onCompletePromise.Reject(e);
                    }
                );
        }

        // For now just loading everything from StreamingAssets. Proper way would be loading it from AssetBudles.
        private IPromise LoadMetaJson(string metaFileName)
        {
            Promise promiseReturn = new Promise();

            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, metaFileName);
                
                StreamReader reader = new StreamReader(path);
                MetaData metaData = JsonConvert.DeserializeObject<MetaData>(reader.ReadToEnd());
                reader.Close();

                _staticDataModel.SeedMetaData(metaData);

                promiseReturn.Resolve();
            }
            catch(Exception ex)
            {
                promiseReturn.Reject(ex);
            }

            return promiseReturn;
        }
    }
}