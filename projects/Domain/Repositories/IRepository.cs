namespace Domain.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IUnitOfWork : IDisposable
    {
    }
}