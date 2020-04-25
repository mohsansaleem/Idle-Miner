using System;
using System.IO;
using Newtonsoft.Json;
using PG.Core.Commands;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using RSG;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Commands
{
    public class LoadUserDataCommand : BaseCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private readonly StartupModel _startupModel;

        public void Execute(Promise onCompletePromise)
        {
            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, Constants.GameStateFile);

                if (File.Exists(path))
                {
                    StreamReader reader = new StreamReader(path);
                    UserData userData = JsonConvert.DeserializeObject<UserData>(reader.ReadToEnd());
                    reader.Close();

                    _remoteDataModel.SeedUserData(userData);

                    Debug.Log("UserData Loaded From: " + path);

                    onCompletePromise.Resolve();
                }
                else
                    onCompletePromise.Reject(new FileNotFoundException(Constants.GameStateFile + " doesn't Exist."));

            }
            catch (Exception ex)
            {
                onCompletePromise.Reject(ex);
            }
        }
    }

}
