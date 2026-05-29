using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IBootstrapStep
    {
        Task Initialize();
    }
}