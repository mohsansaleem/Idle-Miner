using System;
using System.IO;
using PG.IdleMiner.Contexts.Startup;
using Newtonsoft.Json;
using PG.Core.Commands;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.MediatorModels;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Commands
{
    public class CreateUserDataCommand : BaseCommand
    {
        [Inject] private readonly StartupModel _startupModel;

        public void Execute(CreateUserDataSignal commandParams)
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
