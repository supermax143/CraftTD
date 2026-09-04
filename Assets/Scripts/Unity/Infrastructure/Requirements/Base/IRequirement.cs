namespace Unity.Infrastructure.Requirements.Base
{
    /// <summary>
    /// Все врапперы аспектов-рекваерментов реализуют этот интерфейс
    /// </summary>
    public interface IRequirement
    {
        bool Check(IRequirementVisitor visitor);
    }
}