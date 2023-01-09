using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Core.DeviceInfo
{
	// Token: 0x0200034D RID: 845
	public static class DeviceId
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x00074888 File Offset: 0x00072C88
		public static string uniqueIdentifier
		{
			get
			{
				if (DeviceId._uniqueIdentifier == null)
				{
					DeviceId._uniqueIdentifier = DeviceId.NativeUniqueIdentifier();
				}
				return DeviceId._uniqueIdentifier;
			}
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000748A3 File Offset: 0x00072CA3
		private static string NativeUniqueIdentifier()
		{
			return DeviceId.AndroidUniqueIdentifierCached();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x000748AC File Offset: 0x00072CAC
		private static string AndroidUniqueIdentifierCached()
		{
			string key = "Wooga.Device.UniqueAndroidDeviceId";
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			string text = DeviceId.AndroidUniqueIdentifier();
			PlayerPrefs.SetString(key, text);
			return text;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x000748E0 File Offset: 0x00072CE0
		private static string AndroidUniqueIdentifier()
		{
			string str = DeviceId.AndroidDeviceModel() + " running Apportable";
			string text = DeviceId.AndroidId() + DeviceId.AndroidMacAddress() + ":" + str;
			global::UnityEngine.Debug.Log("andoid id, hashing: " + text);
			return DeviceId.HashIdentifier(7778, text);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00074930 File Offset: 0x00072D30
		public static string AndroidId()
		{
			if (DeviceId._androidId != null)
			{
				return DeviceId._androidId;
			}
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			// AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
			// AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
			// string static2 = androidJavaClass2.GetStatic<string>("ANDROID_ID");
			// string text = androidJavaClass2.CallStatic<string>("getString", new object[]
			// {
				// androidJavaObject,
				// static2
			// });
			global::UnityEngine.Debug.Log("android id " + "@@@@@@####");
			// DeviceId._androidId = text;
			return DeviceId._androidId;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000749C8 File Offset: 0x00072DC8
		private static string AndroidDeviceModel()
		{
			// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
			// return androidJavaClass.GetStatic<string>("MODEL");
			return "";
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000749EC File Offset: 0x00072DEC
		public static string AndroidManufacturer()
		{
			if (DeviceId._androidManufacturer == null)
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
				// DeviceId._androidManufacturer = androidJavaClass.GetStatic<string>("MANUFACTURER");
			}
			return DeviceId._androidManufacturer;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00074A24 File Offset: 0x00072E24
		public static string AndroidModel()
		{
			if (DeviceId._androidModel == null)
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
				// DeviceId._androidModel = androidJavaClass.GetStatic<string>("MODEL");
			}
			return DeviceId._androidModel;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00074A5C File Offset: 0x00072E5C
		public static string AndroidDeviceCode()
		{
			if (DeviceId._androidDeviceCode == null)
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
				// DeviceId._androidDeviceCode = androidJavaClass.GetStatic<string>("DEVICE");
			}
			return DeviceId._androidDeviceCode;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00074A94 File Offset: 0x00072E94
		public static int AndroidApiLevel()
		{
			if (DeviceId._androidApiLevel == 0)
			{
				IntPtr clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
				IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
				DeviceId._androidApiLevel = AndroidJNI.GetStaticIntField(clazz, staticFieldID);
			}
			return DeviceId._androidApiLevel;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00074AD8 File Offset: 0x00072ED8
		private static string AndroidMacAddress()
		{
			AndroidJavaObject androidJavaObject;
			// using (AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
			// {
				// androidJavaObject = @static.Call<AndroidJavaObject>("getSystemService", new object[]
				// {
					// "wifi"
				// });
			// }
			// return androidJavaObject.Call<AndroidJavaObject>("getConnectionInfo", new object[0]).Call<string>("getMacAddress", new object[0]);
			return "";
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00074B58 File Offset: 0x00072F58
		private static string HashIdentifier(int prefix, string addressAndDeviceName)
		{
			MD5 md = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(addressAndDeviceName);
			byte[] array = md.ComputeHash(bytes);
			ulong num = (ulong)array[0] | (ulong)array[1] << 8 | (ulong)array[2] << 16 | (ulong)array[3] << 24 | (ulong)array[4] << 32 | (ulong)array[5] << 40;
			return (num + (ulong)((long)prefix * 1000000000000000L)).ToString();
		}

		// Token: 0x0400484B RID: 18507
		private static string _uniqueIdentifier;

		// Token: 0x0400484C RID: 18508
		private static string _androidId;

		// Token: 0x0400484D RID: 18509
		private static string _androidManufacturer;

		// Token: 0x0400484E RID: 18510
		private static string _androidModel;

		// Token: 0x0400484F RID: 18511
		private static string _androidDeviceCode;

		// Token: 0x04004850 RID: 18512
		private static int _androidApiLevel;
	}
}
