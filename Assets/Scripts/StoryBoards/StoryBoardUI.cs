using System;
using System.Collections;
using Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Storyboards
{
    public class StoryboardUI : MonoBehaviour
    {
        [Header("UI Bindings")]
        [SerializeField]
        private Image displayImage;

        [SerializeField]
        private TMP_Text captionText;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float fadeDuration = 0.3f;

        [Header("Input")]
        [SerializeField]
        private InputReader input;

        private StoryboardSO currentStoryboard;
        private int currentSlideIndex = 0;
        private bool isTransitioning = false;
        private Action onCompleteCallback;

        private void OnEnable()
        {
            if (input != null)
                input.InteractionEvent += HandleAdvanceInput;
        }

        private void OnDisable()
        {
            if (input != null)
                input.InteractionEvent -= HandleAdvanceInput;
        }

        public void PlayStoryboard(StoryboardSO storyboard, Action onComplete)
        {
            if (storyboard == null || storyboard.slides.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            currentStoryboard = storyboard;
            currentSlideIndex = 0;
            onCompleteCallback = onComplete;
            gameObject.SetActive(true);

            StartCoroutine(ShowSlide(currentSlideIndex));
        }

        private void HandleAdvanceInput()
        {
            if (isTransitioning || currentStoryboard == null)
                return;

            currentSlideIndex++;
            if (currentSlideIndex < currentStoryboard.slides.Count)
            {
                StartCoroutine(ShowSlide(currentSlideIndex));
            }
            else
            {
                FinishCutscene();
            }
        }

        private IEnumerator ShowSlide(int index)
        {
            isTransitioning = true;

            // Fade Out
            yield return FadeCanvas(1f, 0f, fadeDuration);

            // Swap Content
            var slide = currentStoryboard.slides[index];
            if (displayImage != null)
                displayImage.sprite = slide.slideImage;
            if (captionText != null)
                captionText.text = slide.captionText;

            if (slide.voiceOrSfx != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(slide.voiceOrSfx);
            }

            // Fade In
            yield return FadeCanvas(0f, 1f, fadeDuration);

            isTransitioning = false;
        }

        private IEnumerator FadeCanvas(float start, float end, float duration)
        {
            if (canvasGroup == null)
                yield break;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = end;
        }

        private void FinishCutscene()
        {
            gameObject.SetActive(false);
            var callback = onCompleteCallback;
            onCompleteCallback = null;
            callback?.Invoke();
        }
    }
}
