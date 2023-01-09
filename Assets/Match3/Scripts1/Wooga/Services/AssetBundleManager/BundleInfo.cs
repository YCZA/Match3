using System;
using Wooga.Newtonsoft.Json;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000316 RID: 790
	[JsonObject]
	[Serializable]
	public class BundleInfo : IBundleInfo
	{
		// Token: 0x060018AD RID: 6317 RVA: 0x0007052B File Offset: 0x0006E92B
		public BundleInfo(string name, string hash, string url, uint crc = 0U, uint size = 0U, string variant = null, string[] dependencies = null)
		{
			this.n = name;
			this.h = hash;
			this.u = url;
			this.c = crc;
			this.s = size;
			this.v = variant;
			this.d = dependencies;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00070568 File Offset: 0x0006E968
		protected BundleInfo()
		{
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x00070570 File Offset: 0x0006E970
		[JsonIgnore]
		public string Name
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x00070578 File Offset: 0x0006E978
		[JsonIgnore]
		public string Url
		{
			get
			{
				return this.u;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x00070580 File Offset: 0x0006E980
		[JsonIgnore]
		public string QualifiedName
		{
			get
			{
				string result;
				if ((result = this._qualidiedName) == null)
				{
					result = (this._qualidiedName = ((!this.HasVariant) ? this.n : (this.n + "." + this.v)));
				}
				return result;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x000705CF File Offset: 0x0006E9CF
		[JsonIgnore]
		public uint Size
		{
			get
			{
				return this.s;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x000705D7 File Offset: 0x0006E9D7
		[JsonIgnore]
		public string Hash
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x000705E0 File Offset: 0x0006E9E0
		[JsonIgnore]
		public Hash128 Hash128
		{
			get
			{
				return (!this._hash128.isValid) ? (this._hash128 = Hash128.Parse(this.h)) : this._hash128;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0007061C File Offset: 0x0006EA1C
		[JsonIgnore]
		public uint CRC
		{
			get
			{
				return this.c;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00070624 File Offset: 0x0006EA24
		[JsonIgnore]
		public string Variant
		{
			get
			{
				return this.v;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0007062C File Offset: 0x0006EA2C
		[JsonIgnore]
		public bool HasVariant
		{
			get
			{
				return !string.IsNullOrEmpty(this.v);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x0007063C File Offset: 0x0006EA3C
		// (set) Token: 0x060018B9 RID: 6329 RVA: 0x00070665 File Offset: 0x0006EA65
		[JsonIgnore]
		public string[] Dependencies
		{
			get
			{
				string[] result;
				if ((result = this.d) == null)
				{
					result = (this.d = new string[0]);
				}
				return result;
			}
			set
			{
				this.d = value;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x0007066E File Offset: 0x0006EA6E
		[JsonIgnore]
		public bool HasDependecies
		{
			get
			{
				return this.d != null && this.d.Length > 0;
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00070689 File Offset: 0x0006EA89
		public static bool operator ==(BundleInfo info1, BundleInfo info2)
		{
			if (object.ReferenceEquals(info1, null))
			{
				return object.ReferenceEquals(info2, null);
			}
			return info1.Equals(info2);
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000706A6 File Offset: 0x0006EAA6
		public static bool operator !=(BundleInfo info1, BundleInfo info2)
		{
			return !(info1 == info2);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000706B4 File Offset: 0x0006EAB4
		public override bool Equals(object other)
		{
			BundleInfo bundleInfo = other as BundleInfo;
			return !object.ReferenceEquals(bundleInfo, null) && this.QualifiedName == bundleInfo.QualifiedName && this.Hash == bundleInfo.Hash;
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x000706FE File Offset: 0x0006EAFE
		public override int GetHashCode()
		{
			return this.QualifiedName.GetHashCode();
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0007070C File Offset: 0x0006EB0C
		public override string ToString()
		{
			return string.Format("[BundleInfo: Name={0}, Variant={1}, Size={2}, Hash={3}, Hash128={4}, CRC={5} ]", new object[]
			{
				this.Name,
				this.Variant,
				this.Size,
				this.Hash,
				this.Hash128,
				this.CRC
			});
		}

		// Token: 0x040047CD RID: 18381
		[SerializeField]
		[JsonProperty]
		protected string n;

		// Token: 0x040047CE RID: 18382
		[SerializeField]
		[JsonProperty]
		protected string u;

		// Token: 0x040047CF RID: 18383
		private string _qualidiedName;

		// Token: 0x040047D0 RID: 18384
		[SerializeField]
		[JsonProperty]
		protected uint s;

		// Token: 0x040047D1 RID: 18385
		[SerializeField]
		[JsonProperty]
		protected string h;

		// Token: 0x040047D2 RID: 18386
		private Hash128 _hash128;

		// Token: 0x040047D3 RID: 18387
		[SerializeField]
		[JsonProperty]
		protected uint c;

		// Token: 0x040047D4 RID: 18388
		[SerializeField]
		[JsonProperty]
		protected string v;

		// Token: 0x040047D5 RID: 18389
		[SerializeField]
		[JsonProperty]
		protected string[] d;
	}
}
