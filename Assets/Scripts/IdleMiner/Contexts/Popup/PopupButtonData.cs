using System;
using PG.Core.Contexts.Popup;
using UnityEngine;


namespace PG.IdleMiner.view.popup
{
    [Serializable]
    public class PopupButtonData : IPopupButtonData
    {
        public PopupButtonData(string text)
        {
            Text = text;
        }

        public Sprite Sprite { get; set; }
        public string Text { get; set; }
    }
}