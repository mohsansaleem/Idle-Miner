using PG.Core.Contexts.Popup;
using PG.IdleMiner.Views.Popup;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.view.popup
{
    public class PopupInstaller : MonoInstaller
    {
        [SerializeField]
        public PopupView PopupView;

        public override void InstallBindings()
        {
            Container.BindInstance(PopupView);
            Container.BindInterfacesTo<PopupMediator>().AsSingle();
        }
    }
}
