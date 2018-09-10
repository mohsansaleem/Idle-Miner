using pg.core.installer;
using pg.im.command;
using pg.im.model;
using RSG;
using Zenject;

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
        }
    }
}