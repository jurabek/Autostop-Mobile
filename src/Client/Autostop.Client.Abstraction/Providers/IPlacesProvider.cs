using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface IPlacesProvider
    {
        Task<ObservableCollection<IAutoCompleteResultModel>> GetAutoCompleteResponse(string input);
    }
}