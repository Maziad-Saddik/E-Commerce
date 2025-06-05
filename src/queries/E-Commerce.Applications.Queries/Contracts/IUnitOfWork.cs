namespace E_Commerce.Applications.Queries.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task CompleteAsync(CancellationToken cancellationToken);
}
