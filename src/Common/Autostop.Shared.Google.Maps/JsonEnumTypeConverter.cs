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
using Google.Maps.Places;
using Google.Maps.Shared;
using Newtonsoft.Json;

namespace Google.Maps
{
    public class JsonEnumTypeConverter : JsonConverter
    {
        public static ServiceResponseStatus AsResponseStatus(string s)
        {
            var result = ServiceResponseStatus.Unknown;

            switch (s)
            {
                case "OK":
                    result = ServiceResponseStatus.Ok;
                    break;
                case "ZERO_RESULTS":
                    result = ServiceResponseStatus.ZeroResults;
                    break;
                case "OVER_QUERY_LIMIT":
                    result = ServiceResponseStatus.OverQueryLimit;
                    break;
                case "REQUEST_DENIED":
                    result = ServiceResponseStatus.RequestDenied;
                    break;
                case "INVALID_REQUEST":
                    result = ServiceResponseStatus.InvalidRequest;
                    break;
                case "MAX_WAYPOINTS_EXCEEDED":
                    result = ServiceResponseStatus.MaxWaypointsExceeded;
                    break;
                case "NOT_FOUND":
                    result = ServiceResponseStatus.NotFound;
                    break;
            }

            return result;
        }

        public static AddressType AsAddressType(string s)
        {
            var result = AddressType.Unknown;

            switch (s)
            {
                case "street_address":
                    result = AddressType.StreetAddress;
                    break;
                case "route":
                    result = AddressType.Route;
                    break;
                case "intersection":
                    result = AddressType.Intersection;
                    break;
                case "political":
                    result = AddressType.Political;
                    break;
                case "country":
                    result = AddressType.Country;
                    break;
                case "administrative_area_level_1":
                    result = AddressType.AdministrativeAreaLevel1;
                    break;
                case "administrative_area_level_2":
                    result = AddressType.AdministrativeAreaLevel2;
                    break;
                case "administrative_area_level_3":
                    result = AddressType.AdministrativeAreaLevel3;
                    break;
                case "colloquial_area":
                    result = AddressType.ColloquialArea;
                    break;
                case "locality":
                    result = AddressType.Locality;
                    break;
                case "sublocality":
                    result = AddressType.Sublocality;
                    break;
                case "neighborhood":
                    result = AddressType.Neighborhood;
                    break;
                case "premise":
                    result = AddressType.Premise;
                    break;
                case "subpremise":
                    result = AddressType.Subpremise;
                    break;
                case "postal_code":
                    result = AddressType.PostalCode;
                    break;
                case "postal_town":
                    result = AddressType.PostalTown;
                    break;
                case "postal_code_prefix":
                    result = AddressType.PostalCodePrefix;
                    break;
                case "natural_feature":
                    result = AddressType.NaturalFeature;
                    break;
                case "airport":
                    result = AddressType.Airport;
                    break;
                case "park":
                    result = AddressType.Park;
                    break;
                case "point_of_interest":
                    result = AddressType.PointOfInterest;
                    break;
                case "post_box":
                    result = AddressType.PostBox;
                    break;
                case "street_number":
                    result = AddressType.StreetNumber;
                    break;
                case "floor":
                    result = AddressType.Floor;
                    break;
                case "room":
                    result = AddressType.Room;
                    break;
            }

            return result;
        }

        private static PlaceType AsPlaceType(string s)
        {
            var result = PlaceType.Unknown;
            switch (s)
            {
                case "accounting":
                    result = PlaceType.Accounting;
                    break;
                case "airport":
                    result = PlaceType.Airport;
                    break;
                case "amusement_park":
                    result = PlaceType.AmusementPark;
                    break;
                case "aquarium":
                    result = PlaceType.Aquarium;
                    break;
                case "art_gallery":
                    result = PlaceType.ArtGallery;
                    break;
                case "atm":
                    result = PlaceType.ATM;
                    break;
                case "bakery":
                    result = PlaceType.Bakery;
                    break;
                case "bank":
                    result = PlaceType.Bank;
                    break;
                case "bar":
                    result = PlaceType.Bar;
                    break;
                case "beauty_salon":
                    result = PlaceType.BeautySalon;
                    break;
                case "bicycle_store":
                    result = PlaceType.BicycleStore;
                    break;
                case "book_store":
                    result = PlaceType.BookStore;
                    break;
                case "bowling_alley":
                    result = PlaceType.BowlingAlley;
                    break;
                case "bus_station":
                    result = PlaceType.BusStation;
                    break;
                case "cafe":
                    result = PlaceType.Cafe;
                    break;
                case "campground":
                    result = PlaceType.Campground;
                    break;
                case "car_dealer":
                    result = PlaceType.CarDealer;
                    break;
                case "car_rental":
                    result = PlaceType.CarRental;
                    break;
                case "car_repair":
                    result = PlaceType.CarRepair;
                    break;
                case "car_wash":
                    result = PlaceType.CarRepair;
                    break;
                case "casino":
                    result = PlaceType.Casino;
                    break;
                case "cemetery":
                    result = PlaceType.Cemetery;
                    break;
                case "church":
                    result = PlaceType.Church;
                    break;
                case "city_hall":
                    result = PlaceType.CityHall;
                    break;
                case "clothing_store":
                    result = PlaceType.ClothingStore;
                    break;
                case "convenience_store":
                    result = PlaceType.ConvenienceStore;
                    break;
                case "courthouse":
                    result = PlaceType.CourtHouse;
                    break;
                case "dentist":
                    result = PlaceType.Dentist;
                    break;
                case "department_store":
                    result = PlaceType.DepartmentStore;
                    break;
                case "doctor":
                    result = PlaceType.Doctor;
                    break;
                case "electrician":
                    result = PlaceType.Electrician;
                    break;
                case "electronics_store":
                    result = PlaceType.ElectronicsStore;
                    break;
                case "embassy":
                    result = PlaceType.Embassy;
                    break;
                case "fire_station":
                    result = PlaceType.FireStation;
                    break;
                case "florist":
                    result = PlaceType.Florist;
                    break;
                case "funeral_home":
                    result = PlaceType.FuneralHome;
                    break;
                case "furniture_store":
                    result = PlaceType.FurnitureStore;
                    break;
                case "gas_station":
                    result = PlaceType.GasStation;
                    break;
                case "gym":
                    result = PlaceType.Gym;
                    break;
                case "hair_care":
                    result = PlaceType.HairCare;
                    break;
                case "hardware_store":
                    result = PlaceType.HardwareStore;
                    break;
                case "hindu_temple":
                    result = PlaceType.HinduTemple;
                    break;
                case "home_goods_store":
                    result = PlaceType.HomeGoodsStore;
                    break;
                case "hospital":
                    result = PlaceType.Hospital;
                    break;
                case "insurance_agency":
                    result = PlaceType.InsuranceAgency;
                    break;
                case "jewelry_store":
                    result = PlaceType.JewelryStore;
                    break;
                case "laundry":
                    result = PlaceType.Laundry;
                    break;
                case "lawyer":
                    result = PlaceType.Lawyer;
                    break;
                case "library":
                    result = PlaceType.Library;
                    break;
                case "liquor_store":
                    result = PlaceType.LiquorStore;
                    break;
                case "local_government_office":
                    result = PlaceType.LocalGovermentOffice;
                    break;
                case "locksmith":
                    result = PlaceType.Locksmith;
                    break;
                case "lodging":
                    result = PlaceType.Lodging;
                    break;
                case "meal_delivery":
                    result = PlaceType.MealDelivery;
                    break;
                case "meal_takeaway":
                    result = PlaceType.MealTakeaway;
                    break;
                case "mosque":
                    result = PlaceType.Mosque;
                    break;
                case "movie_rental":
                    result = PlaceType.MovieRental;
                    break;
                case "movie_theater":
                    result = PlaceType.MovieTheater;
                    break;
                case "moving_company":
                    result = PlaceType.MovingCompany;
                    break;
                case "museum":
                    result = PlaceType.Museum;
                    break;
                case "night_club":
                    result = PlaceType.NightClub;
                    break;
                case "painter":
                    result = PlaceType.Painter;
                    break;
                case "park":
                    result = PlaceType.Park;
                    break;
                case "parking":
                    result = PlaceType.Parking;
                    break;
                case "pet_store":
                    result = PlaceType.PetStore;
                    break;
                case "pharmacy":
                    result = PlaceType.Pharmacy;
                    break;
                case "physiotherapist":
                    result = PlaceType.Physiotherapist;
                    break;
                case "plumber":
                    result = PlaceType.Plumber;
                    break;
                case "police":
                    result = PlaceType.Police;
                    break;
                case "post_office":
                    result = PlaceType.PostOffice;
                    break;
                case "real_estate_agency":
                    result = PlaceType.RealEstateAgency;
                    break;
                case "restaurant":
                    result = PlaceType.Restaurant;
                    break;
                case "roofing_contractor":
                    result = PlaceType.RoofingContractor;
                    break;
                case "rv_park":
                    result = PlaceType.RVPark;
                    break;
                case "school":
                    result = PlaceType.School;
                    break;
                case "shoe_store":
                    result = PlaceType.ShoeStore;
                    break;
                case "shopping_mall":
                    result = PlaceType.ShoppingMall;
                    break;
                case "spa":
                    result = PlaceType.Spa;
                    break;
                case "stadium":
                    result = PlaceType.Stadium;
                    break;
                case "storage":
                    result = PlaceType.Storage;
                    break;
                case "store":
                    result = PlaceType.Store;
                    break;
                case "subway_station":
                    result = PlaceType.SubwayStation;
                    break;
                case "synagogue":
                    result = PlaceType.Synagogue;
                    break;
                case "taxi_stand":
                    result = PlaceType.TaxiStand;
                    break;
                case "train_station":
                    result = PlaceType.TrainStation;
                    break;
                case "travel_agency":
                    result = PlaceType.TravelAgency;
                    break;
                case "university":
                    result = PlaceType.University;
                    break;
                case "veterinary_care":
                    result = PlaceType.VeterinaryCare;
                    break;
                case "zoo":
                    result = PlaceType.Zoo;
                    break;
                case "administrative_area_level_1":
                    result = PlaceType.AdministrativeAreaLevel1;
                    break;
                case "administrative_area_level_2":
                    result = PlaceType.AdministrativeAreaLevel2;
                    break;
                case "administrative_area_level_3":
                    result = PlaceType.AdministrativeAreaLevel3;
                    break;
                case "colloquial_area":
                    result = PlaceType.ColloquialArea;
                    break;
                case "country":
                    result = PlaceType.Country;
                    break;
                case "floor":
                    result = PlaceType.Floor;
                    break;
                case "geocode":
                    result = PlaceType.Geocode;
                    break;
                case "intersection":
                    result = PlaceType.Intersection;
                    break;
                case "locality":
                    result = PlaceType.Locality;
                    break;
                case "natural_feature":
                    result = PlaceType.NaturalFeature;
                    break;
                case "neighborhood":
                    result = PlaceType.Neighborhood;
                    break;
                case "political":
                    result = PlaceType.Political;
                    break;
                case "point_of_interest":
                    result = PlaceType.PointOfInterest;
                    break;
                case "post_box":
                    result = PlaceType.PostBox;
                    break;
                case "postal_code":
                    result = PlaceType.PostalCode;
                    break;
                case "postal_code_prefix":
                    result = PlaceType.PostalCodePrefix;
                    break;
                case "postal_town":
                    result = PlaceType.PostalTown;
                    break;
                case "premise":
                    result = PlaceType.Premise;
                    break;
                case "room":
                    result = PlaceType.Room;
                    break;
                case "route":
                    result = PlaceType.Route;
                    break;
                case "street_address":
                    result = PlaceType.StreetAddress;
                    break;
                case "street_number":
                    result = PlaceType.StreetNumber;
                    break;
                case "sublocality":
                    result = PlaceType.Sublocality;
                    break;
                case "sublocality_level_4":
                    result = PlaceType.SublocalityLevel4;
                    break;
                case "sublocality_level_5":
                    result = PlaceType.SublocalityLevel5;
                    break;
                case "sublocality_level_3":
                    result = PlaceType.SublocalityLevel3;
                    break;
                case "sublocality_level_2":
                    result = PlaceType.SublocalityLevel2;
                    break;
                case "sublocality_level_1":
                    result = PlaceType.SublocalityLevel1;
                    break;
                case "subpremise":
                    result = PlaceType.Subpremise;
                    break;
                case "transit_station":
                    result = PlaceType.TransitStation;
                    break;
            }
            return result;
        }

        public static LocationType AsLocationType(string s)
        {
            var result = LocationType.Unknown;

            switch (s)
            {
                case "ROOFTOP":
                    result = LocationType.Rooftop;
                    break;
                case "RANGE_INTERPOLATED":
                    result = LocationType.RangeInterpolated;
                    break;
                case "GEOMETRIC_CENTER":
                    result = LocationType.GeometricCenter;
                    break;
                case "APPROXIMATE":
                    result = LocationType.Approximate;
                    break;
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return
                objectType == typeof(ServiceResponseStatus)
                || objectType == typeof(AddressType)
                || objectType == typeof(LocationType)
                || objectType == typeof(PlaceType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            object result = null;

            if (objectType == typeof(ServiceResponseStatus))
                result = AsResponseStatus(reader.Value.ToString());

            if (objectType == typeof(AddressType))
                result = AsAddressType(reader.Value.ToString());

            if (objectType == typeof(LocationType))
                result = AsLocationType(reader.Value.ToString());

            if (objectType == typeof(PlaceType))
                result = AsPlaceType(reader.Value.ToString());

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}