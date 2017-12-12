namespace Autostop.Common.Shared.Models
{
	public struct Coordinate
	{
		public double Latitude;
		public double Longitude;

		public Coordinate(double latitude, double longitude)
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
	}
}
