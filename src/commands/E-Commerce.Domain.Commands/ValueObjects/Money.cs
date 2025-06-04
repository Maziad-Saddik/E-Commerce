using E_Commerce.Domain.Constants;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain.ValueObjects;

[JsonConverter(typeof(MoneyJsonConverter))]

public record struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public static Money Parse(string value)
    {
        var parts = value.Split(' ');
        return parts.Length == 2 ? new Money(decimal.Parse(parts[0]), parts[1]) : throw new FormatException("Transaction Id Format Error");
    }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));

        if (!IsValidCurrency(currency))
            throw new ArgumentException($"Currency '{currency}' is not supported.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    private static bool IsValidCurrency(string currency)
    {
        return Const.SupportedCurrencies.Contains(currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}
