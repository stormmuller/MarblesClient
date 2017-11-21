using Marbles.Systems.Contracts;
using UnityEngine;
using Zenject;

namespace Marbles.Systems
{
    public class TimeController : ITimeController, ITickable
    {
        private const float normalTimeScale = 1f;

        private float nextResetTime;
        private bool needToReset;

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }

        public void ResetTimeScale()
        {
            Time.timeScale = normalTimeScale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            needToReset = false;
        }

        public void SetTimeScale(float scale, float refreshTime)
        {
            SetTimeScale(scale);
            needToReset = true;
            nextResetTime = Time.time + refreshTime * Time.timeScale;
        }

        public void Tick()
        {
            if (needToReset && Time.time > nextResetTime)
            {
                ResetTimeScale();
            }
        }
    }
}