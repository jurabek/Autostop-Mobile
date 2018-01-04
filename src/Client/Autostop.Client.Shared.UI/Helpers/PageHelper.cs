using Xamarin.Forms;

namespace Autostop.Client.Shared.UI.Helpers
{
    public static class PageHelper
    {
		public static readonly BindableProperty BarTintColorProperty = 
			BindableProperty.CreateAttached("BarTintColor", typeof(Color), typeof(Page), Color.Transparent);

	    public static Color GetBarTintColor(BindableObject page)
	    {
		    return (Color)page.GetValue(BarTintColorProperty);
	    }

	    public static void SetBarTintColor(BindableObject page, Color color)
	    {
		    page.SetValue(BarTintColorProperty, color);
	    }
	}
}
