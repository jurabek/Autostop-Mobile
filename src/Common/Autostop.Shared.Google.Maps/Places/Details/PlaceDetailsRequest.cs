using System;
using Google.Maps.Internal;

namespace Google.Maps.Places.Details
{
    public class PlaceDetailsRequest : BaseRequest
    {
        /// <summary>
        ///     Undocumented address component filters.
        ///     Only geocoding results matching the component filters will be returned.
        /// </summary>
        /// <remarks>IE: country:uk|locality:stathern</remarks>
        public string PlaceID { get; set; }

        /// <summary>
        ///     The bounding box of the viewport within which to bias geocode
        ///     results more prominently.
        /// </summary>
        /// <remarks>
        ///     Optional. This parameter will only influence, not fully restrict, results
        ///     from the geocoder.
        /// </remarks>
        /// <see href="http://code.google.com/apis/maps/documentation/geocoding/#Viewports" />
        [Obsolete("Use PlaceID")]
        public string Reference { get; set; }

        /// <summary>
        ///     The region code, specified as a ccTLD ("top-level domain")
        ///     two-character value.
        /// </summary>
        /// <remarks>
        ///     Optional. This parameter will only influence, not fully restrict, results
        ///     from the geocoder.
        /// </remarks>
        /// <see href="http://code.google.com/apis/maps/documentation/geocoding/#RegionCodes" />
        public string Extensions { get; set; }

        /// <summary>
        ///     The language in which to return results. If language is not
        ///     supplied, the geocoder will attempt to use the native language of
        ///     the domain from which the request is sent wherever possible.
        /// </summary>
        /// <remarks>Optional.</remarks>
        /// <see href="http://code.google.com/apis/maps/faq.html#languagesupport" />
        public string Language { get; set; }

        public override Uri ToUri()
        {
            var qsb = new QueryStringBuilder();

            if (!string.IsNullOrEmpty(PlaceID))
                qsb.Append("placeid", PlaceID);
#pragma warning disable CS0618 // Type or member is obsolete
            else if (!string.IsNullOrEmpty(Reference))
                qsb.Append("reference", Reference);
#pragma warning restore CS0618 // Type or member is obsolete
            else
                throw new InvalidOperationException("Either PlaceID or Reference fields must be set.");

            if (!string.IsNullOrEmpty(Extensions))
                qsb.Append("extensions", Extensions);

            if (!string.IsNullOrEmpty(Language))
                qsb.Append("language", Language);

            var url = "json?" + qsb;

            return new Uri(url, UriKind.Relative);
        }
    }
}