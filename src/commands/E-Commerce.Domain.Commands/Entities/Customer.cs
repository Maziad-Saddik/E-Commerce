using E_Commerce.Domain.ValueObjects;

namespace E_Commerce.Domain.Entities
{
    public class Customer
    {
        private Customer(
            Guid id,
            string name,
            Email email
        )
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Email Email { get; private set; }

        public static Customer Add(Guid id, string name, Email email)
            => new Customer(
                id: id,
                name: name,
                email: email
            );

        public void UpdateName(string newName) => Name = newName;

        public void ChangeEmail(Email newEmail) => Email = newEmail;
    }
}
