using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;

namespace Autostop.Client.Android
{
	public class FirebasePhoneAuthenticationProvider
	{
		public void VerifyPhoneNumber()
		{
			PhoneAuthProvider.Instance.VerifyPhoneNumber("", 60, TimeUnit.Seconds, new Activity(), new OnVerificationStateChanged());
		}
	}

	class OnVerificationStateChanged : PhoneAuthProvider.OnVerificationStateChangedCallbacks
	{
		public override void OnVerificationCompleted(PhoneAuthCredential credential)
		{

		}

		public override void OnVerificationFailed(FirebaseException exception)
		{
			if (exception is FirebaseTooManyRequestsException)
			{

			}
		}

		public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
		{
			base.OnCodeSent(verificationId, forceResendingToken);
		}
	}
}