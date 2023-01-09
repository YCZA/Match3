using System.Text;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000302 RID: 770
	public class AssetBundleName
	{
		// Token: 0x06001837 RID: 6199 RVA: 0x0006F0DF File Offset: 0x0006D4DF
		public AssetBundleName(string baseName, string variant)
		{
			this.BaseName = baseName;
			this.Variant = AssetBundleName.Nullify(variant);
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x0006F0FA File Offset: 0x0006D4FA
		// (set) Token: 0x06001839 RID: 6201 RVA: 0x0006F102 File Offset: 0x0006D502
		public string BaseName { get; private set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x0006F10C File Offset: 0x0006D50C
		public string QualifiedName
		{
			get
			{
				if (this._qualifiedName == null)
				{
					StringBuilder stringBuilder = new StringBuilder(this.BaseName);
					if (this.Variant != null)
					{
						stringBuilder.Append('.').Append(this.Variant);
					}
					this._qualifiedName = stringBuilder.ToString();
				}
				return this._qualifiedName;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x0006F161 File Offset: 0x0006D561
		// (set) Token: 0x0600183C RID: 6204 RVA: 0x0006F169 File Offset: 0x0006D569
		public string Variant { get; private set; }

		// Token: 0x0600183D RID: 6205 RVA: 0x0006F172 File Offset: 0x0006D572
		public override bool Equals(object obj)
		{
			return this.Equals(obj as AssetBundleName);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0006F180 File Offset: 0x0006D580
		public bool Equals(AssetBundleName other)
		{
			return other != null && this.QualifiedName == other.QualifiedName;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0006F19C File Offset: 0x0006D59C
		public override int GetHashCode()
		{
			return this.QualifiedName.GetHashCode();
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0006F1A9 File Offset: 0x0006D5A9
		public override string ToString()
		{
			return string.Format("[AssetBundleName: BaseName={0}, Variant={1}]", this.BaseName, this.Variant ?? "<none>");
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0006F1CD File Offset: 0x0006D5CD
		private static string Nullify(string str)
		{
			return (!string.IsNullOrEmpty(str)) ? str : null;
		}

		// Token: 0x040047B1 RID: 18353
		private string _qualifiedName;
	}
}
