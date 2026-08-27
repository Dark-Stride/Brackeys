using System.Collections.Generic;
using Scripts.Systems.Aggro_System;
using UnityEngine;

namespace Scripts.Companion
{
    public class PostCombatDialogueTrigger : MonoBehaviour
    {
        [Header("Room Trigger Area")]
        [SerializeField]
        private float roomRadius = 12f;

        [SerializeField]
        private LayerMask enemyLayer;

        [Header("Dialogue Content")]
        [TextArea(2, 3)]
        public string[] conversationLines = new string[]
        {
            "Player: Hey... you okay back there? You dropped your blade twice.",
            "Brother: Yeah, just... hand slipped. Must be the rust on these old hilts. Don't worry about it.",
            "Player: Let's stay focused. We're getting close.",
        };

        private bool conversationTriggered = false;
        private bool inCombat = true;

        private void Update()
        {
            if (conversationTriggered)
                return;

            // Check if any hostile enemies remain in this zone
            Collider2D[] remainingEnemies = Physics2D.OverlapCircleAll(
                transform.position,
                roomRadius,
                enemyLayer
            );

            if (inCombat && remainingEnemies.Length == 0)
            {
                inCombat = false;
                TriggerPostCombatDialogue();
            }
        }

        private void TriggerPostCombatDialogue()
        {
            conversationTriggered = true;
            Debug.Log(
                $"<color=cyan>[Dialogue]</color> Starting Post-Combat conversation with Brother!"
            );

            // In a full UI build, pass conversationLines to your DialogueBoxUI:
            // DialogueBoxUI.Instance.StartDialogue(conversationLines);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, roomRadius);
        }
    }
}
