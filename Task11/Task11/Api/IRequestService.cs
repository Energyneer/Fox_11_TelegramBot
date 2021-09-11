using System;
using System.Threading.Tasks;

namespace Task11.Api
{
    public interface IRequestService
    {
        ValueTask<double> GetRate(string curr1, string curr2);
        ValueTask<double> GetRate(string curr1, string curr2, DateTime dt);
    }
}
