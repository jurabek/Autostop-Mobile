using System;
using Autostop.Client.Abstraction.Providers;

namespace Autostop.Client.Android.Providers
{
	public class KeyboardProvider : IKeyboardProvider
	{
		public int KeyboardHeight { get; set; }

		public int ScreenHeight { get; set; }

		public event EventHandler<EventArgs> KeyboardOpened;

		public event EventHandler<EventArgs> KeyboardClosed;

		public void OnKeyboardClosed() => KeyboardClosed?.Invoke(this, EventArgs.Empty);

		public void OnKeyboardOpened() => KeyboardOpened?.Invoke(this, EventArgs.Empty);
	}
}