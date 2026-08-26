using UnityEngine;
using Scripts.UI;

namespace Scripts.Systems
{
    public class DamageNumberManager : MonoBehaviour
    {
        public static DamageNumberManager Instance { get; private set; }

        [SerializeField] private GameObject damageNumberPrefab;
        [SerializeField] private Canvas worldSpaceCanvas;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SpawnDamageNumber(Vector3 position, float amount, bool isCritical)
        {
            if (damageNumberPrefab == null) return;

            Vector3 spawnPos = position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1f), 0f);
            Transform parentTransform = worldSpaceCanvas != null ? worldSpaceCanvas.transform : transform;

            GameObject obj = Instantiate(damageNumberPrefab, spawnPos, Quaternion.identity, parentTransform);
            if (obj.TryGetComponent(out DamageNumber damageNum))
            {
                damageNum.Setup(amount, isCritical);
            }
        }
    }
}
