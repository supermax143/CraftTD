using System;

namespace Unity.Utils.Time
{
    public static class TimeHelper
    {
        public static float GetTime(TimeType timeType)
        {
            switch (timeType)
            {
                case TimeType.Scaled: return UnityEngine.Time.time;
                case TimeType.Unscaled: return UnityEngine.Time.unscaledTime;
                case TimeType.Fixed: return UnityEngine.Time.fixedTime;
                default: throw new ArgumentException("timeType");
            }
        }
    }
}