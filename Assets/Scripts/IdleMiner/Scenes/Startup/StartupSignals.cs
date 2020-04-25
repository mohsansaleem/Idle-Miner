using PG.IdleMiner.Models.DataModels;
using RSG;
using Zenject;

namespace PG.IdleMiner.Scenes.Startup
{
    public class LoadStaticDataSignal : Signal<Promise, LoadStaticDataSignal> { }
    public class LoadUserDataSignal : Signal<Promise, LoadUserDataSignal> { }
    public class SaveUserDataSignal : Signal<SaveUserDataSignal> { }
    public class CreateUserDataSignal : Signal<CreateUserDataSignalParams, CreateUserDataSignal> { }
    public class CreateUserDataSignalParams
    {
        public UserData UserData;
        public Promise OnUserCreated;

        public CreateUserDataSignalParams(UserData userData, Promise promise)
        {
            UserData = userData;
            OnUserCreated = promise;
        }
    }

}