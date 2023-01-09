using System;
using System.Collections;
using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x0200001E RID: 30
	public class AdjustUtils
	{
		// Token: 0x0600012E RID: 302 RVA: 0x000069E2 File Offset: 0x00004DE2
		public static int ConvertLogLevel(AdjustLogLevel? logLevel)
		{
			if (logLevel == null)
			{
				return -1;
			}
			return (int)logLevel.Value;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000069FC File Offset: 0x00004DFC
		public static int ConvertBool(bool? value)
		{
			if (value == null)
			{
				return -1;
			}
			if (value.Value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006A1E File Offset: 0x00004E1E
		public static double ConvertDouble(double? value)
		{
			if (value == null)
			{
				return -1.0;
			}
			return value.Value;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006A40 File Offset: 0x00004E40
		public static long ConvertLong(long? value)
		{
			if (value == null)
			{
				return -1L;
			}
			return value.Value;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006A5C File Offset: 0x00004E5C
		public static string ConvertListToJson(List<string> list)
		{
			if (list == null)
			{
				return null;
			}
			JSONArray jsonarray = new JSONArray();
			foreach (string aData in list)
			{
				jsonarray.Add(new JSONData(aData));
			}
			return jsonarray.ToString();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006ACC File Offset: 0x00004ECC
		public static string GetJsonResponseCompact(Dictionary<string, object> dictionary)
		{
			string text = string.Empty;
			if (dictionary == null)
			{
				return text;
			}
			int num = 0;
			text += "{";
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				string text2 = keyValuePair.Value as string;
				if (text2 != null)
				{
					if (++num > 1)
					{
						text += ",";
					}
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"\"",
						keyValuePair.Key,
						"\":\"",
						text2,
						"\""
					});
				}
				else
				{
					Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
					if (++num > 1)
					{
						text += ",";
					}
					text = text + "\"" + keyValuePair.Key + "\":";
					text += AdjustUtils.GetJsonResponseCompact(dictionary2);
				}
			}
			text += "}";
			return text;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006BFC File Offset: 0x00004FFC
		public static string GetJsonString(JSONNode node, string key)
		{
			if (node == null)
			{
				return null;
			}
			JSONData jsondata = node[key] as JSONData;
			if (jsondata == null)
			{
				return null;
			}
			return jsondata.Value;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006C38 File Offset: 0x00005038
		public static void WriteJsonResponseDictionary(JSONClass jsonObject, Dictionary<string, object> output)
		{
			IEnumerator enumerator = jsonObject.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					KeyValuePair<string, JSONNode> keyValuePair = (KeyValuePair<string, JSONNode>)obj;
					JSONClass asObject = keyValuePair.Value.AsObject;
					string key = keyValuePair.Key;
					if (asObject == null)
					{
						string value = keyValuePair.Value.Value;
						output.Add(key, value);
					}
					else
					{
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						output.Add(key, dictionary);
						AdjustUtils.WriteJsonResponseDictionary(asObject, dictionary);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006CE8 File Offset: 0x000050E8
		public static string TryGetValue(Dictionary<string, string> d, string key)
		{
			string result;
			if (d.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04000078 RID: 120
		public static string KeyAdid = "adid";

		// Token: 0x04000079 RID: 121
		public static string KeyMessage = "message";

		// Token: 0x0400007A RID: 122
		public static string KeyNetwork = "network";

		// Token: 0x0400007B RID: 123
		public static string KeyAdgroup = "adgroup";

		// Token: 0x0400007C RID: 124
		public static string KeyCampaign = "campaign";

		// Token: 0x0400007D RID: 125
		public static string KeyCreative = "creative";

		// Token: 0x0400007E RID: 126
		public static string KeyWillRetry = "willRetry";

		// Token: 0x0400007F RID: 127
		public static string KeyTimestamp = "timestamp";

		// Token: 0x04000080 RID: 128
		public static string KeyEventToken = "eventToken";

		// Token: 0x04000081 RID: 129
		public static string KeyClickLabel = "clickLabel";

		// Token: 0x04000082 RID: 130
		public static string KeyTrackerName = "trackerName";

		// Token: 0x04000083 RID: 131
		public static string KeyTrackerToken = "trackerToken";

		// Token: 0x04000084 RID: 132
		public static string KeyJsonResponse = "jsonResponse";
	}
}
