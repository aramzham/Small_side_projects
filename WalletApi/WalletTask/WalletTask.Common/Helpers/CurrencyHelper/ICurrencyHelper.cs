namespace WalletTask.Common.Helpers.CurrencyHelper
{
    public interface ICurrencyHelper
    {
        decimal Convert(string fromCurrency, string toCurrency, decimal amount);
    }
}