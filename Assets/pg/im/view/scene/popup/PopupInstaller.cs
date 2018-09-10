using pg.core.view;
using UnityEngine;
using Zenject;

namespace pg.im.view.popup
{
    public class PopupInstaller : MonoInstaller
    {
        [SerializeField]
        public PopupView PopupView;

        public override void InstallBindings()
        {
            Container.BindInstance(PopupView);
            Container.BindInterfacesTo<PopupMediator>().AsSingle();

            // Listening to Popup Signal.
            Container.BindSignal<OpenPopupSignalParams, OpenPopupSignal>()
                .To<PopupMediator>((x, popupSignalParams) => x.Execute(popupSignalParams))
                .AsCached();
        }
    }
}
