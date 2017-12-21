using System;
using Newtonsoft.Json.Linq;

namespace Google.Maps
{
    public class JsonLocationConverter : JsonCreationConverter<Location>
    {
        protected override Location Create(Type objectType, JObject jsonObject)
        {
            return new LatLng(jsonObject.Value<double>("lat"), jsonObject.Value<double>("lng"));
        }
    }
}