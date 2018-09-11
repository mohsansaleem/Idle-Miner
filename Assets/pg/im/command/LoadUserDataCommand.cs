using Zenject;
using UnityEngine;
using Newtonsoft.Json;
using pg.im.model.data;
using pg.core.command;
using RSG;
using System;
using pg.im.view;
using System.IO;
using pg.im.model.remote;
using pg.im.model.scene;

namespace pg.im.command
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
