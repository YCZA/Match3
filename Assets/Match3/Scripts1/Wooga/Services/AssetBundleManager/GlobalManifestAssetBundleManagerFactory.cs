using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000308 RID: 776
	[Obsolete("Please use AssetBundleManagerBuilder")]
	public static class GlobalManifestAssetBundleManagerFactory
	{
		// Token: 0x06001854 RID: 6228 RVA: 0x0006F714 File Offset: 0x0006DB14
		public static IEnumerator<IAssetBundleManager> SetupManager(IEnumerable<string> activeVariants = null)
		{
			return new AssetBundleManagerBuilder().WithManifests(new string[]
			{
				"MainManifest.json"
			}).WithActiveVariants(activeVariants ?? Enumerable.Empty<string>()).WithBundleManagerConfig(null).Build();
		}
	}
}
