using System;
using System.Threading.Tasks;
using Autostop.Client.Abstraction.Providers;
using Autostop.Common.Shared.Models;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using JetBrains.Annotations;
using Plugin.CurrentActivity;

namespace Autostop.Client.Android.Providers
{
    [UsedImplicitly]
    public class FirebasePhoneAuthenticationProvider : IPhoneAuthenticationProvider
    {
	    private readonly ICurrentActivity _currentActivity;
	    private PhoneAuthProvider.ForceResendingToken _forceResendToken;

	    public FirebasePhoneAuthenticationProvider(ICurrentActivity currentActivity)
	    {
		    _currentActivity = currentActivity;
	    }

        public async Task<AuthorizedUser> SignIn(string verificationId, string verificationCode)
        {
            PhoneAuthCredential credential = PhoneAuthProvider.GetCredential(verificationId, verificationCode);
            var result = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);

            return new AuthorizedUser
            {
                PhoneNumber = result.User.PhoneNumber,
                Uid = result.User.Uid
            };
        }

        public Task<VerifyNumberResult> VerifyPhoneNumber(string phoneNumber)
        {
            var tcs = new TaskCompletionSource<VerifyNumberResult>();

            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, TimeUnit.Seconds, _currentActivity.Activity, new VerificationCallback(
                    credential => {}, exception => tcs.SetException(exception),
                    (verificationId, forceResendingToken) =>
                    {
                        _forceResendToken = forceResendingToken;
                        tcs.SetResult(new VerifyNumberResult { VerificationId = verificationId });
                    }));

            return tcs.Task;
        }

        public Task<VerifyNumberResult> ResendVerificationCode(string phoneNumber)
        {
            var tcs = new TaskCompletionSource<VerifyNumberResult>();

            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, TimeUnit.Seconds, _currentActivity.Activity, new VerificationCallback(
                credential => { }, exception => tcs.SetException(exception),
                (verificationId, forceResendingToken) =>
                {
                    _forceResendToken = forceResendingToken;
                    tcs.SetResult(new VerifyNumberResult { VerificationId = verificationId });
                }), _forceResendToken);

            return tcs.Task;
        }

        private class VerificationCallback : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private readonly Action<PhoneAuthCredential> _onCompleted;
            private readonly Action<FirebaseException> _onFailed;
            private readonly Action<string, PhoneAuthProvider.ForceResendingToken> _onSent;

            public VerificationCallback(
                Action<PhoneAuthCredential> onCompleted,
                Action<FirebaseException> onFailed,
                Action<string, PhoneAuthProvider.ForceResendingToken> onSent)
            {
                _onCompleted = onCompleted;
                _onFailed = onFailed;
                _onSent = onSent;
            }
            public override void OnVerificationCompleted(PhoneAuthCredential credential)
            {
                _onCompleted(credential);
            }

            public override void OnVerificationFailed(FirebaseException exception)
            {
                _onFailed(exception);
            }

            public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
            {
                base.OnCodeSent(verificationId, forceResendingToken);
                _onSent(verificationId, forceResendingToken);
            }
        }
    }
}