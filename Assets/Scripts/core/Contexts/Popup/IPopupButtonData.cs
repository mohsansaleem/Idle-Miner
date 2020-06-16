using UnityEngine;

namespace PG.Core.Contexts.Popup
{
    public interface IPopupButtonData
    {
        Sprite Sprite { get; set; }
        string Text { get; set; }
    }
}