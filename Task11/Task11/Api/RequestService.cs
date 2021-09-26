using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task11.Api
{
    public class RequestService : IRequestService
    {
        private string AccessToken { get; } = Constants.API_TOKEN;
        private HttpClient Client { get; }

        public RequestService()
        {
            Client = new HttpClient();
        }

        public async ValueTask<double> GetRate(string firstCurrency, string secondCurrency)
        {
            HttpResponseMessage message = await Client.GetAsync(Constants.API_HTTP_PATH + Constants.API_LAST_VALUE + 
                Constants.API_HTTP_SUFX + AccessToken);
            string line = await message.Content.ReadAsStringAsync();
            return ConvertResponse(line, firstCurrency, secondCurrency);
        }

        public async ValueTask<double> GetRate(string firstCurrency, string secondCurrency, DateTime selectedDate)
        {
            HttpResponseMessage message = await Client.GetAsync(Constants.API_HTTP_PATH + 
                selectedDate.ToString(Constants.API_DATE_FORMAT) + 
                Constants.API_HTTP_SUFX + AccessToken);
            string line = await message.Content.ReadAsStringAsync();
            return ConvertResponse(line, firstCurrency, secondCurrency);
        }

        private double ConvertResponse(string textResponse, string firstCurrency, string secondCurrency)
        {
            string baseJsonCurrency = GetBaseCurrency(textResponse);

            if (firstCurrency == baseJsonCurrency)
                return ParseValue(textResponse, secondCurrency);

            if (firstCurrency == baseJsonCurrency)
                return 1.0 / ParseValue(textResponse, firstCurrency);

            return ParseValue(textResponse, secondCurrency) / ParseValue(textResponse, firstCurrency);
        }

        private double ParseValue(string textResponse, string currency)
        {
            int index = textResponse.IndexOf(currency + "\":") + currency.Length + 2;
            string textValue = textResponse.Substring(index)
                .Substring(0, textResponse.Substring(index).IndexOf(",")).Replace('.', ',');
            return double.Parse(textValue);
        }

        private string GetBaseCurrency(string textResponse)
        {
            int index = textResponse.IndexOf("base\":\"") + 7;
            return textResponse.Substring(index).Substring(0, textResponse.Substring(index).IndexOf("\""));
        }
    }
}
