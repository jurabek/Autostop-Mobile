namespace Autostop.Client.Abstraction.Providers
{
	public interface IMarkerSizeProvider
	{
		int GetHeight(float zoomLevel);

		int GetWidth(float zoomLevel);
	}
}