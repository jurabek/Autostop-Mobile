/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace Google.Maps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ViaLatLng : Location, IEquatable<ViaLatLng>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ViaLatLng" /> class.
        /// </summary>
        public ViaLatLng()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViaLatLng" /> class with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude">Latitude coordinates.</param>
        /// <param name="longitude">Longitude coordinates.</param>
        public ViaLatLng(decimal latitude, decimal longitude)
        {
            Latitude = Convert.ToDouble(latitude);
            Longitude = Convert.ToDouble(longitude);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViaLatLng" /> class with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude">Latitude coordinates.</param>
        /// <param name="longitude">Longitude coordinates.</param>
        public ViaLatLng(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViaLatLng" /> class with the given latitude and longitude coordinates.
        /// </summary>
        /// <param name="latitude">Latitude coordinates.</param>
        /// <param name="longitude">Longitude coordinates.</param>
        public ViaLatLng(float latitude, float longitude)
        {
            Latitude = Convert.ToDouble(latitude);
            Longitude = Convert.ToDouble(longitude);
        }

        /// <summary>
        ///     Gets the latitude coordinate.
        /// </summary>
        [JsonProperty("lat")]
        public double Latitude { get; }

        /// <summary>
        ///     Gets the longitude coordinate.
        /// </summary>
        [JsonProperty("lng")]
        public double Longitude { get; }

        public bool Equals(ViaLatLng other)
        {
            if (other == null)
                return false;

            if (other.Latitude == Latitude && other.Longitude == Longitude)
                return true;

            return false;
        }

        /// <summary>
        ///     Gets the string representation of the latitude and longitude coordinates. Default format is "N6" for 6 decimal
        ///     precision.
        /// </summary>
        /// <returns>Latitude and longitude coordinates.</returns>
        public override string ToString()
        {
            return ToString("N6");
        }

        /// <summary>
        ///     Gets the string representation of the latitude and longitude coordinates. The format is applies to a System.Double,
        ///     so any format applicable for System.Double will work.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            var sb = new StringBuilder(50); //default to 50 in the internal array.
            sb.Append("via:");
            sb.Append(Latitude.ToString(format, CultureInfo.InvariantCulture));
            sb.Append(",");
            sb.Append(Longitude.ToString(format, CultureInfo.InvariantCulture));

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the current instance as a URL encoded value.
        /// </summary>
        /// <returns></returns>
        public override string GetAsUrlParameter()
        {
            //we're not returning crazy characters so just return the string.
            //prevents the comma from being converted to %2c, expanding the single character to three characters.
            return ToString("R");
        }

        /// <summary>
        ///     Parses a ViaLatLng from a set of latitude/longitude coordinates
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ViaLatLng Parse(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            try
            {
                var parts = value.Split(',');

                if (parts.Length != 2) throw new FormatException("Missing data for points.");

                var latitude = double.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
                var longitude = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);

                var vialatlng = new ViaLatLng(latitude, longitude);

                return vialatlng;
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to parse ViaLatLng.", ex);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ViaLatLng);
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash += hash * 7 + Latitude.GetHashCode();
            hash += hash * 7 + Longitude.GetHashCode();
            return hash;
        }
    }
}