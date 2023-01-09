using System.Collections.Generic;
using System.Text;

namespace Match3.Scripts1.Wooga.Core.Data
{
	// Token: 0x02000349 RID: 841
	public class SbsMetaData
	{
		// Token: 0x060019B5 RID: 6581 RVA: 0x000743EA File Offset: 0x000727EA
		public SbsMetaData(int formatVersion = -1)
		{
			this.Data = new Dictionary<string, string>();
			this.FormatVersion = formatVersion;
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x00074404 File Offset: 0x00072804
		// (set) Token: 0x060019B7 RID: 6583 RVA: 0x0007440C File Offset: 0x0007280C
		public Dictionary<string, string> Data { get; set; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x00074415 File Offset: 0x00072815
		// (set) Token: 0x060019B9 RID: 6585 RVA: 0x0007441D File Offset: 0x0007281D
		public int FormatVersion { get; set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060019BA RID: 6586 RVA: 0x00074426 File Offset: 0x00072826
		// (set) Token: 0x060019BB RID: 6587 RVA: 0x0007444F File Offset: 0x0007284F
		public string Signature
		{
			get
			{
				if (this.Data.ContainsKey("signature"))
				{
					return this.Data["signature"];
				}
				return null;
			}
			set
			{
				this.Data["signature"] = value;
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00074464 File Offset: 0x00072864
		public string Serialize()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{0}\n", "WDK_META_DATA_V1");
			stringBuilder.AppendFormat("{0}\n", this.Data.Count + 1);
			foreach (KeyValuePair<string, string> keyValuePair in this.Data)
			{
				stringBuilder.AppendFormat("{0}:{1}\n", keyValuePair.Key, keyValuePair.Value);
			}
			stringBuilder.AppendFormat("{0}:{1}\n", "formatVersion", this.FormatVersion);
			return stringBuilder.ToString();
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0007452C File Offset: 0x0007292C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> keyValuePair in this.Data)
			{
				stringBuilder.AppendFormat("{0}:{1}\n", keyValuePair.Key, keyValuePair.Value);
			}
			stringBuilder.AppendFormat("{0}:{1}\n", "formatVersion", this.FormatVersion);
			return stringBuilder.ToString();
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000745C4 File Offset: 0x000729C4
		public static SbsMetaData TryCreateFromString(string data)
		{
			SbsMetaData sbsMetaData = null;
			string[] array = data.Split(new char[]
			{
				'\n'
			});
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				string text = array[i];
				string[] array2 = text.Split(new char[]
				{
					':'
				});
				if (array2.Length == 2)
				{
					if (sbsMetaData == null)
					{
						sbsMetaData = new SbsMetaData(-1);
					}
					if (array2[0] == "formatVersion")
					{
						int formatVersion = -1;
						int.TryParse(array2[1], out formatVersion);
						sbsMetaData.FormatVersion = formatVersion;
					}
					else
					{
						sbsMetaData.Data.Add(array2[0], array2[1]);
					}
				}
				i++;
			}
			return sbsMetaData;
		}

		// Token: 0x04004840 RID: 18496
		public const string META_DATA_FILE_EXTENSION = ".sbsmeta";

		// Token: 0x04004841 RID: 18497
		public const string KEY_FORMAT_VERSION = "formatVersion";

		// Token: 0x04004842 RID: 18498
		public const string KEY_SIGNATURE = "signature";

		// Token: 0x04004843 RID: 18499
		public const string WDK_META_DATA_TAG = "WDK_META_DATA_V1";
	}
}
