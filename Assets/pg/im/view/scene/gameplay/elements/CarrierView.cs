using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace pg.im.view
{
    [RequireComponent(typeof(RectTransform))]
    public class CarrierView : MonoBehaviour
    {
        [SerializeField]
        private Image _fillBar;

        [SerializeField]
        private TextMeshProUGUI _carriedCount;

        [SerializeField]
        public RectTransform RectTransform;

        public void LoadStuff(double current, double capacity)
        {
            _fillBar.fillAmount = (float)(current / capacity);
            _carriedCount.text = current.ToShort();
        }
    }
}