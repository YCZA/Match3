using System.IO;
using System.Text;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// eli key point URLHelper
	public static class URLHelper
	{
		// Token: 0x060018F8 RID: 6392 RVA: 0x00071109 File Offset: 0x0006F509
		public static string GetOriginBaseUrl(string account, string environment)
		{
			// return string.Format("https://s3.amazonaws.com/cdn-service-production/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
			// https://s3.amazonaws.com/cdn-service-production/{0}/{1}/{2}
			return string.Format("https://host2333/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0007111C File Offset: 0x0006F51C
		public static string GetCdnBaseUrl(string account, string environment)
		{
			// return string.Format("https://cdn.wooga.com/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
			return string.Format("https://----host2333/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0007112F File Offset: 0x0006F52F
		public static string GetFallbackCdnBaseUrl(string account, string environment)
		{
			// return string.Format("https://cdn-2nd.wooga.com/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
			return string.Format("https://----host2333/{0}/{1}/{2}", account, environment, Platform.GetPlatformName());
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00071144 File Offset: 0x0006F544
		public static string Concat(params string[] elems)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in elems)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text[0] == '/')
					{
						if (stringBuilder.Length == 0 || stringBuilder[stringBuilder.Length - 1] != '/')
						{
							stringBuilder.Append(text);
						}
						else
						{
							stringBuilder.Append(text, 1, text.Length - 1);
						}
					}
					else
					{
						if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != '/')
						{
							stringBuilder.Append('/');
						}
						stringBuilder.Append(text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x00071205 File Offset: 0x0006F605
		public static string InsertCacheKiller(string path, string cacheKiller)
		{
			return URLHelper.Concat(new string[]
			{
				Path.GetDirectoryName(path),
				Path.GetFileNameWithoutExtension(path) + "_" + cacheKiller + Path.GetExtension(path)
			});
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x00071235 File Offset: 0x0006F635
		public static bool IsAbsoluteUrl(string url)
		{
			return !string.IsNullOrEmpty(url) && (url.StartsWith("https://") || url.StartsWith("http://") || url.StartsWith("file:/"));
		}
	}
}
