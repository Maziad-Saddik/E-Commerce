using E_Commerce.Domain.Constants;

namespace Validation;

public static class ValidationHelpers
{
    public static bool IsValidGuid(string stringGuid)
        => Guid.TryParse(stringGuid, out var guid) && guid != Guid.Empty;

    public static bool ISValidEmail(string email) => Const.EmailRegex.IsMatch(email);

    public static bool ISValidCurrency(string currency) => Const.SupportedCurrencies.Contains(currency);
}