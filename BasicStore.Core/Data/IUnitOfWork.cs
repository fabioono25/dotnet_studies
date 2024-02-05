namespace BasicStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
