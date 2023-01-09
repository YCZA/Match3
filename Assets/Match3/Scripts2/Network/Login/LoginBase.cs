using System.Collections;
using Match3.Scripts1.Puzzletown.Build;
using UnityEngine;

namespace Match3.Scripts2.Network.Login
{
    public class LoginBase
    {
        public WWWForm form = null;

        // 向form中添加Field, 要保证form已初始化(form = new WWWForm())
        // 如果设置失败，要保证form为null
        public virtual IEnumerator SetFormForLogin()
        {
            if(form == null)
            {
                form = new WWWForm();
            }
            form.AddField("ver", BuildVersion.INTERNAL_VERSION);
            yield break;
        }
    }
}