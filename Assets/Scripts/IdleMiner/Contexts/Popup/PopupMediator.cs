using System;
using PG.Core.Contexts.Popup;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.view.popup.popupconfig;
using PG.IdleMiner.Views.Popup;
using Zenject;
using UniRx;

namespace PG.IdleMiner.view.popup
{
    public class PopupMediator : IInitializable, IDisposable
    {
        [Inject] private readonly PopupView _view;
        [Inject] private readonly PopupSystemDataModel _popupSystemDataModel;
        
        [Inject] private readonly SignalBus _signalBus;

        private readonly CompositeDisposable _disposables;

        public PopupMediator()
        {
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            // Init stuff.
            _popupSystemDataModel.Popups.ObserveAdd().Subscribe(OnPopupAdd).AddTo(_disposables);
            _popupSystemDataModel.Popups.ObserveRemove().Subscribe(OnPopupRemove).AddTo(_disposables);

            _view.OnPopupButtonClicked = OnPopupButtonClicked;
            
            // Listening to Popup Signal.
            _signalBus.Subscribe<OpenPopupSignal>(Execute);
        }

        public void Execute(OpenPopupSignal openPopupSignalParams)
        {
            PopupData popupData = new PopupData()
            {
                PopupConfig = (PopupConfig)openPopupSignalParams.PopupConfig,
                OnPopupComplete = openPopupSignalParams.OnPopupComplete
            };

            _popupSystemDataModel.Popups.Add(popupData);
        }

        private void OnPopupAdd(CollectionAddEvent<PopupData> popupAddEventData)
        {
            _view.AddPopup(popupAddEventData.Value);
        }

        private void OnPopupRemove(CollectionRemoveEvent<PopupData> popupRemoveEventData)
        {
            _view.RemovePop(popupRemoveEventData.Value);
        }

        private void OnPopupButtonClicked(PopupData popupData, PopupButtonData popupButtonData)
        {
            if (popupData.PopupConfig.ButtonData.Contains(popupButtonData))
            {
                IPopupResult popupResult = popupData.PopupConfig.GetPopupResult();

                popupResult.SelectedIndex = popupData.PopupConfig.ButtonData.IndexOf(popupButtonData);

                popupData.OnPopupComplete.Resolve(popupResult);

                _popupSystemDataModel.Popups.Remove(popupData);
            }
            else
            {
                throw new Exception("PopupMediator.OnPopupButtonClicked: Something went wrong.");
            }

        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OpenPopupSignal>(Execute);
            _disposables.Dispose();
        }
    }
}

