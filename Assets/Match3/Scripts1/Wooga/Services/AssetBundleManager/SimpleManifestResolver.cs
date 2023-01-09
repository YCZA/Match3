namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200031F RID: 799
	public class SimpleManifestResolver : IManifestResolver
	{
		// Token: 0x060018EE RID: 6382 RVA: 0x00070EF3 File Offset: 0x0006F2F3
		public SimpleManifestResolver(string baseUrl = null)
		{
			this._baseUrl = (baseUrl ?? SimpleManifestResolver.GetLocalServerBaseUrl());
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00070F10 File Offset: 0x0006F310
		public IManifestInfo Resolve(string manifestName)
		{
			string url = URLHelper.Concat(new string[]
			{
				this._baseUrl,
				manifestName
			});
			return new ManifestInfo
			{
				URL = url,
				OriginBaseURL = this._baseUrl,
				CDNBaseURLs = new string[]
				{
					this._baseUrl,
					this._baseUrl
				}
			};
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00070F6E File Offset: 0x0006F36E
		private static string GetLocalServerBaseUrl()
		{
			return "http://127.0.0.1:7888/" + Platform.GetPlatformName() + "/";
		}

		// Token: 0x040047E5 RID: 18405
		private readonly string _baseUrl;
	}
}
