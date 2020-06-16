using PG.IdleMiner.Views.Hud;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.view
{
    public class HudInstaller : MonoInstaller
    {
        [SerializeField]
        public HudView HudView;

        public override void InstallBindings()
        {
            Container.BindInstance(HudView);
            Container.BindInterfacesTo<HudMediator>().AsSingle();
        }
    }
}
