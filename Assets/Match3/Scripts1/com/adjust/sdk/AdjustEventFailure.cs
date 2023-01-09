using System;
using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000018 RID: 24
	public class AdjustEventFailure
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00006097 File Offset: 0x00004497
		public AdjustEventFailure()
		{
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000060A0 File Offset: 0x000044A0
		public AdjustEventFailure(Dictionary<string, string> eventFailureDataMap)
		{
			if (eventFailureDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyEventToken);
			this.WillRetry = bool.Parse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyWillRetry));
			bool willRetry;
			if (bool.TryParse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
			{
				this.WillRetry = willRetry;
			}
			string aJSON = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyJsonResponse);
			JSONNode jsonnode = JSON.Parse(aJSON);
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006180 File Offset: 0x00004580
		public AdjustEventFailure(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyEventToken);
			this.WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyWillRetry));
			JSONNode jsonnode2 = jsonnode[AdjustUtils.KeyJsonResponse];
			if (jsonnode2 == null)
			{
				return;
			}
			if (jsonnode2.AsObject == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode2.AsObject, this.JsonResponse);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00006248 File Offset: 0x00004648
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00006250 File Offset: 0x00004650
		public string Adid { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00006259 File Offset: 0x00004659
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00006261 File Offset: 0x00004661
		public string Message { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000626A File Offset: 0x0000466A
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00006272 File Offset: 0x00004672
		public string Timestamp { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000627B File Offset: 0x0000467B
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00006283 File Offset: 0x00004683
		public string EventToken { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000628C File Offset: 0x0000468C
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00006294 File Offset: 0x00004694
		public bool WillRetry { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000629D File Offset: 0x0000469D
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000062A5 File Offset: 0x000046A5
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x060000FE RID: 254 RVA: 0x000062B0 File Offset: 0x000046B0
		public void BuildJsonResponseFromString(string jsonResponseString)
		{
			JSONNode jsonnode = JSON.Parse(jsonResponseString);
			if (jsonnode == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000062ED File Offset: 0x000046ED
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
