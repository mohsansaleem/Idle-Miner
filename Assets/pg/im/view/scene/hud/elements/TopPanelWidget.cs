using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanelWidget : MonoBehaviour
{
    public TextMeshProUGUI Value;
    public Slider Slider;

    public void SetData(double current, double total)
    {
        Value.text = current.ToString();
        Slider.value = (float)(current / total);
    }
}
