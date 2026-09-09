using Scripts.Companion;
using Scripts.Player;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Systems.Debugging
{
    public class GameFlowDebugGUI : MonoBehaviour
    {
        [SerializeField]
        private KeyCode toggleKey = KeyCode.F1;
        private bool showGUI = true;

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey) || Input.GetKeyDown(KeyCode.BackQuote))
            {
                showGUI = !showGUI;
            }
        }

        private void OnGUI()
        {
            if (!showGUI)
                return;

            GUI.Box(new Rect(10, 10, 320, 340), "<b>=== DEV CHEAT & FLOW MONITOR ===</b>");

            // Game Step Display
            string currentStepName =
                GameFlowManager.Instance != null
                    ? GameFlowManager.Instance.CurrentStep.ToString()
                    : "No GameFlowManager";

            GUI.Label(
                new Rect(20, 35, 300, 25),
                $"<b>Current Step:</b> <color=yellow>{currentStepName}</color>"
            );

            // Entity Status
            var player = FindFirstObjectByType<Player.Player>();
            var playerHealth = player != null ? player.GetComponent<HealthController>() : null;
            if (playerHealth != null)
            {
                GUI.Label(
                    new Rect(20, 60, 300, 20),
                    $"Player HP: {playerHealth.CurrentHealth:0}/{playerHealth.MaxHealth:0}"
                );
            }

            var brother = FindFirstObjectByType<BrotherAI>();
            if (brother != null)
            {
                var broHealth = brother.GetComponent<HealthController>();
                GUI.Label(
                    new Rect(20, 80, 300, 20),
                    $"Brother Mode: <color=cyan>{brother.CurrentActMode}</color> | HP: {broHealth.CurrentHealth:0}"
                );
            }

            // Quick Jump Buttons
            GUI.Label(new Rect(20, 105, 300, 20), "<b>--- Jump to Story Step ---</b>");

            if (GUI.Button(new Rect(20, 130, 140, 25), "Cutscene 1"))
                JumpTo(GameStep.Cutscene1);
            if (GUI.Button(new Rect(170, 130, 140, 25), "Gameplay 1 (Tutorial)"))
                JumpTo(GameStep.Gameplay1_Tutorial);

            if (GUI.Button(new Rect(20, 160, 140, 25), "Cutscene 2"))
                JumpTo(GameStep.Cutscene2);
            if (GUI.Button(new Rect(170, 160, 140, 25), "Gameplay 2 (Mission)"))
                JumpTo(GameStep.Gameplay2_CaptureMission);

            if (GUI.Button(new Rect(20, 190, 140, 25), "Cutscene 3"))
                JumpTo(GameStep.Cutscene3);
            if (GUI.Button(new Rect(170, 190, 140, 25), "Gameplay 3 (HQ/Dungeon)"))
                JumpTo(GameStep.Gameplay3_GuildHQ_Dungeon);

            if (GUI.Button(new Rect(20, 220, 140, 25), "Cutscene 4 (Betrayal)"))
                JumpTo(GameStep.Cutscene4);
            if (GUI.Button(new Rect(170, 220, 140, 25), "Gameplay 4 (Boss Fight)"))
                JumpTo(GameStep.Gameplay4_FinalBetrayalFight);

            if (GUI.Button(new Rect(20, 250, 140, 25), "Cutscene 5"))
                JumpTo(GameStep.Cutscene5);
            if (GUI.Button(new Rect(170, 250, 140, 25), "Credits Scene"))
                JumpTo(GameStep.Credits);

            // Cheats
            if (GUI.Button(new Rect(20, 290, 140, 25), "Heal Player (Full)"))
            {
                if (playerHealth != null)
                    playerHealth.Heal(999f);
            }
            if (GUI.Button(new Rect(170, 290, 140, 25), "Kill All Enemies"))
            {
                KillAllEnemies();
            }
        }

        private void JumpTo(GameStep step)
        {
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.SetStep(step);
            }
        }

        private void KillAllEnemies()
        {
            var enemies = FindObjectsByType<Enemies.Enemy>(FindObjectsSortMode.None);
            foreach (var e in enemies)
            {
                if (e.TryGetComponent(out HealthController hp))
                    hp.TakeDamage(new Core.DamageData { amount = 9999f });
            }
            Debug.Log(
                $"<color=red>[Cheat]</color> Killed {enemies.Length} enemies in current room."
            );
        }
    }
}
