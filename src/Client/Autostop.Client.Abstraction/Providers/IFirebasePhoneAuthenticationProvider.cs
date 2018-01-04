using System.Threading.Tasks;
using Autostop.Common.Shared.Models;

namespace Autostop.Client.Abstraction.Providers
{
    public interface IPhoneAuthenticationProvider
    {
        Task<AuthorizedUser> SignIn(string verificationId, string verificationCode);
        Task<VerifyNumberResult> VerifyPhoneNumber(string phoneNumber);
    }
}