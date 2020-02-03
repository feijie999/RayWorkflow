namespace RayWorkflow.Domain.Shared
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; }

        void SetId(TPrimaryKey id);
    }
}