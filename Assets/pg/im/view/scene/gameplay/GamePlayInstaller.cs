using pg.im.model.scene;
using UnityEngine;
using Zenject;

namespace pg.im.view
{
    public class GamePlayInstaller : MonoInstaller
    {
        [SerializeField]
        public GamePlayView GamePlayView;

        public override void InstallBindings()
        {
            Container.Bind<GamePlayModel>().AsSingle();

            Container.BindInstance(GamePlayView);
            Container.BindInterfacesTo<GamePlayMediator>().AsSingle();
        }
    }
}
