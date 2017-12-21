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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Google.Maps.Internal;

namespace Google.Maps.StaticMaps
{
    /// <summary>
    ///     The Google Static Maps API returns an image (either GIF, PNG or JPEG)
    ///     in response to a HTTP request via a URL. For each request, you can
    ///     specify the location of the map, the size of the image, the zoom level,
    ///     the type of map, and the placement of optional markers at locations on
    ///     the map.
    /// </summary>
    public class StaticMapRequest : BaseRequest
    {
        public static readonly Uri BaseUri =
            new Uri("https://maps.google.com/maps/api/");

        private int? _scale;
        private MapSize _size;
        private int? _zoom;

        public StaticMapRequest()
        {
            Size = new MapSize(512, 512); //default size is 512x512
            Visible = new List<Location>(1);
            Markers = new MapMarkersCollection();
            Paths = new List<Path>();
        }

        /// <summary>
        ///     Defines the center of the map, equidistant from all edges of the
        ///     map. This parameter takes an <see cref="Location" />-derived instance identifying a
        ///     unique location on the face of the earth. Use <see cref="LatLng" /> for a
        ///     {latitude,longitude} pair (e.g. 40.714728,-73.998672) or use <see cref="Location" /> for a
        ///     string address (e.g. "city hall, new york, ny"). (Required if markers not present.)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#Locations</remarks>
        public Location Center { get; set; }

        /// <summary>
        ///     Defines the zoom level of the map, which determines the
        ///     magnification level of the map. This parameter takes a numerical
        ///     value corresponding to the zoom level of the region desired. (Required if markers not present)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#Zoomlevels</remarks>
        public int? Zoom
        {
            get => _zoom;
            set
            {
                if (value != null)
                    if (value < Constants.ZOOM_LEVEL_MIN)
                        throw new ArgumentOutOfRangeException(string.Format("value cannot be less than 0.",
                            Constants.ZOOM_LEVEL_MIN));
                _zoom = value;
            }
        }


        /// <summary>
        ///     Defines the rectangular dimensions of the map image. This parameter
        ///     takes a string of the form valuexvalue where horizontal pixels are
        ///     denoted first while vertical pixels are denoted second. For example,
        ///     500x400 defines a map 500 pixels wide by 400 pixels high. If you
        ///     create a static map that is 100 pixels wide or smaller, the
        ///     "Powered by Google" logo is automatically reduced in size. (required)
        /// </summary>
        public MapSize Size
        {
            get => _size;
            set
            {
                if (value.Width < Constants.SIZE_WIDTH_MIN)
                    throw new ArgumentOutOfRangeException(string.Format("value.Width cannot be less than {0}.",
                        Constants.SIZE_WIDTH_MIN));
                if (value.Height < Constants.SIZE_HEIGHT_MIN)
                    throw new ArgumentOutOfRangeException(string.Format("value.Height cannot be less than {0}.",
                        Constants.SIZE_HEIGHT_MIN));
                if (value.Width > Constants.SIZE_WIDTH_MAX)
                    throw new ArgumentOutOfRangeException(string.Format("value.Width cannot be greater than {0}.",
                        Constants.SIZE_WIDTH_MAX));
                if (value.Height > Constants.SIZE_HEIGHT_MAX)
                    throw new ArgumentOutOfRangeException(string.Format("value.Height cannot be greater than {0}.",
                        Constants.SIZE_HEIGHT_MAX));
                _size = value;
            }
        }

        /// <summary>
        ///     affects the number of pixels that are returned. scale=2 returns twice as many pixels as scale=1
        ///     while retaining the same coverage area and level of detail (i.e. the contents of the map don't change).
        ///     This is useful when developing for high-resolution displays, or when generating a map for printing.
        ///     The default value is 1. Accepted values are 2 and 4 (4 is only available to Maps API for Business customers.)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#scale_values</remarks>
        public int? Scale
        {
            get => _scale;
            set
            {
                if (value != null)
                    Constants.IsExpectedScaleValue(value.Value, true);

                _scale = value;
            }
        }

        /// <summary>
        ///     Defines the format of the resulting image. By default, the Static
        ///     Maps API creates PNG images. There are several possible formats
        ///     including GIF, JPEG and PNG types. Which format you use depends on
        ///     how you intend to present the image. JPEG typically provides
        ///     greater compression, while GIF and PNG provide greater detail. (optional)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#ImageFormats</remarks>
        public GMapsImageFormats Format { get; set; }

        /// <summary>
        ///     Defines the type of map to construct. There are several possible
        ///     maptype values, including roadmap, satellite, hybrid, and terrain. (optional)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#MapTypes</remarks>
        public MapTypes MapType { get; set; }

        /// <summary>
        ///     Defines the language to use for display of labels on map tiles. Note
        ///     that this parameter is only supported for some country tiles; if the
        ///     specific language requested is not supported for the tile set, then
        ///     the default language for that tileset will be used. (optional)
        /// </summary>
        /// <remarks></remarks>
        public string Language { get; set; }

        /// <summary>
        ///     defines the appropriate borders to display, based on geo-political sensitivities.
        ///     Accepts a region code specified as a two-character ccTLD ('top-level domain') value. (optional)
        /// </summary>
        public string Region { get; set; }


        /// <summary>
        ///     Define one or more markers to attach to the image at specified
        ///     locations. This parameter takes a single marker definition with
        ///     parameters separated by the pipe character (|). Multiple markers
        ///     may be placed within the same markers parameter as long as they
        ///     exhibit the same style; you may add additional markers of
        ///     differing styles by adding additional markers parameters. Note that
        ///     if you supply markers for a map, you do not need to specify the
        ///     (normally required) center and zoom parameters. (optional)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#Markers</remarks>
        public MapMarkersCollection Markers { get; set; }

        /// <summary>
        ///     For backwards-compatibility; shortcut for Paths when not using
        ///     multiple paths.
        /// </summary>
        public Path Path
        {
            get => Paths.SingleOrDefault();
            set => Paths = new List<Path> {value};
        }

        /// <summary>
        ///     Defines a single path of two or more connected points to overlay on
        ///     the image at specified locations. This parameter takes a string of
        ///     point definitions separated by the pipe character (|). You may
        ///     supply additional paths by adding additional path parameters. Note
        ///     that if you supply a path for a map, you do not need to specify the
        ///     (normally required) center and zoom parameters. (optional)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#Paths</remarks>
        public ICollection<Path> Paths { get; set; }

        /// <summary>
        ///     Specifies one or more locations that should remain visible on the
        ///     map, though no markers or other indicators will be displayed. Use
        ///     this parameter to ensure that certain features or map locations
        ///     are shown on the static map. (optional)
        /// </summary>
        /// <remarks>http://code.google.com/apis/maps/documentation/staticmaps/#Paths</remarks>
        public ICollection<Location> Visible { get; set; }

        public override Uri ToUri()
        {
            string formatStr = null;
            switch (Format)
            {
                case GMapsImageFormats.Unspecified: break;
                case GMapsImageFormats.JPGbaseline:
                    formatStr = "jpg-baseline";
                    break;
                default:
                    formatStr = Format.ToString().ToLower();
                    break;
            }

            string maptypeStr = null;
            switch (MapType)
            {
                case MapTypes.Unspecified: break;
                default:
                    maptypeStr = MapType.ToString().ToLower();
                    break;
            }

            string zoomStr = null;
            if (Zoom != null)
                zoomStr = Zoom.ToString();

            string centerStr = null;
            if (Center != null)
                centerStr = Center.GetAsUrlParameter();

            var qs = new QueryStringBuilder()
                .Append("center", centerStr)
                .Append("zoom", zoomStr)
                .Append("size", string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Size.Width, Size.Height))
                .Append("scale", Scale == null ? null : Scale.Value.ToString())
                .Append("format", formatStr)
                .Append("maptype", maptypeStr)
                .Append("language", Language)
                .Append("region", Region)
                .Append(GetMarkersStr())
                .Append(GetPathsStr())
                .Append("visible", GetVisibleStr());

            var url = "staticmap?" + qs;

            return new Uri(BaseUri, new Uri(url, UriKind.Relative));
        }

        /// <summary>
        ///     Builds path uri parameter
        /// </summary>
        /// <returns></returns>
        private string GetPathsStr()
        {
            if (Paths == null || Paths.Count == 0) return null;

            var pathParam = new string[Paths.Count];
            var pathParamIndex = 0;
            var sb = new StringBuilder();

            foreach (var currentPath in Paths)
            {
                sb.Length = 0;

                if (!currentPath.Color.IsUndefined)
                    sb.Append("color:").Append(currentPath.Color.To32BitColorString());

                if (!currentPath.FillColor.IsUndefined)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.Append("fillcolor:").Append(currentPath.FillColor.To32BitColorString());
                }

                if (currentPath.Encode.GetValueOrDefault())
                {
                    var encodedValue = GetPathEncoded(currentPath);

                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.Append(Constants.PATH_ENCODED_PREFIX).Append(encodedValue);
                }
                else
                {
                    if (currentPath.Encode == null && currentPath.Points.Count > 10)
                    {
                        IEnumerable<LatLng> positionCollection;
                        if (ConvertUtil.TryCast(currentPath.Points, out positionCollection))
                        {
                            var encodedValue = PolylineEncoder.EncodeCoordinates(positionCollection);
                            if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                            sb.Append(Constants.PATH_ENCODED_PREFIX).Append(encodedValue);
                        }
                    }

                    foreach (var loc in currentPath.Points)
                    {
                        if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                        sb.Append(loc.GetAsUrlParameter());
                    }
                }

                pathParam[pathParamIndex++] = "path=" + sb;
            }

            return string.Join("&", pathParam);
        }

        private static string GetPathEncoded(Path currentPath)
        {
            IEnumerable<LatLng> latlngPoints;
            try
            {
                latlngPoints = currentPath.Points.Cast<LatLng>().ToList();
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidOperationException(
                    "Encountered a point specified as a location.  Encoding only supports all points in LatLng types.",
                    ex);
            }

            var encodedValue = PolylineEncoder.EncodeCoordinates(latlngPoints);
            return encodedValue;
        }

        /// <summary>
        ///     Builds visible uri parameter
        /// </summary>
        /// <returns></returns>
        private string GetVisibleStr()
        {
            if (Visible.Count == 0) return null;

            var sb = new StringBuilder();
            foreach (var loc in Visible)
            {
                if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                sb.Append(loc);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Builds markers uri parameter(s)
        /// </summary>
        /// <returns></returns>
        private string GetMarkersStr()
        {
            if (Markers.Count == 0) return null;

            var sb = new StringBuilder(200);

            var markerStrings = new string[Markers.Count];
            var markerStringsIndex = 0;

            foreach (var current in Markers)
            {
                //start with an empty stringbuilder.
                sb.Remove(0, sb.Length);

                //output the size parameter, if it was specified.
                if (current.MarkerSize != MarkerSizes.Unspecified)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.AppendFormat("size:{0}", current.MarkerSize.ToString().ToLowerInvariant());
                }

                //check for a color specified for the markers and add that style attribute if so
                if (!current.Color.IsUndefined)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);

                    sb.AppendFormat(current.Color.To24BitColorString());
                }

                // add a label, but if the MarkerSize is MarkerSizes.Tiny or Small then you can't have a label.
                if (string.IsNullOrEmpty(current.Label) == false &&
                    !(current.MarkerSize == MarkerSizes.Tiny || current.MarkerSize == MarkerSizes.Small))
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.AppendFormat("label:{0}", current.Label);
                }

                //add a custom icon param
                if (string.IsNullOrEmpty(current.IconUrl) == false)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.AppendFormat("icon:{0}", Uri.EscapeDataString(current.IconUrl));

                    if (current.Shadow != null)
                    {
                        if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                        sb.AppendFormat("shadow:{0}", current.Shadow == true ? "true" : "false");
                    }
                }

                //add a custom scale param
                if (current.Scale != null)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.AppendFormat("scale:{0}", current.Scale.Value);
                }

                //iterate the locations
                foreach (var loc in current.Locations)
                {
                    if (sb.Length > 0) sb.Append(Constants.PIPE_URL_ENCODED);
                    sb.Append(loc.GetAsUrlParameter());
                }

                markerStrings[markerStringsIndex++] = "markers=" + sb;
            }

            return string.Join("&", markerStrings);
        }
    }
}