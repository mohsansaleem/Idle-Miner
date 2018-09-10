using System;
using System.Collections.Generic;
using pg.im.model;
using UnityEngine;
using UnityEngine.UI;

namespace pg.im.view
{
    public class GamePlayView : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

