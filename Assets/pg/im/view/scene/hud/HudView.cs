using UnityEngine;

namespace pg.im.view
{
    public class HudView : MonoBehaviour
    {
        [SerializeField]
        public TopPanelWidget _idleCashWidget;
        [SerializeField]
        public TopPanelWidget _cashWidget;
        [SerializeField]
        public TopPanelWidget _superCashWidget;

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

