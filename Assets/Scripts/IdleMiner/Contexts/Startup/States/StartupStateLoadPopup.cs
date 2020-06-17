using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.view.popup.popupconfig;
using PG.IdleMiner.view.popup.popupresult;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadPopup : StartupState
        {
            public StartupStateLoadPopup(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                LoadUnloadScenesSignal.Load(ProjectScenes.Popup, SignalBus).Done
                (
                    () =>
                    {
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadStaticData;

                        // Testing Popup
                        /*
                        #if UNITY_EDITOR && DEBUG
                        Mediator.ShowPopup(YesNoPopupConfig.GetYesNoPopupConfig
                                          ("Start Game?", "Going to Start Game"))
                        .Done((result) =>
                        {
                            YesNoPopupResult popupResult = (YesNoPopupResult)result;
                            if (popupResult.Yes)
                            {
                                StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadStaticData;
                                UnityEngine.Debug.Log("Yes Button Clicked.");
                                // Success.
                            }
                            else if(popupResult.No)
                            {
                                UnityEngine.Debug.Log("No Button Clicked.");
                            }
                        });
                        #endif
                        */
                    },
                    exception => { }
                );
            }
        }
    }
}