namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000311 RID: 785
	public class FallbackInfo
	{
		// Token: 0x0600188B RID: 6283 RVA: 0x0006FB21 File Offset: 0x0006DF21
		public FallbackInfo(string clientVersion, LastManifestInfo lastManifestInfo)
		{
			this.ClientVersion = clientVersion;
			this.LastManifestInfo = lastManifestInfo;
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x0600188C RID: 6284 RVA: 0x0006FB37 File Offset: 0x0006DF37
		public bool IsFallbackAllowed
		{
			get
			{
				return this.ClientVersion == this.LastManifestInfo.Version;
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0006FB4F File Offset: 0x0006DF4F
		public void Update(string url, string md5Hash)
		{
			this.LastManifestInfo.Url = url;
			this.LastManifestInfo.MD5 = md5Hash;
			this.LastManifestInfo.Version = this.ClientVersion;
		}

		// Token: 0x040047BF RID: 18367
		public readonly string ClientVersion;

		// Token: 0x040047C0 RID: 18368
		public readonly LastManifestInfo LastManifestInfo;
	}
}
