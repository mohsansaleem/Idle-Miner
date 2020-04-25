using UnityEngine;

namespace PG.Core.Scenes.Popup
{
    public interface IPopupButtonData
    {
        Sprite Sprite { get; set; }
        string Text { get; set; }
    }
}