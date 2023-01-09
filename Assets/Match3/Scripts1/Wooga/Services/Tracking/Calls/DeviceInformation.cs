using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.Tracking.Calls
{
	// Token: 0x02000437 RID: 1079
	public class DeviceInformation
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x00084FAE File Offset: 0x000833AE
		private static AndroidJavaClass android_os_Build
		{
			get
			{
				if (DeviceInformation.__android_os_Build == null)
				{
					// DeviceInformation.__android_os_Build = new AndroidJavaClass("android.os.Build");
				}
				return DeviceInformation.__android_os_Build;
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x00084FCE File Offset: 0x000833CE
		public static void Track()
		{
			DeviceInformation.TrackAndroidDeviceInformation();
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x00084FD8 File Offset: 0x000833D8
		private static void TrackAndroidDeviceInformation()
		{
			Tracking.TrackOnce("device", new Dictionary<string, object>
			{
				{
					"ram",
					SystemInfo.systemMemorySize
				},
				{
					"gsl",
					SystemInfo.graphicsShaderLevel
				},
				{
					"gdn",
					SystemInfo.graphicsDeviceName
				},
				{
					"gdve",
					SystemInfo.graphicsDeviceVendor
				},
				{
					"gdv",
					SystemInfo.graphicsDeviceVersion
				},
				{
					"model",
					DeviceInformation.ProductModel()
				},
				{
					"dev",
					DeviceInformation.DeviceCode()
				},
				{
					"proc",
					SystemInfo.processorType
				},
				{
					"brand",
					DeviceInformation.ProductBrand()
				},
				{
					"sres",
					(Screen.currentResolution.width <= Screen.currentResolution.height) ? (Screen.currentResolution.height + "x" + Screen.currentResolution.width) : (Screen.currentResolution.width + "x" + Screen.currentResolution.height)
				},
				{
					"dpi",
					Screen.dpi
				}
			});
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x00085133 File Offset: 0x00083533
		private static string ProductModel()
		{
			if (Application.isEditor)
			{
				return "editor";
			}
			// return DeviceInformation.android_os_Build.GetStatic<string>("MODEL");
			return "no editor";
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x00085154 File Offset: 0x00083554
		private static string DeviceCode()
		{
			if (Application.isEditor)
			{
				return "editor";
			}
			// return DeviceInformation.android_os_Build.GetStatic<string>("DEVICE");
			return "no editor";
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x00085175 File Offset: 0x00083575
		private static string ProductBrand()
		{
			if (Application.isEditor)
			{
				return "editor";
			}
			// return DeviceInformation.android_os_Build.GetStatic<string>("BRAND");
			return "no editor";
		}

		// Token: 0x04004AED RID: 19181
		private const string CALL_NAME = "device";

		// Token: 0x04004AEE RID: 19182
		private static AndroidJavaClass __android_os_Build;
	}
}
