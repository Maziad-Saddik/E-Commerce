namespace E_Commerce.Infrastructure.Queries.Persistence.Repositories
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {

        public Task CompleteAsync(CancellationToken cancellationToken)
            => dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            dbContext.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
