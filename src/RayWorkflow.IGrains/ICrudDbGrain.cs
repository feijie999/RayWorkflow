using System.Threading.Tasks;
using Ray.Core.Event;

namespace RayWorkflow.IGrains
{
    public interface ICrudDbGrain<TPrimaryKey>
    {
        Task Process(FullyEvent<TPrimaryKey> @event);
    }
}