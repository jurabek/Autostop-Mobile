using System;

namespace Autostop.Client.Abstraction.Providers
{
	public interface IKeyboardProvider
    {
		event EventHandler<EventArgs> KeyboardOpened;

		event EventHandler<EventArgs> KeyboardClosed;

		int KeyboardHeight { get; set; }

		int ScreenHeight { get; set; }

		void OnKeyboardOpened();

		void OnKeyboardClosed();
    }
}
