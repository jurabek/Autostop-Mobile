using System;
using System.Collections.Generic;
using System.Text;

namespace Autostop.Common.Shared.Models
{
    public class VerificationCountryCode
    {
        public string CountryCodeFormatted { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string Title => $"{CountryName} {CountryCodeFormatted}";
    }
}
