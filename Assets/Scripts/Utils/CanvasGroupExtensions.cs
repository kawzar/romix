using System;
using System.Collections;
using UnityEngine;

namespace Romix.Utils
{
    public static class CanvasGroupExtensions
    {
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, float duration, Action onComplete = null)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, time / duration);
                yield return null;
            }

            canvasGroup.alpha = 1;
            onComplete?.Invoke();
        }

        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, float duration, Action onComplete = null)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
                yield return null;
            }

            canvasGroup.alpha = 0;
            onComplete?.Invoke();
        }
    }
}