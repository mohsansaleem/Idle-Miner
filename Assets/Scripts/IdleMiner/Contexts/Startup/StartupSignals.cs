using PG.IdleMiner.Models.DataModels;
using RSG;
using Zenject;

namespace PG.IdleMiner.Contexts.Startup
{
    public class LoadStaticDataSignal
    {
        public Promise Promise;
    }

    public class LoadUserDataSignal
    {
        public Promise Promise;
    }
    public class SaveUserDataSignal { }

    public class CreateUserDataSignal
    {
        public UserData UserData;
        public Promise OnUserCreated;

        public CreateUserDataSignal(UserData userData, Promise promise)
        {
            UserData = userData;
            OnUserCreated = promise;
        }
    }
}