using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface ISettingsProvider
    {
        Address GetHomeAddress();
        void SetHomeAddress(Address value);
        Address GetWorkAddress();
        void SetWorkAddress(Address value);
    }
}