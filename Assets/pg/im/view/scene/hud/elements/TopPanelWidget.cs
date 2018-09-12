using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace pg.im.view
{
    public class TopPanelWidget : MonoBehaviour
    {
        public TextMeshProUGUI Value;
        public Slider Slider;

        public void SetData(double current, double total)
        {
            Value.text = current.ToShort();
            Slider.value = (float)(current / total);
        }
    }
}
