using System.Collections;
using UnityEngine;

namespace Scripts.Systems
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField]
        private float displayDuration = 3f;

        private bool hasAdvanced = false;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(displayDuration);
            ProceedToMenu();
        }

        private void Update()
        {
            if (!hasAdvanced && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
            {
                ProceedToMenu();
            }
        }

        private void ProceedToMenu()
        {
            if (hasAdvanced)
                return;
            hasAdvanced = true;

            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.SetStep(GameStep.MainMenu);
            }
        }
    }
}
