using System.Collections.Generic;

namespace Unity.Infrastructure.Requirements.Base
{
    /// <summary>
    /// Отвечает за проверку конкретного рекваермента/группы рекваерментов
    /// </summary>
    public interface IRequirementChecker
    {
        bool Check<T>(T req) where T : class, IRequirement;
    }
}