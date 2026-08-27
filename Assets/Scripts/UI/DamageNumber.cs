using UnityEngine;
using TMPro;

namespace Scripts.UI
{
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private float floatSpeed = 2f;
        [SerializeField] private float fadeDuration = 0.8f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color criticalColor = Color.yellow;

        private float timer;
        private Color currentColor;

        public void Setup(float amount, bool isCritical)
        {
            timer = 0f;
            if (damageText != null)
            {
                damageText.text = Mathf.RoundToInt(amount).ToString();
                damageText.fontSize = isCritical ? 36 : 26;
                currentColor = isCritical ? criticalColor : normalColor;
                damageText.color = currentColor;
            }
        }

        void Update()
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            timer += Time.deltaTime;

            if (damageText != null && fadeDuration > 0f)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                currentColor.a = alpha;
                damageText.color = currentColor;
            }

            if (timer >= fadeDuration) Destroy(gameObject);
        }
    }
}
