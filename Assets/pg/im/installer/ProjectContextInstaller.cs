using pg.core.installer;
using pg.im.model;
using pg.im.model.remote;
using pg.im.model.scene;

namespace pg.im.installer
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