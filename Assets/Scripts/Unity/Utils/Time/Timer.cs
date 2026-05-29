using UnityEngine;

namespace Unity.Utils.Time
{
    public class Timer
    {
        public delegate void TimerDelegate();

        public Timer(TimeType timeType)
        {
            TimeType = timeType;
        }

        public Timer()
        {
        }

        public TimeType TimeType { get; } = TimeType.Scaled;

        /// <summary>Длительность таймера в секундах</summary>
        public float Duration { get; private set; }

        public float StartTime { get; private set; }
        public float TimeElapsed => TimeHelper.GetTime(TimeType) - StartTime;
        public float TimeLeft => Duration - TimeElapsed;
        public float Progress => Duration == 0 ? 1 : Mathf.Min(1, TimeElapsed / Duration);
        public bool IsComplete => TimeLeft <= 0;
        public bool Started { get; private set; }

        public event TimerDelegate Complete;

        public void Start(float duration)
        {
            Duration = duration;
            StartTime = TimeHelper.GetTime(TimeType);
            Started = true;
        }

        public void Stop()
        {
            Started = false;
        }

        public void Update()
        {
            if (!Started)
                return;
            if (TimeLeft > 0)
                return;
            Started = false;
            if (Complete != null)
                Complete();
        }
    }
}