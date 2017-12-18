using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface ISettingsProvider
    {
		Address HomeAddress { get; set; }

		Address WorkAddress { get; set; }
    }
}
