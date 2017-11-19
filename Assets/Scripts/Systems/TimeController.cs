using Marbles.Systems.Contracts;
using UnityEngine;

namespace Marbles.Systems
{
    public class TimeController : ITimeController
    {
        private const float normalTimeScale = 1f;

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
        }

        public void ResetTimeScale()
        {
            Time.timeScale = normalTimeScale;
        }
    }
}