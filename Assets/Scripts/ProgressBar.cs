using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image progressImage;
        public Action<float> OnProgress;
        public Action OnCompleted;
        
        private Coroutine animationCoroutine;


        public void SetProgress(float progress, float speed = 1f)
        {
            
            if (progress < 0 || progress > 1)
                progress = Mathf.Clamp01(progress);
            if (Math.Abs(progress - progressImage.fillAmount) < 0.1f)
                return;
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(MakeProgress(progress, speed));
        }

        private IEnumerator MakeProgress(float progress, float speed)
        {
            float time = 0f;
            float initialProgress = progressImage.fillAmount;
            while (time < 1)
            {
                progressImage.fillAmount = Mathf.Lerp(initialProgress, progress, speed);
                time += Time.deltaTime * speed;

                OnProgress?.Invoke(progressImage.fillAmount);
                yield return null;
            }

            progressImage.fillAmount = progress;
            OnProgress?.Invoke(progressImage.fillAmount);
            OnCompleted?.Invoke();
        }
    }
}