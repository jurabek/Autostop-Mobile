namespace Autostop.Common.Shared.Models
{
	public struct Location
	{
		public readonly double Latitude;
		public readonly double Longitude;

		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public bool IsValid()
		{
			if (Latitude > 90 || Latitude < -90)
			{
				return false;
			}
			if (Longitude > 180 || Longitude < -180)
			{
				return false;
			}

			return true;
		}

		public override string ToString()
		{
			return $"Lat: {Latitude} \r\nLong: {Longitude}";
		}
	}
}
