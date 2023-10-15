using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WalletTask.Common.Helpers.CurrencyHelper
{
    public class CurrencyHelper : ICurrencyHelper
    {
        private const string _url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public decimal Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            var result = SendGetRequest(_url).Result;
            var envelope = XmlHelper.FromXml<Envelope>(result);
            var crossRate = GetCrossRate(fromCurrency, toCurrency, envelope.Cube.Cube1);
            return amount / crossRate;
        }

        private static async Task<string> SendGetRequest(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }

        private static decimal GetCrossRate(string fromCurrency, string toCurrency, CubeCube cubeCube)
        {
            var currencyDict = cubeCube.Cube.ToDictionary(k => k.currency, v => v.rate);
            var toCubeRate = toCurrency == CurrencyCodes.EUR ? 1 : currencyDict[toCurrency];
            var fromCubeRate = fromCurrency == CurrencyCodes.EUR ? 1 : currencyDict[fromCurrency];

            return fromCubeRate / toCubeRate;
        }
    }
}