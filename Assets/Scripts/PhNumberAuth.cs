using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
public class PhNumberAuth : MonoBehaviour {
	public InputField Phonenumber;
	public InputField VerifyOTP;
	public InputField VerifyOTPAgain;
	public Button BtnsendOTP;
	public Button BtnverifyOTP;
	FirebaseAuth firebaseAuth;
	PhoneAuthProvider provider;
	FirebaseUser user;
	private uint phoneAuthTimeoutMs = 0;
	private string verificationID;
	public Text txtdebug;
	void Start() {
		firebaseAuth = FirebaseAuth.DefaultInstance;
		provider= PhoneAuthProvider.GetInstance(firebaseAuth);
		Phonenumber.text = string.Empty;
		VerifyOTP.text = string.Empty;
	}

	public void OnClickSendOTP() {
		Send_OTP(Phonenumber.text);
	}

	public void Send_OTP(string phno) {
		provider.VerifyPhoneNumber(phno, phoneAuthTimeoutMs, null,
		  verificationCompleted: (credential) => {
			  txtdebug.text = "Credential" + credential.ToString();
		  },
		  verificationFailed: (error) => {
			  txtdebug.text = error;
		  },
		  codeSent: (id, token) => {
			  verificationID = id;
			  txtdebug.text = "Code Sent";

		  },
		  codeAutoRetrievalTimeOut: (id) => {

		  });
	}

	public void Verify_OTP() {
		Credential credential = provider.GetCredential(verificationID, VerifyOTP.text);
		firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsFaulted) {
				txtdebug.text = "SignInWithCredentialAsync encountered an error: " +
							   task.Exception;
				return;
			}

			FirebaseUser newUser = task.Result;
			txtdebug.text = "User signed in successfully";
			txtdebug.text = "Phone number: " + newUser.PhoneNumber;
			txtdebug.text = "Phone provider ID: " + newUser.ProviderId;
			//Phonenumber.text = string.Empty;
			//VerifyOTP.text = string.Empty;
		});
	}

	public void ShowData() {
		user = firebaseAuth.CurrentUser;
		if (user != null) {
			string uid = user.UserId;
			string phno = user.PhoneNumber;
			string providerName = user.ProviderId;
			txtdebug.text = uid + "\n" + phno + "\n" + providerName;
		}
	}

	public void GetVerifyOTP_Again() {
		user = firebaseAuth.CurrentUser;
		Send_OTP(user.PhoneNumber);
	}

	public void VerifyOTP_Again() {
		Credential credential = provider.GetCredential(verificationID, VerifyOTPAgain.text);
		firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsFaulted) {
				txtdebug.text = "SignInWithCredentialAsync encountered an error: " +
							   task.Exception;
				return;
			}

			FirebaseUser newUser = task.Result;
			txtdebug.text = "User verified successfully";
		});
	}

	void OnApplicationQuit() {
		firebaseAuth.SignOut();
	}

}
