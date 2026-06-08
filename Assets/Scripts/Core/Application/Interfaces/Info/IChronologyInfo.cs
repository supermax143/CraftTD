namespace Core.Application.Interfaces.Info
{
    public interface IChronologyInfo
    {
        bool TryGetEpochInfo(int index, out IEpochInfo epoch);
    }
}