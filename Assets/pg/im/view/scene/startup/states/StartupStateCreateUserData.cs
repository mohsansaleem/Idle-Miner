using pg.im.installer;
using RSG;
using UnityEngine;
using pg.im.model.data;
using pg.im.model.remote;
using pg.im.model.scene;

namespace pg.im.view
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