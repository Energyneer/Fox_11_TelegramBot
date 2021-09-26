using System;
using System.Threading.Tasks;

namespace Task11.Api
{
    public interface IRequestService
    {
        ValueTask<double> GetRate(string firstCurrency, string secondCurrency);
        ValueTask<double> GetRate(string firstCurrency, string secondCurrency, DateTime selectedDate);
    }
}
