using System;
using System.Security.Cryptography;
using System.Text;

namespace Google.Maps
{
    /// <summary>
    ///     Stores a Google Business API customer's signing information to be passed with service requests to Google's APIs.
    /// </summary>
    /// <remarks>
    ///     Use GoogleSigned.AssignAllServices() method to attach the signing information to all outgoing requests, usually
    ///     during App Startup.
    /// </remarks>
    public class GoogleSigned
    {
        /// <summary>
        ///     Used by all the services except Geolocation API and Places API
        /// </summary>
        private static GoogleSigned S_universalSigningInstance;

        private readonly string _apiKey;
        private readonly byte[] _privateKeyBytes;
        private readonly GoogleSignedType _signType = GoogleSignedType.ApiKey;

        public GoogleSigned(string apiKey)
        {
            _apiKey = apiKey;
            _signType = GoogleSignedType.ApiKey;
        }

        public GoogleSigned(string clientId, string usablePrivateKey)
        {
            usablePrivateKey = usablePrivateKey.Replace("-", "+").Replace("_", "/");
            _privateKeyBytes = Convert.FromBase64String(usablePrivateKey);
            ClientId = clientId;
            _signType = GoogleSignedType.Business;
        }

        /// <summary>
        ///     Gets or sets the GoogleSigned instance to use for all of the various service calls.
        /// </summary>
        public static GoogleSigned SigningInstance => S_universalSigningInstance;

        public string ClientId { get; }

        /// <summary>
        ///     Assigns the given SigningInstance to all services that can utilize it.  Note that some of the services do not
        ///     currently use the signature method.
        /// </summary>
        /// <param name="signingInstance"></param>
        public static void AssignAllServices(GoogleSigned signingInstance)
        {
            S_universalSigningInstance = signingInstance;
        }

        public string GetSignedUri(Uri uri)
        {
            var builder = new UriBuilder(uri);
            if (_signType == GoogleSignedType.Business)
            {
                builder.Query = builder.Query.Substring(1) + "&client=" + ClientId;
                uri = builder.Uri;

                var signature = GetSignature(uri);
                signature = signature.Replace("+", "-").Replace("/", "_");

                return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
            }
            builder.Query = builder.Query.Substring(1) + "&key=" + _apiKey;
            uri = builder.Uri;
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query;
        }

        public string GetSignedUri(string url)
        {
            return GetSignedUri(new Uri(url));
        }

        public string GetSignature(Uri uri)
        {
            var encodedPathQuery = Encoding.ASCII.GetBytes(uri.LocalPath + uri.Query);

            var hashAlgorithm = new HMACSHA1(_privateKeyBytes);
            var hashed = hashAlgorithm.ComputeHash(encodedPathQuery);

            var signature = Convert.ToBase64String(hashed);
            return signature;
        }

        public string GetSignature(string url)
        {
            return GetSignature(new Uri(url));
        }
    }
}