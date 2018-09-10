using Newtonsoft.Json;
using pg.im.model.data;
using pg.im.view.popup;
using UniRx;
using UnityEngine;
using Zenject;

namespace pg.im.model
{
    public class RemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        public ReactiveProperty<double> IdleCash;
        public ReactiveProperty<double> Cash;
        public ReactiveProperty<double> SuperCash;

        public RemoteDataModel()
        {
            IdleCash = new ReactiveProperty<double>(0.0);
            Cash = new ReactiveProperty<double>(0.0);
            SuperCash = new ReactiveProperty<double>(0.0);
        }

        public void SeedUserData(UserData userData)
        {
            Debug.LogError("UserLoaded: " + JsonConvert.SerializeObject(userData));

            IdleCash.Value = userData.IdleCash;
            Cash.Value = userData.Cash;
            SuperCash.Value = userData.SuperCash;

        }
    }
}

