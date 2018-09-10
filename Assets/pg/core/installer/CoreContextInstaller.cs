using Zenject;
using pg.core.view;
using pg.core.command;

namespace pg.core.installer
{
    public class CoreContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<LoadSceneSignal>();
            Container.BindSignal<LoadSceneCommandParams, LoadSceneSignal>()
                .To<LoadSceneCommand>((x, loadParams) => x.Execute(loadParams))
                .AsTransient();

            Container.DeclareSignal<LoadUnloadScenesSignal>();
            Container.BindSignal<LoadUnloadScenesCommandParams, LoadUnloadScenesSignal>()
                .To<LoadUnloadScenesCommand>((x, loadParams) => x.Execute(loadParams))
                .AsTransient();

            Container.DeclareSignal<UnloadSceneSignal>();
            Container.BindSignal<LoadSceneCommandParams, UnloadSceneSignal>()
                .To<UnloadSceneCommand>((x, loadParams) => x.Execute(loadParams))
                .AsTransient();

            Container.DeclareSignal<UnloadAllScenesExceptSignal>();
            Container.BindSignal<LoadSceneCommandParams, UnloadAllScenesExceptSignal>()
                .To<UnloadAllScenesExceptCommand>((x, loadParams) => x.Execute(loadParams))
                .AsTransient();

            Container.DeclareSignal<OpenPopupSignal>();

            Container.Bind<PromiseLoader>().AsTransient();

            Container.BindInterfacesTo<AsyncSceneLoader>().AsTransient();
            

            Container.DeclareSignal<RequestStateChangeSignal>();
            Container.DeclareSignal<SendSceneParamsSignal>();
        }
    }
}