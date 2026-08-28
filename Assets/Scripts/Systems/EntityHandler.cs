using UnityEngine;
using Scripts.Items;

namespace Scripts.Systems
{
    public class EntityHandler : MonoBehaviour
    {
        [Header("On Death")]
        [SerializeField] private GameObject deathEffectPrefab;
        [SerializeField] private ItemSO[] lootTable;
        [SerializeField] private GameObject lootPickupPrefab;

        private HealthController health;

        void Awake()
        {
            health = GetComponent<HealthController>();
            if (health != null) health.OnDeath += HandleDeath;
        }

        private void HandleDeath()
        {
            // 1. Spawn Effect
            if (deathEffectPrefab) Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

            // 2. Drop Loot
            if (lootTable.Length > 0)
            {
                ItemSO droppedItem = lootTable[Random.Range(0, lootTable.Length)];
                GameObject loot = Instantiate(lootPickupPrefab, transform.position, Quaternion.identity);
                if (loot.TryGetComponent(out ItemInstance itemInstance))
                {
                    itemInstance.Initialize(droppedItem, 1);
                }
            }

            // 3. Clean up
            Destroy(gameObject);
        }
    }
}
