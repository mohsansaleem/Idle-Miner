using System;
using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Misc.UI
{
    public delegate void OnClickedDelegate(UIButton button);

    public class UIButton : MonoBehaviour
    {
        public Button Button;
        public Text Label;

        private event OnClickedDelegate _onClickedEvent;

        private int _clickListenerCount = 0;

        // arbitrary data associated with the button.
        public object Data { get; set; }

        public void AddListener(OnClickedDelegate onClickedDelegate)
        {
            _onClickedEvent += onClickedDelegate;
            _clickListenerCount++;

            if (_clickListenerCount == 1)
            {
                Button.onClick.AddListener (ButtonClicked);
            }
        }
        
        public void RemoveListener(OnClickedDelegate onClickedDelegate)
        {
            _onClickedEvent -= onClickedDelegate;
            _clickListenerCount--;

            if (_clickListenerCount == 0)
            {
                Button.onClick.RemoveListener (ButtonClicked);
            }
        }

        public void RemoveAllListeners()
        {
            Button.onClick.RemoveAllListeners();
            _onClickedEvent = null;
            _clickListenerCount = 0;
        }

        void OnDestroy()
        {
            RemoveAllListeners();
        }

        private void ButtonClicked()
        {
            if (_onClickedEvent != null)
            {
                _onClickedEvent.Invoke (this);
            }
        }

        public void SetLabel(string text)
        {
            if (Label != null)
            {
                Label.text = text;
            }
            else
            {
                throw new ArgumentException("Label Reference is null.");
            }
        }

        public void SetSprite(Sprite sprite)
        {
            Button.image.sprite = sprite;
        }
        
        public bool Visible
        {
            set
            {
                gameObject.SetActive(value);
            }
            
            get
            {
                return gameObject.activeSelf;
            }
        }
        
        public bool Interactable
        {
            set
            {
                Button.interactable = value;
            }

            get
            {
                return Button.interactable;
            }
        }
    }
}

