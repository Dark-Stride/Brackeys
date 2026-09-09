using Scripts.Systems.Aggro_System;
using UnityEngine;

namespace Scripts.Systems.Debugging
{
    [RequireComponent(typeof(AggroController))]
    public class AggroDebugVisualizer : MonoBehaviour
    {
        private AggroController aggro;
        private Transform lastTarget;

        private void Awake()
        {
            aggro = GetComponent<AggroController>();
        }

        private void Update()
        {
            Transform current = aggro.CurrentTarget;
            if (current != lastTarget)
            {
                string targetName = current != null ? current.name : "None";
                Debug.Log(
                    $"<color=orange>[AggroDebug]</color> <b>{gameObject.name}</b> switched focus to: <b>{targetName}</b>"
                );
                lastTarget = current;
            }
        }

        private void OnDrawGizmos()
        {
            if (aggro == null || aggro.CurrentTarget == null)
                return;

            // Draw line to current target in Scene view
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, aggro.CurrentTarget.position);
            Gizmos.DrawWireSphere(aggro.CurrentTarget.position, 0.4f);
        }
    }
}
