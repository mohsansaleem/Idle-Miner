using System;
using System.IO;
using Newtonsoft.Json;
using PG.Core.Commands;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.RemoteDataModels;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Commands
{
    public class SaveUserDataCommand : BaseCommand
    {
        [Inject] private RemoteDataModel _remoteDataModel;

        public override void Execute()
        {
            try
            {
                string path = Path.Combine(Application.streamingAssetsPath, Constants.GameStateFile);

                _remoteDataModel.UserData.LastSaved = DateTime.Now;
                
                StreamWriter writer = new StreamWriter(path);
                writer.Write(JsonConvert.SerializeObject(_remoteDataModel.UserData, Formatting.Indented));
                writer.Flush();

                writer.Close();
            }
            catch(Exception ex)
            {
                Debug.LogError("Error while Saving User: "+ ex.ToString());
            }
        }
    }

}
