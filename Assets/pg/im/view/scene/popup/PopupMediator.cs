using System;
using System.Collections.Generic;
using Zenject;
using UniRx;
using pg.im.model;
using pg.core.view;
using pg.im.view.popup.popupconfig;

namespace pg.im.view.popup
{
    public class PopupMediator : IInitializable, IDisposable
    {
        [Inject] private readonly PopupView _view;
        [Inject] private readonly PopupSystemDataModel _popupSystemDataModel;

        [Inject] private readonly OpenPopupSignal _openPopupSignal;

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
        }

        public void Execute(OpenPopupSignalParams openPopupSignalParams)
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
            _disposables.Dispose();
        }
    }
}

