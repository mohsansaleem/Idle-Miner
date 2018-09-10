using System;
using pg.core.view;
using UnityEngine;


namespace pg.im.view.popup
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