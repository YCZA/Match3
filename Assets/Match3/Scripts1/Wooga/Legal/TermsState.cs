using System;
using Match3.Scripts1.Wooga.Core.Data;
using Wooga.Newtonsoft.Json;
using Wooga.Foundation.Json;

namespace Match3.Scripts1.Wooga.Legal
{
	// Token: 0x02000B5A RID: 2906
	public class TermsState
	{
		// Token: 0x060043F6 RID: 17398 RVA: 0x0015AAB5 File Offset: 0x00158EB5
		public TermsState(long acceptedVersion = 0L)
		{
			this.accepted_version = acceptedVersion;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0015AAC4 File Offset: 0x00158EC4
		public static TermsState FromJsonOrDefault(string json)
		{
			TermsState termsState = null;
			if (!string.IsNullOrEmpty(json))
			{
				try
				{
					termsState = JSON.Deserialize<TermsState>(json);
				}
				catch (Exception ex)
				{
					WoogaDebug.LogWarning(new object[]
					{
						ex
					});
				}
			}
			if (termsState == null)
			{
				termsState = new TermsState(0L);
			}
			return termsState;
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0015AB20 File Offset: 0x00158F20
		public string ToJson()
		{
			return JSON.Serialize(this, false, 1, ' ');
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0015AB2C File Offset: 0x00158F2C
		public static ISbsData MergeHandler(ISbsData myData, ISbsData serverData)
		{
			return myData;
		}

		// Token: 0x04006C4F RID: 27727
		public const string BUCKET_NAME = "z_consent";

		// Token: 0x04006C50 RID: 27728
		public const int FORMAT_VERSION = 1;

		// Token: 0x04006C51 RID: 27729
		public const int SBS_FETCH_TIMEOUT = 5;

		// Token: 0x04006C52 RID: 27730
		[JsonProperty("consent")]
		public long accepted_version;
	}
}
