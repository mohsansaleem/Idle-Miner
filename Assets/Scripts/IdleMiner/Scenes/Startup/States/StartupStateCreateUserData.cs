using PG.IdleMiner.Models.DataModels;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using RSG;
using UnityEngine;

namespace PG.IdleMiner.Scenes.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateCreateUserData : StartupState
        {
            private readonly RemoteDataModel _remoteDataModel;
            private readonly CreateUserDataSignal _createUserDataSignal;

            public StartupStateCreateUserData(StartupMediator mediator) : base(mediator)
            {
                _remoteDataModel = mediator._remoteDataModel;
                _createUserDataSignal = mediator._createUserDataSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                UserData userData = View.DefaultGameState.User;
                
                Promise promise = new Promise();

                promise.Then(
                    () => {
                        _remoteDataModel.SeedUserData(userData);
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.DataSeeded;
                    }
                ).Catch(e =>
                {
                    Debug.LogError("Exception Creating new User: " + e.ToString());
                });

                CreateUserDataSignalParams param = new CreateUserDataSignalParams(userData, promise);

                _createUserDataSignal.Fire(param);
            }
        }
    }
}