using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wooga.Newtonsoft.Json;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000317 RID: 791
	[JsonObject]
	[Serializable]
	public class BundleManifest : IEnumerable<BundleInfo>, IEnumerable
	{
		// Token: 0x060018C1 RID: 6337 RVA: 0x00070776 File Offset: 0x0006EB76
		public static BundleManifest Parse(string json)
		{
			return JsonUtility.FromJson<BundleManifest>(json);
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00070780 File Offset: 0x0006EB80
		public static BundleManifest CreateWith(List<BundleInfo> infos)
		{
			return new BundleManifest
			{
				i = (infos ?? new List<BundleInfo>())
			};
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x000707A8 File Offset: 0x0006EBA8
		public static BundleManifest CreateWith(params BundleInfo[] infos)
		{
			return new BundleManifest
			{
				i = ((infos == null) ? new List<BundleInfo>() : infos.ToList<BundleInfo>())
			};
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x000707D8 File Offset: 0x0006EBD8
		[JsonIgnore]
		public List<BundleInfo> BundleInfos
		{
			get
			{
				List<BundleInfo> result;
				if ((result = this.i) == null)
				{
					result = (this.i = new List<BundleInfo>());
				}
				return result;
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00070800 File Offset: 0x0006EC00
		public IEnumerator<BundleInfo> GetEnumerator()
		{
			return ((IEnumerable<BundleInfo>)this.BundleInfos).GetEnumerator();
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0007080D File Offset: 0x0006EC0D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040047D6 RID: 18390
		[SerializeField]
		[JsonProperty]
		private List<BundleInfo> i;
	}
}
