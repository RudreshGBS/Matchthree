using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rudscreation.Utils
{
    public enum TimerColorZone
    {
        Green,
        Yellow,
        Red,
        Grey
    }

    public class TimerHandler :Singleton<TimerHandler>
    {
        private bool isPaused = false;
        private bool isGreenInvoked = false;
        private bool isYellowInvoked = false;
        private bool isRedInvoked = false;
        private bool isGreyInvoked = false;

        private int TotaltimeinSeconds;
        public TimeSpan totaltimeinTimespan;
        public float valueInzerotoOne;
        public Action<TimerColorZone> UpdateZone; 
        public Action TimerReachedToEnd;

        /// <summary>
        /// pause timer from External class
        /// </summary>
        public void PauseTimer()
        {
            isPaused = true;
        }
        /// <summary>
        /// Resume Timer form External class
        /// </summary>
        public void ResumeTimer()
        {
            isPaused = false;
        }
        /// <summary>
        /// Start timer from external class
        /// </summary>
        /// <param name="timeInSeconds"> provde time in se</param>
        public void StartTimer(TimeSpan timeInSeconds)
        {
            TotaltimeinSeconds = (int) timeInSeconds.TotalSeconds;
            StartCoroutine("CountdownTimer");
        }
        /// <summary>
        /// Stop timer at end of 
        /// </summary>
        public void StopTimer() 
        {
            StopCoroutine("CountdownTimer");
        }
        
        /// <summary>
        /// Timer Implemetation 
        /// </summary>
        /// <returns></returns>
        private IEnumerator CountdownTimer()
        {
            float totalTime = TotaltimeinSeconds;
            float unitValue = 1 / totalTime;
            Debug.Log($"unitvalue : {unitValue}");

            while (TotaltimeinSeconds > 0)
            {
                while (isPaused)
                {
                    Debug.Log("Paused");
                    if (!isGreyInvoked)
                    {
                        isGreyInvoked = true;
                        isGreenInvoked = false;
                        isYellowInvoked = false;
                        isRedInvoked = false;
                        UpdateZone?.Invoke(TimerColorZone.Grey);
                    }
                    yield return null;
                }

                yield return new WaitForSecondsRealtime(1);
                isGreyInvoked = false;
                if (TotaltimeinSeconds > (totalTime * 0.5f))
                {
                    Debug.Log("100 to 50");
                    if (!isGreenInvoked)
                    {
                        UpdateZone?.Invoke(TimerColorZone.Green);
                        isGreenInvoked = true;
                    }
                }
                else if (TotaltimeinSeconds <= (totalTime * 0.5f))
                {
                    if (TotaltimeinSeconds >= (totalTime * 0.1f))
                    {
                        Debug.Log("50 to 10");
                        if (!isYellowInvoked)
                        {
                            UpdateZone?.Invoke(TimerColorZone.Yellow);
                            isYellowInvoked = true;
                        }
                    }
                    else
                    {
                        Debug.Log("Below 10");
                        if (!isRedInvoked)
                        {
                            UpdateZone?.Invoke(TimerColorZone.Red);
                            isRedInvoked = true;
                        }
                    }
                }
                TotaltimeinSeconds--;
                valueInzerotoOne = TotaltimeinSeconds * unitValue;
                totaltimeinTimespan = TimeSpan.FromSeconds (TotaltimeinSeconds);
                Debug.Log($"time left : {TotaltimeinSeconds}");
                Debug.Log($"time left 0 to 1 : {valueInzerotoOne}");
                Debug.Log($"time in timespan : {totaltimeinTimespan}");

            }
            if (TotaltimeinSeconds == 0)
            {
                Debug.Log("timer finished");
                TimerReachedToEnd?.Invoke();
            }
        }
    }
}