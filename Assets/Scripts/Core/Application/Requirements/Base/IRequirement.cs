namespace Core.Application.Requirements.Base
{
    /// <summary>
    /// Все врапперы аспектов-рекваерментов реализуют этот интерфейс
    /// </summary>
    public interface IRequirement
    {
        bool Check(IRequirementChecker checker);

        float GetProgress(IRequirementChecker checker);
    }
}