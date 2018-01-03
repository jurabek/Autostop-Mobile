using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Common.Shared.Models
{
    public class AuthorizedUser
    {
	    public string Uid { get; set; }
	    public string RefreshToken { get; set; }
	    public string PhoneNumber { get; set; }
	    public string Token { get; set; }
    }
}
