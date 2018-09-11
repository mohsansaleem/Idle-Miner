using Zenject;
using UnityEngine;
using Newtonsoft.Json;
using pg.im.installer;
using pg.core.command;
using System;
using System.IO;
using pg.im.view;
using pg.im.model.scene;

namespace pg.im.command
{
    public class CreateUserDataCommand : BaseCommand
    {
        [Inject] private readonly StartupModel _startupModel;

        public void Execute(CreateUserDataSignalParams commandParams)
        {
            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, Constants.GameStateFile);

                StreamWriter writer = new StreamWriter(path);
                writer.Write(JsonConvert.SerializeObject(commandParams.UserData, Formatting.Indented));
                writer.Flush();
                writer.Close();

                commandParams.OnUserCreated.Resolve();
            }
            catch(Exception ex)
            {
                commandParams.OnUserCreated.Reject(ex);
            }
        }
    }

}
