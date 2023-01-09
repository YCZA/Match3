namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x0200031C RID: 796
	public interface IManifestInfo
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060018E0 RID: 6368
		string MD5 { get; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060018E1 RID: 6369
		string URL { get; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060018E2 RID: 6370
		string OriginBaseURL { get; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060018E3 RID: 6371
		string[] CDNBaseURLs { get; }
	}
}
