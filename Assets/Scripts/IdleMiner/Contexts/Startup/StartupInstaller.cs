using PG.IdleMiner.Commands;
using PG.IdleMiner.Models.MediatorModels;
using RSG;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Contexts.Startup
{
    public class StartupInstaller : MonoInstaller
    {
        [SerializeField]
        public StartupView StartupView;

        public override void InstallBindings()
        {
            Container.DeclareSignal<LoadStaticDataSignal>();
            Container.BindSignal<LoadStaticDataSignal>()
                .ToMethod<LoadStaticDataCommand>((x) => x.Execute)
                .FromNew();

            Container.DeclareSignal<LoadUserDataSignal>();
            Container.BindSignal<LoadUserDataSignal>()
                .ToMethod<LoadUserDataCommand>((x) => x.Execute)
                .FromNew();

            Container.DeclareSignal<SaveUserDataSignal>();
            Container.BindSignal<SaveUserDataSignal>()
                .ToMethod<SaveUserDataCommand>((x) => x.Execute)
                .FromNew();

            Container.DeclareSignal<CreateUserDataSignal>();
            Container.BindSignal<CreateUserDataSignal>()
                .ToMethod<CreateUserDataCommand>((x) => x.Execute)
                .FromNew();

            Container.Bind<StartupModel>().AsSingle();

            Container.BindInstance(StartupView);
            Container.BindInterfacesTo<StartupMediator>().AsSingle();
        }
    }
}
