using System;
using System.IO;
using Wooga.Core.Utilities;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200032B RID: 811
	public static class WWWCache
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x00072361 File Offset: 0x00070761
		private static FileCache FileCache
		{
			get
			{
				if (WWWCache._fileCache == null)
				{
					WWWCache.Init(50);
				}
				return WWWCache._fileCache;
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00072379 File Offset: 0x00070779
		public static void Init(int cacheSize = 50)
		{
			WWWCache._fileCache = new FileCache(Path.Combine(Application.persistentDataPath, "wwwcache"), 50);
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00072396 File Offset: 0x00070796
		public static OptionalResult<string> Read(string url, string md5Hash = null)
		{
			return WWWCache.FileCache.ReadText(WWWCache.GetCachePath(url), md5Hash);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x000723A9 File Offset: 0x000707A9
		public static bool Write(string path, string content)
		{
			return WWWCache.FileCache.Write(WWWCache.GetCachePath(path), content);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000723BC File Offset: 0x000707BC
		public static void Cleanup()
		{
			if (WWWCache._fileCache != null)
			{
				WWWCache._fileCache.Cleanup();
			}
			else
			{
				Log.Error(new object[]
				{
					"Cannot cleanup uninitialised cache"
				});
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000723EA File Offset: 0x000707EA
		public static bool ClearCache()
		{
			return WWWCache.FileCache.Clear();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x000723F8 File Offset: 0x000707F8
		private static string GetCachePath(string url)
		{
			Uri uri = new Uri(url);
			return uri.LocalPath;
		}

		// Token: 0x040047F5 RID: 18421
		private static FileCache _fileCache;
	}
}
