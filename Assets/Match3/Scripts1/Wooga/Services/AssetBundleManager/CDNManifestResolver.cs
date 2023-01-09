using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000319 RID: 793
	public class CDNManifestResolver : IManifestResolver
	{
		// Token: 0x060018CC RID: 6348 RVA: 0x0007096C File Offset: 0x0006ED6C
		public CDNManifestResolver(string sbsAccount, string sbsEnvironment, Dictionary<string, string> manifestToMd5 = null, string path = null)
		{
			this._manifestToMd5 = (manifestToMd5 ?? new Dictionary<string, string>());
			this._originUrl = URLHelper.Concat(new string[]
			{
				URLHelper.GetOriginBaseUrl(sbsAccount, sbsEnvironment),
				path
			});
			this._cdnUrl = URLHelper.Concat(new string[]
			{
				URLHelper.GetCdnBaseUrl(sbsAccount, sbsEnvironment),
				path
			});
			this._fallbackCdnUrl = URLHelper.Concat(new string[]
			{
				URLHelper.GetFallbackCdnBaseUrl(sbsAccount, sbsEnvironment),
				path
			});
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x000709F4 File Offset: 0x0006EDF4
		public IManifestInfo Resolve(string manifestName)
		{
			string cacheKiller;
			string url = (!this._manifestToMd5.TryGetValue(manifestName, out cacheKiller)) ? URLHelper.Concat(new string[]
			{
				this._originUrl,
				manifestName
			}) : URLHelper.Concat(new string[]
			{
				this._cdnUrl,
				URLHelper.InsertCacheKiller(manifestName, cacheKiller)
			});
			return new ManifestInfo
			{
				URL = url,
				OriginBaseURL = this._originUrl,
				CDNBaseURLs = new string[]
				{
					this._cdnUrl,
					this._fallbackCdnUrl
				}
			};
		}

		// Token: 0x040047DA RID: 18394
		private readonly Dictionary<string, string> _manifestToMd5;

		// Token: 0x040047DB RID: 18395
		private readonly string _originUrl;

		// Token: 0x040047DC RID: 18396
		private readonly string _cdnUrl;

		// Token: 0x040047DD RID: 18397
		private readonly string _fallbackCdnUrl;
	}
}
