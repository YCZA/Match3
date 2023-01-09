using System;
using System.Collections.Generic;
using Wooga.Core.Utilities;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000320 RID: 800
	public class StreamingAssetsResolver : ABundleManifestResolver
	{
		// Token: 0x060018F1 RID: 6385 RVA: 0x00070F84 File Offset: 0x0006F384
		private StreamingAssetsResolver(BundleManifest manifest, IEnumerable<string> activeVariants = null, string baseFolder = null) : base(manifest, activeVariants)
		{
			this._baseFolder = (baseFolder ?? "AssetBundles");
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00070FA4 File Offset: 0x0006F3A4
		private static string BasePath
		{
			get
			{
				if (StreamingAssetsResolver._basePath == null)
				{
					string streamingAssetsPath = Application.streamingAssetsPath;
					StreamingAssetsResolver._basePath = ((!streamingAssetsPath.Contains("://")) ? ("file://" + streamingAssetsPath + '/') : (streamingAssetsPath + '/'));
				}
				return StreamingAssetsResolver._basePath;
			}
		}

		// eli key point 载入assetbundle相关？
		public static IEnumerator<StreamingAssetsResolver> CreateResolver(IEnumerable<string> activeVariants = null, string baseFolder = null)
		{
			return ManifestLoader.LoadManifest(StreamingAssetsResolver.GetUrl(baseFolder ?? "AssetBundles", "AppManifest.json"), null, null).Catch(delegate(Exception e)
			{
				Log.DebugFormatted("Could not load bundled manifest: {0}", new object[]
				{
					e
				});
				return new BundleManifest();
			}).ContinueWith((BundleManifest manifest) => new StreamingAssetsResolver(manifest, activeVariants, baseFolder));
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00071078 File Offset: 0x0006F478
		public override string GetUrlForAsset(IBundleInfo bundleInfo, bool cached)
		{
			string url = bundleInfo.Url;
			return StreamingAssetsResolver.GetUrl(this._baseFolder, url);
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00071098 File Offset: 0x0006F498
		public override string[] GetUrlsForAsset(IBundleInfo bundleInfo, bool cached)
		{
			return new string[]
			{
				this.GetUrlForAsset(bundleInfo, cached)
			};
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000710AB File Offset: 0x0006F4AB
		private static string GetUrl(string baseFolder, string path)
		{
			return string.Concat(new object[]
			{
				StreamingAssetsResolver.BasePath,
				baseFolder,
				'/',
				path
			});
		}

		// Token: 0x040047E6 RID: 18406
		public const string ManifestName = "AppManifest.json";

		// Token: 0x040047E7 RID: 18407
		private readonly string _baseFolder;

		// Token: 0x040047E8 RID: 18408
		private static string _basePath;
	}
}
