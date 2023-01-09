using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.AssetBundleManager;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000749 RID: 1865
	[Serializable]
	public class BundleManifestList
	{
		// Token: 0x04005790 RID: 22416
		public List<BundleManifestList.Entry> all = new List<BundleManifestList.Entry>();

		// Token: 0x0200074A RID: 1866
		[Serializable]
		public class Entry
		{
			// Token: 0x04005791 RID: 22417
			public string name;

			// Token: 0x04005792 RID: 22418
			public BundleManifest manifest;
		}
	}
}
