namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200031D RID: 797
	public class ManifestInfo : IManifestInfo
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060018E5 RID: 6373 RVA: 0x00070EAF File Offset: 0x0006F2AF
		// (set) Token: 0x060018E6 RID: 6374 RVA: 0x00070EB7 File Offset: 0x0006F2B7
		public string MD5 { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060018E7 RID: 6375 RVA: 0x00070EC0 File Offset: 0x0006F2C0
		// (set) Token: 0x060018E8 RID: 6376 RVA: 0x00070EC8 File Offset: 0x0006F2C8
		public string URL { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x00070ED1 File Offset: 0x0006F2D1
		// (set) Token: 0x060018EA RID: 6378 RVA: 0x00070ED9 File Offset: 0x0006F2D9
		public string OriginBaseURL { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00070EE2 File Offset: 0x0006F2E2
		// (set) Token: 0x060018EC RID: 6380 RVA: 0x00070EEA File Offset: 0x0006F2EA
		public string[] CDNBaseURLs { get; set; }
	}
}
