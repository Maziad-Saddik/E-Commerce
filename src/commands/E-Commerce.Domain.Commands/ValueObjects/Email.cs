using E_Commerce.Domain.Constants;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain.ValueObjects
{
    [JsonConverter(typeof(EmailJsonConverter))]
    public readonly record struct Email
    {
        public string Value { get; }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email address cannot be null or empty.", nameof(value));


            if (!Const.EmailRegex.IsMatch(value))
                throw new ArgumentException($"'{value}' is not a valid email address format.", nameof(value));

            Value = value.ToLowerInvariant();
        }

        public override string ToString() => Value;
    }
}
