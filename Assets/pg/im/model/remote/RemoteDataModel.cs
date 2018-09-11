using pg.im.model.data;
using UniRx;
using Zenject;

namespace pg.im.model.remote
{
    public class RemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        [Inject] private readonly ShaftRemoteDataModel.Factory _shaftRemoteDataModelFactory;
        [Inject] private readonly ElevatorRemoteDataModel.Factory _elevatorRemoteDataModelFactory;
        [Inject] private readonly WarehouseRemoteDataModel.Factory _warehouseRemoteDataModelFactory;

        public UserData UserData;

        public ReactiveCollection<ShaftRemoteDataModel> Shafts;
        public ElevatorRemoteDataModel Elevator;
        public WarehouseRemoteDataModel Warehouse;

        public ReactiveProperty<double> IdleCash;
        public ReactiveProperty<double> Cash;
        public ReactiveProperty<double> SuperCash;

        public RemoteDataModel()
        {
            Shafts = new ReactiveCollection<ShaftRemoteDataModel>();

            IdleCash = new ReactiveProperty<double>(0.0);
            Cash = new ReactiveProperty<double>(0.0);
            SuperCash = new ReactiveProperty<double>(0.0);
        }

        public void SeedUserData(UserData userData)
        {
            UserData = userData;

            foreach(var shaftRemoteData in userData.UserShafts)
            {
                ShaftRemoteDataModel shaft = _shaftRemoteDataModelFactory.Create();
                shaft.SeedShaftRemoteData(shaftRemoteData);

                Shafts.Add(shaft);
            }

            ElevatorRemoteDataModel elevator = _elevatorRemoteDataModelFactory.Create();
            elevator.SeedElevatorRemoteData(userData.Elevator);
            Elevator = elevator;

            WarehouseRemoteDataModel warehouse = _warehouseRemoteDataModelFactory.Create();
            warehouse.SeedWarehouseRemoteData(userData.Warehouse);
            Warehouse = warehouse;

            IdleCash.Value = userData.IdleCash;
            Cash.Value = userData.Cash;
            SuperCash.Value = userData.SuperCash;
        }

        public void UpdateCash(double cash)
        {
            UserData.Cash = cash;
            Cash.Value = cash;
        }
    }
}

