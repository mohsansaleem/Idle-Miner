using Zenject;
using UnityEngine;
using Newtonsoft.Json;
using pg.core.command;
using System;
using System.IO;
using pg.im.view;
using pg.im.model.remote;

namespace pg.im.command
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
