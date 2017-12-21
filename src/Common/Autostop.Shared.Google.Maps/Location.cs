using System;

namespace Google.Maps
{
    /// <summary>
    ///     A general free-text location, usually for specifying an address or particular place for Google Maps.
    /// </summary>
    public class Location
    {
        private readonly string _value;

        protected Location()
        {
        }

        /// <summary>
        ///     Creates a location instance for an address specified as text.
        /// </summary>
        /// <param name="value"></param>
        public Location(string value)
        {
            if (value != null) _value = value.Trim();
        }

        /// <summary>
        ///     Returns the string representation of the current instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }

        /// <summary>
        ///     Gets the current instance as a URL encoded value.
        /// </summary>
        /// <returns></returns>
        public virtual string GetAsUrlParameter()
        {
            return Uri.EscapeDataString(ToString());
        }

        /// <summary>
        ///     implicitly converts a System.String to Google.Maps.Location
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Location(string value)
        {
            return new Location(value);
        }
    }
}