using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.Env;
using Match3.Scripts2.Network.Login;
using UnityEngine;
using UnityEngine.Networking;

namespace Match3.Scripts2.Network
{
    public class ToServer : MonoBehaviour
    {
        enum MsgCode
        {
            success = 0,
            error = 1
        }

        public enum ErrorCode
        {
            unknown = -2,
            serverOrNetworkErr = -1,
            generalErr = 0
        }
        private class Response
        {
            public MsgCode code = MsgCode.error;
            public string content = "";
        }

        public class ErrorInfo
        {
            public ErrorCode code = ErrorCode.unknown;
            public string err = "error: unknown";
        }
        
        public static string userId { get; private set; }// 登录后才会有值
        public static string temppw { get; private set; }// 登录后才会有值
        
        private static ToServer toServer = null;
        public static ToServer Instance
        {
            get
            {
                if (toServer == null)
                {
                    toServer = new GameObject("ToServer").AddComponent<ToServer>();
                }

                return toServer;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            toServer = null;
        }

        #region Tools

        private void SendSync(UnityWebRequest request)
        {
            if (string.IsNullOrEmpty(userId) && request.url != URL.Login)
            {
                Debug.LogError("请先登录游戏");
                return;
            }
            
            request.SetRequestHeader("c-userid", userId);
            request.SetRequestHeader("c-temppw", temppw);
            Debug.Log("<color=blue>sendRequest</color>:" + request.url);
            request.SendWebRequest();
            while (!request.isDone)
            {
                Thread.Sleep(30);
            }
            
            if (request.responseCode == 200L)
            {
                if (string.IsNullOrEmpty(request.downloadHandler.text))
                {
                    // 服务器可能出错了, 或参数错误
                    Debug.Log("服务器处理失败: " + request.url);
                    return;
                }
                
                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
                if (response.code == MsgCode.success)
                {
                    Debug.Log("收到：" + response.content);
                }
                else
                {
                    // 服务器没出错，但操作失败
                    Debug.Log("服务器处理失败: " + request.url + " 原因："+response.content);
                }
            }
            else
            {
                // 服务器出错，或网络错误
                Debug.Log("连接服务器失败: " + request.error);
                StartCoroutine(PopupMissingAssetsRoot.TryShowRoutine(""));
            }
        }
        
        private IEnumerator Send(UnityWebRequest request, Action<string> successCb, Action<ErrorInfo> failureCb = null)
        {
            if (string.IsNullOrEmpty(userId) && request.url != URL.Login && request.url != URL.PostEventInfo)
            {
                Debug.LogError("Please login first");
                yield break;
            }
            
            request.SetRequestHeader("c-userid", userId??"");
            request.SetRequestHeader("c-temppw", temppw??"");
            Debug.Log("<color=blue>sendRequest</color>:" + request.url);
            yield return request.SendWebRequest();
            if (request.responseCode == 200L)
            {
                if (string.IsNullOrEmpty(request.downloadHandler.text))
                {
                    // 服务器可能出错了, 或参数错误
                    failureCb?.Invoke(new ErrorInfo()
                    {
                        code = ErrorCode.serverOrNetworkErr,
                        err = "200"
                    });
                    Debug.Log("server error: " + request.url);
                    yield break;
                }
                
                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
                if (response.code == MsgCode.success)
                {
                    Debug.Log("收到：" + response.content);
                    successCb?.Invoke(response.content);
                }
                else
                {
                    // 服务器没出错，但操作失败
                    ErrorInfo errorInfo = new ErrorInfo();
                    try
                    {
                        errorInfo = JsonUtility.FromJson<ErrorInfo>(response.content);
                        if (errorInfo == null)
                        {
                            errorInfo = new ErrorInfo();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    failureCb?.Invoke(errorInfo);
                    Debug.Log("server error: " + request.url + " response："+response.content);
                }
            }
            else
            {
                // 服务器出错，或网络错误
                failureCb?.Invoke(new ErrorInfo()
                {
                    code = ErrorCode.serverOrNetworkErr,
                    err = request.responseCode.ToString()
                });
                Debug.Log("server or network error: " + request.error);
            }
        }

        private IEnumerator SendByGet(string url, Action<string> successCb, Action<ErrorInfo> failureCb = null)
        {
		    UnityWebRequest request = UnityWebRequest.Get(url);
            yield return Send(request, successCb, failureCb);
        }

        private IEnumerator SendByPost(string url, WWWForm form, Action<string> successCb, Action<ErrorInfo> failureCb = null)
        {
		    UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return Send(request, successCb, failureCb);
        }
        #endregion

        #region logic

        private string GetErrorText(ErrorInfo errorInfo)
        {
            if (errorInfo.code == ErrorCode.serverOrNetworkErr)
            {
                return "Network connection error. Error code: " + errorInfo.err;
            }
            else
            {
                return errorInfo.err;
            }
        }
        public IEnumerator Login()
        {
            // 确定用户使用的平台
            LoginBase loginHandler = new LoginByHW();
            if (Application.isEditor)
            {
                loginHandler = new LoginByDeviceId();
            }
            else
            {
                switch (GameEnvironment.CurrentPlatform)
                {
                    case GameEnvironment.Platform.Samsung_Abroad:
                        loginHandler = new LoginByFirebase();
                        break;
                    case GameEnvironment.Platform.HW_Abroad:
                        loginHandler = new LoginByHW();
                        break;
                    default:
                        // 登录失败
                        yield return new List<object>{false, "Login type error: " + GameEnvironment.CurrentPlatform.ToString()};
                        yield break;
                }
            }

            // 获取登录所需参数
            yield return loginHandler.SetFormForLogin();
            WWWForm form = loginHandler.form;

            if (form == null)
            {
                yield return new List<object>{false, "Login failed."};   // 返回的错误为空，则会重新尝试登录，不会弹出错误对话框
                yield break;
            }
            
            // 开始登录
            bool isSuccess = false;
            string err = "";
            yield return SendByPost(URL.Login, form, (content)=>
            {
                userId = content.Split(' ')[0];
                if (Application.isEditor)
                {
                    temppw = "18a9g3b5G8sH234Gsip3";
                }
                else
                {
                    temppw = content.Split(' ')[1];
                }
                Debug.Log("登录成功：" + userId);
                isSuccess = true;
            }, errorInfo =>
            {
                isSuccess = false;
                err = GetErrorText(errorInfo);
            });
            yield return new List<object>{isSuccess, err};
        }

        public IEnumerator PullArchive()
        {
            string archive = "";
            bool isSuccess = false;
            yield return SendByGet(URL.PullArchive, s =>
            {
                archive = s;
                isSuccess = true;
                Debug.Log("successed to pull archive：" + archive);
            }, s =>
            {
                isSuccess = false;
                Debug.LogError("failed to pull archive: " + s);
            });

            if (isSuccess)
            {
                yield return archive;
            }
            else
            {
                // todo 应该有弹窗提示
                throw new Exception("failed to pull archive");
            }
        }

        private IEnumerator PushArchiveIE(string archive)
        {
            WWWForm form = new WWWForm();
            form.AddField("archive", archive);
            UnityWebRequest request = UnityWebRequest.Post(URL.PushArchive, form);
            ErrorInfo errorInfo = null;
            yield return SendByPost(URL.PushArchive, form, s => errorInfo = null, info =>
            {
                errorInfo = info;
            });
            
            // 服务器内部错误或服务器给出了错误信息，则不重新上传
            while (errorInfo != null)
            {
                // 弹窗暂时游戏，上传完成才能继续
                yield return PopupMissingAssetsRoot.TryShowRoutine("");
                
		        Wooroutine<LoadingSpinnerRoot> loadingSpinner = SceneManager.Instance.LoadScene<LoadingSpinnerRoot>(null);
		        yield return loadingSpinner;
		        yield return new WaitForSeconds(loadingSpinner.ReturnValue.dialog.open.length + 1f);
                // 重新上传
                yield return SendByPost(URL.PushArchive, form, s =>
                {
                    errorInfo = null;
                }, info =>
                {
                    errorInfo = info;
                });
                loadingSpinner.ReturnValue.Close();
            }
        }

        public void PushArchiveAsync(string archive)
        {
            StartCoroutine(PushArchiveIE(archive));
        }
        
        public void PushArchiveSync(string archive)
        {
            // 暂时使用同步的方式上传存档
            WWWForm form = new WWWForm();
            form.AddField("archive", archive);
            UnityWebRequest request = UnityWebRequest.Post(URL.PushArchive, form);
            SendSync(request);
        }

        public IEnumerator DelArchive(Action cb)
        {
            yield return SendByGet(URL.DelArchive, s =>
            {
                Debug.Log("删除存档数量: " + s);
                cb?.Invoke();
            });
        }

        public void VerifyPurchaseResult(string content, string sign, Action<string> successCb, Action<ErrorInfo> failureCb)
        {
            WWWForm form = new WWWForm();
            form.AddField("content", content);
            form.AddField("sign", sign);
            StartCoroutine(SendByPost(URL.VerifyPurchaseResult, form, successCb, failureCb));
        }

        private IEnumerator PushEventInfoIE(WWWForm form, Action<string> successCb, Action<ErrorInfo> failureCb)
        {
            yield return SendByPost(URL.PostEventInfo, form, successCb, failureCb);
        }
        public void PushEventInfo(WWWForm form, Action<string> successCb, Action<ErrorInfo> failureCb)
        {
            StartCoroutine(PushEventInfoIE(form, successCb, failureCb));
        }
        #endregion
    }
}