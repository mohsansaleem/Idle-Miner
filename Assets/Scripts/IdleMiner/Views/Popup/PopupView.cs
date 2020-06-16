using System;
using System.Collections.Generic;
using PG.Core.Contexts.Popup;
using PG.IdleMiner.view.popup;
using UnityEngine;

namespace PG.IdleMiner.Views.Popup
{
    public class PopupView : MonoBehaviour
    {
        [Header("Prefabs")]
        public PopupDialogView PopupDialogViewPrefab;

        [Header("References")]
        public RectTransform PopupDialogsContainer;

        public Action<PopupData, PopupButtonData> OnPopupButtonClicked;

        private readonly Dictionary<PopupData, PopupDialogView> _createdPopupDialogViews = new Dictionary<PopupData, PopupDialogView>();

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void AddPopup(PopupData popupData)
        {
            // TODO: MS: Use the pool.
            PopupDialogView popupDialogView = Instantiate(PopupDialogViewPrefab, PopupDialogsContainer);

            popupDialogView.OnPopupButtonClicked = OnPopupButtonClicked;
            popupDialogView.transform.localScale = Vector3.one;
            popupDialogView.SetData(popupData);

            foreach (IPopupButtonData popupButtonData in popupData.PopupConfig.ButtonData)
            {
                popupDialogView.AddButton((PopupButtonData)popupButtonData);
            }

            _createdPopupDialogViews.Add(popupData, popupDialogView);
        }

        public void RemovePop(PopupData popupData)
        {
            if (_createdPopupDialogViews.ContainsKey(popupData))
            {
                PopupDialogView popupDialogView = _createdPopupDialogViews[popupData];
                _createdPopupDialogViews.Remove(popupData);

                // TODO: MS: Return to pool.
                Destroy(popupDialogView.gameObject);
            }
            else
            {
                throw new Exception("PopupView.RemovePop: PopupData is not in the Dictionary.");
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

