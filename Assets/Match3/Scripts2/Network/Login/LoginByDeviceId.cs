using System.Collections;
using UnityEngine;

namespace Match3.Scripts2.Network.Login
{
    public class LoginByDeviceId : LoginBase
    {
        public override IEnumerator SetFormForLogin()
        {
            form = new WWWForm();
            form.AddField("acctype", LoginType.deviceid.ToString());
            form.AddField("account", SystemInfo.deviceUniqueIdentifier);
            yield return base.SetFormForLogin();
        }
    }
}