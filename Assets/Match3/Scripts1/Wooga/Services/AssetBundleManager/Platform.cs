using System;
using System.IO;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000313 RID: 787
	public static class Platform
	{
		// Token: 0x06001890 RID: 6288 RVA: 0x0006FD07 File Offset: 0x0006E107
		public static string GetPlatformName()
		{
			return Platform.GetPlatformForAssetBundles(Application.platform);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0006FD14 File Offset: 0x0006E114
		public static string GetStreamingAssetsPath()
		{
			if (Application.isEditor)
			{
				return "file://" + Environment.CurrentDirectory.Replace("\\", "/");
			}
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
			}
			if (Application.isMobilePlatform || Application.isConsolePlatform)
			{
				return Application.streamingAssetsPath;
			}
			return "file://" + Application.streamingAssetsPath;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0006FDA4 File Offset: 0x0006E1A4
		public static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			switch (platform)
			{
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				return "Standalone";
			default:
				if (platform != RuntimePlatform.WebGLPlayer)
				{
					return null;
				}
				return "WebGL";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.Android:
				return "Android";
			}
		}

		// Token: 0x040047C1 RID: 18369
		public const string AssetBundlesOutputPath = "AssetBundles";
	}
}
