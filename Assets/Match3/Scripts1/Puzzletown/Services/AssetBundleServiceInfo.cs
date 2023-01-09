using Match3.Scripts1.Wooga.Services.AssetBundleManager;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000748 RID: 1864
	public class AssetBundleServiceInfo
	{
		// Token: 0x06002E35 RID: 11829 RVA: 0x000D7930 File Offset: 0x000D5D30
		public AssetBundleServiceInfo(IAssetBundleManager bundleManager)
		{
			this.bundleManager = bundleManager;
		}

		// Token: 0x0400578F RID: 22415
		private readonly IAssetBundleManager bundleManager;
	}
}
