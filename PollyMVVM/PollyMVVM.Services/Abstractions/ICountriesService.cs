using System.Threading.Tasks;
using PollyMVVM.Models;

namespace PollyMVVM.Services.Abstractions
{
    public interface ICountriesService
    {
        Task<Country[]> GetCountries();
        Task<Country[]> GetCountriesWithRetry();
        Task<Country[]> GetCountriesWithWaitAndRetry();
    }
}
