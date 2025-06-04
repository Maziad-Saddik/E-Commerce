namespace E_Commerce.Domain.Interfaces
{
    public interface ICommand
    {
        public string OrderId { get; }

        public string UserId { get; }
    }
}
