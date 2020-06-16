using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Contexts.Startup
{
    public class StartupView : MonoBehaviour
    {
        [Header("Assets")]
        public DefaultGameState DefaultGameState;

        [Header("References")]
        public Slider ProgressBar;

        public void SetProgress(float progress)
        {
            ProgressBar.value = progress;
        }

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

