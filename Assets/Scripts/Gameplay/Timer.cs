using System;
using TMPro;
using UnityEngine;

namespace Romix.Gameplay
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        
        private float startTimeInSeconds = 60f;
        private float currentTime;
        private bool isRunning = false;
        
        public event Action OnTimeReachedZero;

        private void Start()
        {
            ResetTimer();
            UpdateTimerDisplay();
        }

        private void Update()
        {
            if (!isRunning) return;
            
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                OnTimeReachedZero?.Invoke();
            }
                
            UpdateTimerDisplay();
        }

        public void StartTimer()
        {
            isRunning = true;
        }

        public void PauseTimer()
        {
            isRunning = false;
        }

        public void SetStartTime(float startTimeInSeconds)
        {
            this.startTimeInSeconds = startTimeInSeconds;
        }
        
        public void ResetTimer()
        {
            currentTime = startTimeInSeconds;
            isRunning = false;
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timerText.text = time.ToString(@"mm\:ss");
        }
    }
}