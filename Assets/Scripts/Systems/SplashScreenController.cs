using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Scripts.Systems
{
    public class SplashScreenController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float displayDuration = 3f;

        [SerializeField]
        private string targetMenuScene = "01_MainMenu";

        private bool hasAdvanced = false;

        private IEnumerator Start()
        {
            Debug.Log($"<color=cyan>[SplashScreen]</color> Timer started ({displayDuration}s)...");
            // Realtime ensures it counts down even if timeScale is 0
            yield return new WaitForSecondsRealtime(displayDuration);
            ProceedToMenu();
        }

        private void Update()
        {
            if (hasAdvanced)
                return;

            // Check if player clicked or pressed any key
            if (CheckAnyInputPressed())
            {
                Debug.Log("<color=cyan>[SplashScreen]</color> Skip input detected.");
                ProceedToMenu();
            }
        }

        private bool CheckAnyInputPressed()
        {
#if ENABLE_INPUT_SYSTEM
            // New Input System checks
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                return true;
            if (
                Mouse.current != null
                && (
                    Mouse.current.leftButton.wasPressedThisFrame
                    || Mouse.current.rightButton.wasPressedThisFrame
                )
            )
                return true;
            if (
                Gamepad.current != null
                && Gamepad.current.allControls.Count > 0
                && Gamepad.current.buttonSouth.wasPressedThisFrame
            )
                return true;
            return false;
#else
            // Legacy input fallback
            return Input.anyKeyDown || Input.GetMouseButtonDown(0);
#endif
        }

        private void ProceedToMenu()
        {
            if (hasAdvanced)
                return;
            hasAdvanced = true;

            Debug.Log("<color=green>[SplashScreen]</color> Proceeding to Main Menu...");

            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.SetStep(GameStep.MainMenu);
            }
            else
            {
                Debug.LogWarning(
                    "[SplashScreen] GameFlowManager.Instance not found. Loading scene directly via SceneManager."
                );
                SceneManager.LoadScene(targetMenuScene);
            }
        }
    }
}
