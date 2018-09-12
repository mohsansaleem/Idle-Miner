using Zenject;
using UnityEngine;
using pg.core.command;
using System;
using pg.im.model.remote;
using pg.im.model;
using pg.im.model.data;

namespace pg.im.command
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
