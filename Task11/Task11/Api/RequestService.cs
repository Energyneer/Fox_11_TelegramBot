using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task11.Api
{
    public class RequestService : IRequestService
    {
        private string AccessToken { get; } = "ab457116572fa95d76686be84420dd7f";
        private HttpClient Client { get; }

        public RequestService()
        {
            Client = new HttpClient();
        }

        public async ValueTask<double> GetRate(string curr1, string curr2)
        {
            HttpResponseMessage message = await Client.GetAsync(@"http://api.exchangeratesapi.io/v1/latest?access_key=" + AccessToken);
            string line = await message.Content.ReadAsStringAsync();
            return ConvertResponse(line, curr1, curr2);
        }

        public async ValueTask<double> GetRate(string curr1, string curr2, DateTime dt)
        {
            HttpResponseMessage message = await Client.GetAsync(@"http://api.exchangeratesapi.io/v1/" + dt.ToString("yyyy-MM-dd") + "?access_key=" + AccessToken);
            string line = await message.Content.ReadAsStringAsync();
            return ConvertResponse(line, curr1, curr2);
        }

        private double ConvertResponse(string textResponse, string curr1, string curr2)
        {
            string baseJsonCurrency = GetBaseCurrency(textResponse);

            if (curr1 == baseJsonCurrency)
                return ParseValue(textResponse, curr2);

            if (curr2 == baseJsonCurrency)
                return 1.0 / ParseValue(textResponse, curr1);

            return ParseValue(textResponse, curr2) / ParseValue(textResponse, curr1);
        }

        private double ParseValue(string text, string curr)
        {
            int index = text.IndexOf(curr + "\":") + curr.Length + 2;
            string textValue = text.Substring(index).Substring(0, text.Substring(index).IndexOf(",")).Replace('.', ',');
            return double.Parse(textValue);
        }

        private string GetBaseCurrency(string text)
        {
            int index = text.IndexOf("base\":\"") + 7;
            return text.Substring(index).Substring(0, text.Substring(index).IndexOf("\""));
        }
    }
}
