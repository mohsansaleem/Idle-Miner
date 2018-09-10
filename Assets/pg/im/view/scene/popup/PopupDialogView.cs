using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using pg.im.view.ui.common;

namespace pg.im.view.popup
{
    public class PopupDialogView : MonoBehaviour
    {
        [Header("Prefabs")]
        public UIButton ButtonPrefab;

        [Header("References")]
        public Text Title;
        public Text Message;
        public RectTransform ButtonsPanel;

        public Action<PopupData, PopupButtonData> OnPopupButtonClicked;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetData(PopupData popupData)
        {
            Title.text = popupData.PopupConfig.Title;
            Message.text = popupData.PopupConfig.Description;

            Data = popupData;
        }

        public void AddButton(PopupButtonData popupButtonData)
        {
            // TODO: MS: Use the pool.
            UIButton popButton = Instantiate(ButtonPrefab, ButtonsPanel);

            popButton.Data = popupButtonData;

            popButton.SetLabel(popupButtonData.Text);
            popButton.AddListener(PopupButtonClicked);
        }

        public void PopupButtonClicked(UIButton button)
        {
            if (OnPopupButtonClicked != null)
            {
                OnPopupButtonClicked(Data, (PopupButtonData)button.Data);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public PopupData Data { get; private set; }
    }
}

