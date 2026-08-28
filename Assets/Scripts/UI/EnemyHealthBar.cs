using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

        private Transform targetTransform;

        public void Initialize(Transform target)
        {
            targetTransform = target;
        }

        public void UpdateHealth(float currentHealth, float maxHealth)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
        }

        void LateUpdate()
        {
            if (targetTransform != null)
            {
                transform.position = targetTransform.position + offset;
            }
        }
    }
}
