using UnityEngine;

namespace AndroidTools
{
    public class AndroidObject
    {
        
        private AndroidJavaObject jo;
        public AndroidObject(AndroidJavaObject jo)
        {
            this.jo = jo;
        }

        public void Call(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if(jo != null)
                {
                    Debug.Log("call:" + methodName);
                    jo?.Call(methodName, args);
                }
                else
                {
                    Debug.LogError("jo未初始化完成:" + methodName);
                }
            }
        }

        public T Call<T>(string methodName, params object[] args)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (jo != null)
                {
                    return jo.Call<T>(methodName, args);
                }
                else
                {
                    Debug.LogError("jo未初始化完成:" + methodName);
                }
            }

            return default(T);
        }
    }
}