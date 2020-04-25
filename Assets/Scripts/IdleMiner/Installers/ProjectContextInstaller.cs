using PG.Core.Installers;
using PG.IdleMiner.Models;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;

namespace PG.IdleMiner.Installers
{
    public class ProjectContextInstaller : CoreContextInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.Bind<PopupSystemDataModel>().AsSingle();
            Container.Bind<StaticDataModel>().AsSingle();
            Container.Bind<RemoteDataModel>().AsSingle();
            Container.Bind<HudModel>().AsSingle();

            Container.BindFactory<ShaftRemoteDataModel, ShaftRemoteDataModel.Factory>();
            Container.BindFactory<ElevatorRemoteDataModel, ElevatorRemoteDataModel.Factory>();
            Container.BindFactory<WarehouseRemoteDataModel, WarehouseRemoteDataModel.Factory>();
        }
    }
}