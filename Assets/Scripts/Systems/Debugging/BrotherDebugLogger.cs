using Scripts.Companion;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Systems.Debugging
{
    [RequireComponent(typeof(BrotherAI))]
    public class BrotherDebugLogger : MonoBehaviour
    {
        private BrotherAI brother;
        private HealthController health;
        private BrotherActMode lastMode;

        private void Awake()
        {
            brother = GetComponent<BrotherAI>();
            health = GetComponent<HealthController>();
            lastMode = brother.CurrentActMode;
        }

        private void Update()
        {
            if (brother.CurrentActMode != lastMode)
            {
                Debug.Log(
                    $"<color=yellow>[BrotherDebug]</color> Act Mode changed from <b>{lastMode}</b> to <b><color=magenta>{brother.CurrentActMode}</color></b>"
                );
                lastMode = brother.CurrentActMode;
            }
        }

        [ContextMenu("Debug: Force Act 1 (Loyal)")]
        public void SetAct1() => brother.SetActMode(BrotherActMode.Act1_Loyal);

        [ContextMenu("Debug: Force Act 2 (Sabotage)")]
        public void SetAct2() => brother.SetActMode(BrotherActMode.Act2_Sabotage);

        [ContextMenu("Debug: Force Act 3 (Hostile)")]
        public void SetAct3() => brother.SetActMode(BrotherActMode.Act3_Hostile);
    }
}
