using System;
using System.Collections.Generic;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000312 RID: 786
	public static class ManifestLoader
	{
		// Token: 0x0600188E RID: 6286 RVA: 0x0006FB7C File Offset: 0x0006DF7C
		public static IEnumerator<BundleManifest> LoadManifest(string url, string md5Hash = null, FallbackInfo fallbackInfo = null)
		{
			bool flag = fallbackInfo != null && fallbackInfo.IsFallbackAllowed;
			if (flag)
			{
				return ManifestLoader.DownloadText(url, md5Hash).Catch(delegate(Exception e)
				{
					Log.DebugFormatted("Falling back to cached manifest file: {0}", new object[]
					{
						e
					});
					url = fallbackInfo.LastManifestInfo.Url;
					md5Hash = fallbackInfo.LastManifestInfo.MD5;
					return ManifestLoader.DownloadText(url, md5Hash);
				}).ContinueWith(delegate(string json)
				{
					BundleManifest result = JsonUtility.FromJson<BundleManifest>(json);
					fallbackInfo.Update(url, md5Hash);
					return result;
				});
			}
			return ManifestLoader.DownloadText(url, md5Hash).ContinueWith(delegate(string json)
			{
				BundleManifest result = JsonUtility.FromJson<BundleManifest>(json);
				if (fallbackInfo != null)
				{
					fallbackInfo.Update(url, md5Hash);
				}
				return result;
			});
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0006FC1A File Offset: 0x0006E01A
		private static IEnumerator<string> DownloadText(string url, string md5 = null)
		{
			return (!string.IsNullOrEmpty(md5)) ? DownloadHelper.DownloadTextCached(url, md5) : DownloadHelper.DownloadText(url, null);
		}
	}
}
