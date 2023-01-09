using System.Collections;
using AndroidTools;
using UnityEngine;

namespace Match3.Scripts2.Network.Login
{
    public class LoginByFirebase : LoginBase
    {
        public override IEnumerator SetFormForLogin()
        {
            Debug.Log("login by firebase");
            // 登录google
            var ao = new AndroidObject(AndroidTools.AndroidAgent.Call<AndroidJavaObject>("getGoogleSignIn"));
            ao.Call("SignIn");
            
            // 等待3秒
            int waitTime = 3;
            string idToken = "";
            while (true)
            {
                if (waitTime <= 0)
                {
                    form = null;
                    yield break;
                }
                yield return new WaitForSeconds(1);
                waitTime--;
                
                idToken = AndroidTools.AndroidAgent.Call<string>("getIdToken");
                if (!string.IsNullOrEmpty(idToken))
                {
                    break;
                }
            }
            
            // 登录Firebase
            // Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            // Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
            int firebaseResult = 0;
            // auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            //     if (task.IsCanceled) {
            //         Debug.LogError("SignInWithCredentialAsync was canceled.");
            //         firebaseResult = 1;
            //         return;
            //     }
            //     if (task.IsFaulted)
            //     {
            //         string error = "SignInWithCredentialAsync encountered an error: " + task.Exception;
            //         Debug.LogError(error);
            //         firebaseResult = 1;
		          //   DataStatistics.Instance.TriggerEnterGameErrorEvent(3, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString(), error);
            //         return;
            //     }
            //
            //     Firebase.Auth.FirebaseUser newUser = task.Result;
                // Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                // firebaseResult = 2;
            // });
            
            // 获取Firebase的idToken
            while (firebaseResult == 0)
            {
                yield return null;
            }

            int getTokenResult = 0;
            if (firebaseResult == 2)
            {
                // auth.CurrentUser.TokenAsync(true).ContinueWith(task =>
                // {
                    // if (task.IsCanceled) {
                        // Debug.LogError("TokenAsync was canceled.");
                        // getTokenResult = 1;
                        // return;
                    // }

                    // if (task.IsFaulted)
                    // {
                        // string error = "TokenAsync encountered an error: " + task.Exception;
                        // Debug.LogError(error);
		                // DataStatistics.Instance.TriggerEnterGameErrorEvent(4, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString(), error);
                        // getTokenResult = 1;
                        // return;
                    // }

                    // idToken = task.Result;
                    // getTokenResult = 2;
                // });
            }
            else
            {
                form = null;
                yield break;
            }
            
            // 设置form
            while (getTokenResult == 0)
            {
                yield return null;
            }
            
            if (getTokenResult == 2)
            {
                form = new WWWForm();
                form.AddField("acctype", LoginType.firebasesub.ToString());
                form.AddField("account", idToken);
                yield return base.SetFormForLogin();
            }
            else
            {
                form = null;
                yield break;
            }
        }
    }
}