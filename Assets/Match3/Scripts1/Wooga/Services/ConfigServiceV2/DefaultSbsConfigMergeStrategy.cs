using System.Collections.Generic;
using Wooga.Newtonsoft.Json;
using Wooga.Newtonsoft.Json.Linq;

namespace Match3.Scripts1.Wooga.Services.ConfigServiceV2
{
	// Token: 0x0200032F RID: 815
	public class DefaultSbsConfigMergeStrategy : ISbsConfigMergeStrategy
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x00072A88 File Offset: 0x00070E88
		public string Merge(string currentJson, string fetchedJson)
		{
			if (fetchedJson == "{}")
			{
				return currentJson;
			}
			JObject jobject = JObject.Parse(currentJson);
			JObject jobject2 = JObject.Parse(fetchedJson);
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject2)
			{
				jobject[keyValuePair.Key] = keyValuePair.Value;
			}
			return jobject.ToString(Formatting.None, new JsonConverter[0]);
		}
	}
}
