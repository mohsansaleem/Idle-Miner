using Zenject;
using UnityEngine;
using Newtonsoft.Json;
using pg.im.model;
using pg.im.model.data;
using pg.im.installer;
using pg.im.view.popup.popupconfig;
using pg.im.view.popup.popupresult;
using pg.core.command;
using RSG;
using System;
using System.IO;
using pg.im.view;

namespace pg.im.command
{
    public class CreateUserDataCommand : BaseCommand
    {
        //[Inject] private RemoteDataModel _remoteDataModel;
        [Inject] private readonly StartupModel _startupModel;

        public void Execute(CreateUserDataSignalParams commandParams)
        {
            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, Constants.GameStateFile);

                StreamWriter writer = new StreamWriter(path);
                writer.Write(JsonConvert.SerializeObject(commandParams.UserData, Formatting.Indented));
                writer.Flush();

                commandParams.OnUserCreated.Resolve();
            }
            catch(Exception ex)
            {
                commandParams.OnUserCreated.Reject(ex);
            }
        }
    }

}
