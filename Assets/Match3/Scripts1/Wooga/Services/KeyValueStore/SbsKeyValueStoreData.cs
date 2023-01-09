using System.Text.RegularExpressions;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Services.KeyValueStore
{
	// Token: 0x02000415 RID: 1045
	public class SbsKeyValueStoreData : ISbsData
	{
		// Token: 0x06001EDA RID: 7898 RVA: 0x00082964 File Offset: 0x00080D64
		public SbsKeyValueStoreData()
		{
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0008296C File Offset: 0x00080D6C
		public SbsKeyValueStoreData(string json)
		{
			MatchCollection matchCollection = SbsKeyValueStoreData.parseJsonRegex.Matches(json);
			if (matchCollection.Count == 1 && matchCollection[0].Groups.Count == 3)
			{
				this.Data = matchCollection[0].Groups[1].Value;
				int.TryParse(matchCollection[0].Groups[2].Value, out this._formatVersion);
			}
			else
			{
				matchCollection = SbsKeyValueStoreData.parseJsonReverseRegex.Matches(json);
				if (matchCollection.Count == 1 && matchCollection[0].Groups.Count == 3)
				{
					this.Data = matchCollection[0].Groups[2].Value;
					int.TryParse(matchCollection[0].Groups[1].Value, out this._formatVersion);
				}
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00082A62 File Offset: 0x00080E62
		public SbsKeyValueStoreData(int format_version, string data)
		{
			this.FormatVersion = format_version;
			this.Data = data;
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001EDD RID: 7901 RVA: 0x00082A78 File Offset: 0x00080E78
		// (set) Token: 0x06001EDE RID: 7902 RVA: 0x00082A80 File Offset: 0x00080E80
		public string Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x00082A89 File Offset: 0x00080E89
		// (set) Token: 0x06001EE0 RID: 7904 RVA: 0x00082A91 File Offset: 0x00080E91
		public int FormatVersion
		{
			get
			{
				return this._formatVersion;
			}
			set
			{
				this._formatVersion = value;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x00082A9A File Offset: 0x00080E9A
		public JSONNode Dictionary
		{
			get
			{
				return JSON.Deserialize(this.Data);
			}
		}

		// Token: 0x04004A93 RID: 19091
		private static Regex parseJsonRegex = new Regex("\\s*{\\s*\"data\"\\s*:(?<data>.*),\\s*\"format_version\"\\s*:\\s*(?<format_version>\\d*)\\s*}\\s*");

		// Token: 0x04004A94 RID: 19092
		private static Regex parseJsonReverseRegex = new Regex("\\s*{\\s*\"format_version\"\\s*:(?<format_version>\\d*),\\s*\"data\"\\s*:\\s*(?<data>.*)\\s*}\\s*");

		// Token: 0x04004A95 RID: 19093
		private string _data;

		// Token: 0x04004A96 RID: 19094
		private int _formatVersion;
	}
}
