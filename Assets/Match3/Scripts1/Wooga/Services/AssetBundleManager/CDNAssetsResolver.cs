using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000318 RID: 792
	public class CDNAssetsResolver : ABundleManifestResolver
	{
		// Token: 0x060018C7 RID: 6343 RVA: 0x00070815 File Offset: 0x0006EC15
		public CDNAssetsResolver(string originBaseUrl, string[] cdnBaseUrls, BundleManifest manifest, IEnumerable<string> activeVariants = null) : base(manifest, activeVariants)
		{
			this._cdnBaseUrls = CDNAssetsResolver.NormaliseBaseUrls(cdnBaseUrls);
			this._originBaseUrl = ((!originBaseUrl.EndsWith("/")) ? (originBaseUrl + '/') : originBaseUrl);
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00070855 File Offset: 0x0006EC55
		public override string GetUrlForAsset(IBundleInfo bundleInfo, bool cached)
		{
			return (!cached) ? CDNAssetsResolver.BuildUrl(this._originBaseUrl, bundleInfo) : CDNAssetsResolver.BuildUrl(this._cdnBaseUrls[0], bundleInfo);
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0007087C File Offset: 0x0006EC7C
		public override string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached)
		{
			if (cached)
			{
				string[] array = new string[this._cdnBaseUrls.Length];
				for (int i = 0; i < this._cdnBaseUrls.Length; i++)
				{
					array[i] = CDNAssetsResolver.BuildUrl(this._cdnBaseUrls[i], bundleInfo);
				}
				return array;
			}
			return new string[]
			{
				CDNAssetsResolver.BuildUrl(this._originBaseUrl, bundleInfo)
			};
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x000708E0 File Offset: 0x0006ECE0
		private static string[] NormaliseBaseUrls(string[] cdnBaseUrls)
		{
			string[] array = new string[cdnBaseUrls.Length];
			for (int i = 0; i < cdnBaseUrls.Length; i++)
			{
				string text = cdnBaseUrls[i];
				array[i] = ((!text.EndsWith("/")) ? (text + '/') : text);
			}
			return array;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00070935 File Offset: 0x0006ED35
		public static string BuildUrl(string baseUrl, IBundleInfo bundleInfo)
		{
			return (!URLHelper.IsAbsoluteUrl(bundleInfo.Url)) ? URLHelper.Concat(new string[]
			{
				baseUrl,
				bundleInfo.Url
			}) : bundleInfo.Url;
		}

		// Token: 0x040047D7 RID: 18391
		public const string DefaultManifestName = "MainManifest.json";

		// Token: 0x040047D8 RID: 18392
		private readonly string _originBaseUrl;

		// Token: 0x040047D9 RID: 18393
		private readonly string[] _cdnBaseUrls;
	}
}
