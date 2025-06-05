namespace E_Commerce.Domain.Queries.Entities;

public interface IEntity<TId>
{
    TId Id { get; }
}
