using System;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000310 RID: 784
	[Serializable]
	public class LastManifestInfo
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0006FAEE File Offset: 0x0006DEEE
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x0006FAF6 File Offset: 0x0006DEF6
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x0006FAFF File Offset: 0x0006DEFF
		// (set) Token: 0x06001888 RID: 6280 RVA: 0x0006FB07 File Offset: 0x0006DF07
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x0006FB10 File Offset: 0x0006DF10
		// (set) Token: 0x0600188A RID: 6282 RVA: 0x0006FB18 File Offset: 0x0006DF18
		public string MD5
		{
			get
			{
				return this.md5;
			}
			set
			{
				this.md5 = value;
			}
		}

		// Token: 0x040047BC RID: 18364
		[SerializeField]
		private string version;

		// Token: 0x040047BD RID: 18365
		[SerializeField]
		private string url;

		// Token: 0x040047BE RID: 18366
		[SerializeField]
		private string md5;
	}
}
