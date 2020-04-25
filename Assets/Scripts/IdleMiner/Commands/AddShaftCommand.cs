using System;
using PG.Core.Commands;
using PG.IdleMiner.Models;
using PG.IdleMiner.Models.DataModels;
using PG.IdleMiner.Models.RemoteDataModels;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Commands
{
    public class AddShaftCommand : BaseCommand
    {
        [Inject] private StaticDataModel _staticDataModel;
        [Inject] private RemoteDataModel _remoteDataModel;

        public override void Execute()
        {
            try
            {
                ShaftRemoteData shaftData = _staticDataModel.UnlockShaft(_remoteDataModel.Shafts.Count);
                if (shaftData != null)
                {
                    _remoteDataModel.UserData.UserShafts.Add(shaftData);
                    _remoteDataModel.AddShaft(shaftData);
                }
                else
                    Debug.LogError("Error: No more shafts.");
            }
            catch(Exception ex)
            {
                Debug.LogError("Error while Adding Shaft: "+ ex.ToString());
            }
        }
    }

}
