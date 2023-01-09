using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200031B RID: 795
	public interface IBundleInfo
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060018DA RID: 6362
		string Name { get; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060018DB RID: 6363
		string Url { get; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060018DC RID: 6364
		Hash128 Hash128 { get; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060018DD RID: 6365
		uint CRC { get; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060018DE RID: 6366
		uint Size { get; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060018DF RID: 6367
		string Variant { get; }
	}
}
