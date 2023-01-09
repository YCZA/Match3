using UnityEngine;

namespace MyFile.Level.Standard_Assets.Scripts
{
    public class AndroidRotationLockUtil
    {
        private static AndroidJavaClass unity;

        public static bool AllowAutorotation()
        {
            bool result = false;
            // using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityutils.RotationLockUtil"))
            // {
            // result = (androidJavaClass.CallStatic<int>("GetAutorotateSetting", new object[]
            // {
            // AndroidRotationLockUtil.GetUnityActivity()
            // }) != 0);
            // }
            return result;
        }

        private static AndroidJavaObject GetUnityActivity()
        {
            if (AndroidRotationLockUtil.unity == null)
            {
                // AndroidRotationLockUtil.unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
            return AndroidRotationLockUtil.unity.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}