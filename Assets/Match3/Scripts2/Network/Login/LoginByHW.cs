using System.Collections;
using AndroidTools;
using UnityEngine;

namespace Match3.Scripts2.Network.Login
{
    public class LoginByHW : LoginBase
    {
        public override IEnumerator SetFormForLogin()
        {
            string idtoken = AndroidTools.AndroidAgent.Call<string>("getIdToken");
            int waitTime = 3;
            while (string.IsNullOrEmpty(idtoken))
            {
                if (waitTime <= 0)
                {
                    break;
                }
                Debug.Log("等待登录");
                yield return new WaitForSeconds(1);
                waitTime--;
                idtoken = AndroidTools.AndroidAgent.Call<string>("getIdToken");
            }
            if (string.IsNullOrEmpty(idtoken))
            {
		        // DataStatistics.Instance.TriggerEnterGameErrorEvent(1, (int) Time.time, SystemInfo.deviceUniqueIdentifier, BuildVersion.Version, GameEnvironment.CurrentPlatform.ToString(), "idtoken is null");
                // 显式登录
                AndroidTools.AndroidAgent.Call("signin");
                form = null;
                yield break;
            }
            // 如果获取到idtoken, 则设置form
            form = new WWWForm();
            form.AddField("acctype", LoginType.hwsub.ToString());
            form.AddField("account", idtoken);
            yield return base.SetFormForLogin();
        }
    }
}