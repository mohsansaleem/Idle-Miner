using PG.IdleMiner.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Views.Hud
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
