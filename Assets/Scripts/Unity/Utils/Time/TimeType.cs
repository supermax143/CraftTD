namespace Unity.Utils.Time
{
    public enum TimeType
    {
	    /// <summary>Масштабированное время из <see cref="UnityEngine.Time.time" />.</summary>
	    Scaled,

	    /// <summary>Немасштабированное время из <see cref="UnityEngine.Time.unscaledTime" />.</summary>
	    Unscaled,

	    /// <summary>Время с фиксированным шагом из <see cref="UnityEngine.Time.fixedTime" />.</summary>
	    Fixed
    }
}