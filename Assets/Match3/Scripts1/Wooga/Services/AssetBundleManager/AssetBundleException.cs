using System;
using System.Text;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x020002F8 RID: 760
	public class AssetBundleException : Exception
	{
		// Token: 0x060017D3 RID: 6099 RVA: 0x0006D0F3 File Offset: 0x0006B4F3
		public AssetBundleException(string assetBundle, string assetPath = null, string userInfo = null) : base(AssetBundleException.FormatMessage(assetBundle, assetPath, userInfo))
		{
			this.AssetBundle = assetBundle;
			this.AssetPath = assetPath;
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x0006D111 File Offset: 0x0006B511
		// (set) Token: 0x060017D5 RID: 6101 RVA: 0x0006D119 File Offset: 0x0006B519
		public string AssetBundle { get; private set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0006D122 File Offset: 0x0006B522
		// (set) Token: 0x060017D7 RID: 6103 RVA: 0x0006D12A File Offset: 0x0006B52A
		public string AssetPath { get; private set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0006D133 File Offset: 0x0006B533
		// (set) Token: 0x060017D9 RID: 6105 RVA: 0x0006D13B File Offset: 0x0006B53B
		public string UserInfo { get; private set; }

		// Token: 0x060017DA RID: 6106 RVA: 0x0006D144 File Offset: 0x0006B544
		private static string FormatMessage(string assetBundle, string assetPath = null, string userInfo = null)
		{
			StringBuilder stringBuilder = new StringBuilder("bundle:" + assetBundle);
			if (assetPath != null)
			{
				stringBuilder.Append(", path:" + assetPath);
			}
			if (userInfo != null)
			{
				stringBuilder.Append(", userInfo:" + userInfo);
			}
			return stringBuilder.ToString();
		}
	}
}
