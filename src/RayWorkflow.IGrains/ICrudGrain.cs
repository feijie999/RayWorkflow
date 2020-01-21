using System.Threading.Tasks;

namespace RayWorkflow.IGrains
{
    public interface ICrudGrain<TSnapshotDto>
        where TSnapshotDto : class, new()
    {
        Task Create(TSnapshotDto snapshot);

        Task<TSnapshotDto> Get();

        Task Update(TSnapshotDto snapshot);

        Task Delete();

        Task Over();
    }
}
