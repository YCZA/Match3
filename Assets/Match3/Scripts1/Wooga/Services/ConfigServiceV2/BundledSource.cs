using System;
using System.Text;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200032E RID: 814
	public class BundledSource : IReadableConfigSource
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x00072980 File Offset: 0x00070D80
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x00072988 File Offset: 0x00070D88
		public string Hash { get; private set; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x00072991 File Offset: 0x00070D91
		public string AbTests
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001946 RID: 6470 RVA: 0x00072994 File Offset: 0x00070D94
		public string PersonalizationString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00072998 File Offset: 0x00070D98
		public PersistedConfigData Read()
		{
			TextAsset[] array = Resources.LoadAll<TextAsset>("SbsConfigService");
			if (array.Length == 0)
			{
				throw new Exception("Bundled configs not found - Please include default configs at Resources/SbsConfigService");
			}
			StringBuilder stringBuilder = new StringBuilder();
			string hash = null;
			stringBuilder.Append("{");
			for (int i = 0; i < array.Length; i++)
			{
				TextAsset textAsset = array[i];
				if (textAsset.name == "content_hash")
				{
					hash = textAsset.text;
				}
				else
				{
					stringBuilder.Append("\"");
					stringBuilder.Append(textAsset.name);
					stringBuilder.Append("\":");
					stringBuilder.Append(textAsset.text);
					if (i + 1 < array.Length)
					{
						stringBuilder.Append(",");
					}
				}
			}
			stringBuilder.Append("}");
			this.Hash = hash;
			return new PersistedConfigData(stringBuilder.ToString(), hash, null, null);
		}

		// Token: 0x04004803 RID: 18435
		public const string CONFIG_PATH = "SbsConfigService";

		// Token: 0x04004804 RID: 18436
		public const string HASH_FILE_NAME = "content_hash";
	}
}
