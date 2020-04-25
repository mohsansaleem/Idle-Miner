using PG.IdleMiner.Misc;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PG.IdleMiner.view
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
