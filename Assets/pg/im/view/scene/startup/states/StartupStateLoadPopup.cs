using pg.core.installer;
using pg.im.view.scene;
using pg.im.model.scene;

namespace pg.im.view
{
    public partial class StartupMediator
    {
        public class StartupStateLoadPopup : StartupState
        {
            private readonly LoadUnloadScenesSignal _loadUnloadScenesSignal;

            public StartupStateLoadPopup(StartupMediator mediator) : base(mediator)
            {
                _loadUnloadScenesSignal = mediator._loadUnloadScenesSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                _loadUnloadScenesSignal.Load(Scenes.Popup).Done
                (
                    () =>
                    {
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.PopupLoaded;

                        // Testing Popup
                        //#if UNITY_EDITOR && DEBUG
                        //Mediator.ShowPopup(YesNoPopupConfig.GetYesNoPopupConfig
                        //                  ("Start Game?", "Going to Start Game"))
                        //.Done((result) =>
                        //{
                        //    YesNoPopupResult popupResult = (YesNoPopupResult)result;
                        //    if (popupResult.Yes)
                        //    {
                        //        UnityEngine.Debug.Log("Yes Button Clicked.");
                        //        // Success.
                        //    }
                        //    else if(popupResult.No)
                        //    {
                        //        UnityEngine.Debug.Log("No Button Clicked.");
                        //    }
                        //});
                        //#endif
                    },
                exception =>
                {

                }
                );
            }
        }
    }
}