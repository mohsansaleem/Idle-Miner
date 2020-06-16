using PG.IdleMiner.Commands;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Views.GamePlay;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Contexts.GamePlay
{
    public class GamePlayInstaller : MonoInstaller
    {
        [SerializeField]
        public GamePlayView GamePlayView;

        public override void InstallBindings()
        {
            Container.Bind<GamePlayModel>().AsSingle();

            Container.DeclareSignal<AddShaftSignal>();
            Container.BindSignal<AddShaftSignal>().ToMethod<AddShaftCommand>((x) => x.Execute)
                .FromNew();

            Container.BindInstance(GamePlayView);
            Container.BindInterfacesTo<GamePlayMediator>().AsSingle();
        }
    }
}
