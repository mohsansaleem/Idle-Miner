using pg.im.model.data;
using RSG;
using Zenject;

namespace pg.im.installer
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
            this.UserData = userData;
            OnUserCreated = promise;
        }
    }

}