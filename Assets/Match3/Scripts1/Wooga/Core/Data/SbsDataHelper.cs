using System.Collections.Generic;
using Wooga.Core.Utilities;
using Wooga.Newtonsoft.Json;
using Wooga.Foundation.Json;
using Wooga.Foundation.Json.Bridge;

namespace Match3.Scripts1.Wooga.Core.Data
{
	// Token: 0x02000348 RID: 840
	public static class SbsDataHelper
	{
		// Token: 0x060019B3 RID: 6579 RVA: 0x00074318 File Offset: 0x00072718
		public static ISbsData TryCreateFromJson(string json)
		{
			ISbsData sbsData = new SbsRichData(null, -1);
			try
			{
				Dictionary<string, JSONNode> dictionary = JSON.Deserialize<Dictionary<string, JSONNode>>(json);
				if (dictionary != null)
				{
					JSONNode jsonnode;
					if (dictionary.TryGetValue(SbsDataHelper.FORMAT_VERSION, out jsonnode) && jsonnode.IsInt())
					{
						sbsData.FormatVersion = (int)jsonnode;
					}
					JSONNode jsonnode2;
					if (dictionary.TryGetValue(SbsDataHelper.DATA, out jsonnode2) && !jsonnode2.IsNull())
					{
						sbsData.Data = ((SToken)jsonnode2).ToString(Formatting.None, new JsonConverter[0]);
					}
				}
			}
			catch (JsonException ex)
			{
				Log.Error(new object[]
				{
					"Couldn't deserialize json : " + ex.Message
				});
			}
			return sbsData;
		}

		// Token: 0x0400483D RID: 18493
		public static readonly string FORMAT_VERSION = "format_version";

		// Token: 0x0400483E RID: 18494
		public static readonly string DATA = "data";

		// Token: 0x0400483F RID: 18495
		public const int FORMAT_VERSION_NOT_SET = -1;
	}
}
