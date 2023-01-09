using System;
using System.Collections.Generic;
using System.Net;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using UnityEngine;
using UnityEngine.Networking;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000324 RID: 804
	public static class DownloadHelper
	{
		// Token: 0x0600190A RID: 6410 RVA: 0x00071578 File Offset: 0x0006F978
		public static IEnumerator<AssetBundle> DownloadAssetBundleUncached(string bundleName, string url, uint crc = 0U, Action<float> reportProgress = null)
		{
			if (!url.StartsWith("jar:") && !url.Contains("?"))
			{
				url = url + "?" + DateTime.Now.ToFileTimeUtc();
			}
			if (!DownloadHelper.UseCRC)
			{
				crc = 0U;
			}
			Log.DebugFormatted("Downloading: {0} crc: {1}", new object[]
			{
				url,
				crc
			});
			using (UnityWebRequest webRequest = new UnityWebRequest(url))
			{
				DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(url, crc);
				webRequest.downloadHandler = handler;
				webRequest.disposeDownloadHandlerOnDispose = true;
				webRequest.SendWebRequest();
				while (DownloadHelper.CheckInProgress(url, webRequest, null))
				{
					if (reportProgress != null)
					{
						reportProgress(webRequest.downloadProgress);
					}
					yield return null;
				}
				if (reportProgress != null)
				{
					reportProgress(1f);
				}
				AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);
				if (assetBundle == null)
				{
					throw new AssetBundleLoadException(webRequest.error, url);
				}
				AssetBundleCache.GetInstance().AddBundle(bundleName, assetBundle);
				yield return assetBundle;
			}
			yield break;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x000715A8 File Offset: 0x0006F9A8
		public static IEnumerator<AssetBundle> DownloadAssetBundleCached(string bundleName, string url, Hash128 hash, uint crc = 0U, Action<float> reportProgress = null)
		{
			if (!DownloadHelper.UseCRC)
			{
				crc = 0U;
			}
			using (UnityWebRequest webRequest = new UnityWebRequest(url))
			{
				DownloadHandlerAssetBundle handler = (!hash.isValid) ? new DownloadHandlerAssetBundle(url, crc) : new DownloadHandlerAssetBundle(url, hash, crc);
				if (Log.MinSeverity == SeverityId.Debug)
				{
					Log.DebugFormatted((!Caching.IsVersionCached(url, hash)) ? "Downloading: {0} crc: {1} hash: {2}" : "Open cached version: {0} crc: {1} hash: {2}", new object[]
					{
						url,
						crc,
						hash
					});
				}
				webRequest.downloadHandler = handler;
				webRequest.disposeDownloadHandlerOnDispose = true;
				webRequest.SendWebRequest();
				while (DownloadHelper.CheckInProgress(url, webRequest, null))
				{
					if (reportProgress != null)
					{
						reportProgress(webRequest.downloadProgress);
					}
					yield return null;
				}
				if (reportProgress != null)
				{
					reportProgress(1f);
				}
				AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);
				if (assetBundle == null)
				{
					throw new AssetBundleLoadException(webRequest.error, url);
				}
				AssetBundleCache.GetInstance().AddBundle(bundleName, assetBundle);
				yield return assetBundle;
			}
			yield break;
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x000715E0 File Offset: 0x0006F9E0
		public static IEnumerator<string> DownloadText(string url, string md5 = null)
		{
			Log.DebugFormatted("Downloading: {0}", new object[]
			{
				url
			});
			using (UnityWebRequest webRequest = new UnityWebRequest(url))
			{
				webRequest.downloadHandler = new DownloadHandlerBuffer();
				webRequest.SendWebRequest();
				while (DownloadHelper.CheckInProgress(url, webRequest, md5))
				{
					yield return null;
				}
				yield return webRequest.downloadHandler.text;
			}
			yield break;
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00071604 File Offset: 0x0006FA04
		public static IEnumerator<string> DownloadTextCached(string url, string md5 = null)
		{
			OptionalResult<string> optionalResult = WWWCache.Read(url, md5);
			if (optionalResult.HasValue)
			{
				return optionalResult.Value.Yield<string>();
			}
			return DownloadHelper.DownloadText(url, md5).ContinueWith(delegate(string content)
			{
				WWWCache.Write(url, content);
				return content;
			});
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00071664 File Offset: 0x0006FA64
		private static bool CheckInProgress(string url, UnityWebRequest webRequest, string md5 = null)
		{
			if (webRequest.isDone)
			{
				if (webRequest.responseCode < 0L || webRequest.responseCode >= 500L || !string.IsNullOrEmpty(webRequest.error))
				{
					throw new WwwErrorException((HttpStatusCode)webRequest.responseCode, webRequest.error, url);
				}
				if (md5 != null && !MD5Util.VerifyMd5Hash(webRequest.downloadHandler.data, md5))
				{
					string md5Hash = MD5Util.GetMd5Hash(webRequest.downloadHandler.data);
					throw new WwwErrorException(string.Concat(new string[]
					{
						"MD5 Mismatch: expected='",
						md5,
						"' real='",
						md5Hash,
						"'"
					}), url);
				}
			}
			return !webRequest.downloadHandler.isDone;
		}

		// Token: 0x040047EA RID: 18410
		public static readonly Hash128 BaseVersion = new Hash128(0U, 0U, 0U, 1U);

		// Token: 0x040047EB RID: 18411
		public static bool UseCRC = false;
	}
}
