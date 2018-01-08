using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Autostop.Client.Android.Views;
using Firebase.Messaging;

namespace Autostop.Client.Android.Services
{
	[Service, IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class AutostopFirebaseListenerService : FirebaseMessagingService
	{
		public override void OnMessageReceived(RemoteMessage message)
		{
			base.OnMessageReceived(message);

			var notification = message.GetNotification();
			var title = notification.Title;
			var body = notification.Body;

			SendNotification(title, body);
		}

		private void SendNotification(string title, string body)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);

			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

			var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);

			var notificationBuilder =
				new NotificationCompat.Builder(this)
					.SetSmallIcon(Resource.Drawable.pin_pickup)
					.SetContentTitle(title)
					.SetContentText(body)
					.SetAutoCancel(true)
					.SetSound(defaultSoundUri)
					.SetContentIntent(pendingIntent);

			var notificationManager = NotificationManager.FromContext(this);
			notificationManager.Notify(0, notificationBuilder.Build());
		}
	}
}