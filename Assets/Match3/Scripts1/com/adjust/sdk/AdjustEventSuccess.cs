using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000019 RID: 25
	public class AdjustEventSuccess
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000062FA File Offset: 0x000046FA
		public AdjustEventSuccess()
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006304 File Offset: 0x00004704
		public AdjustEventSuccess(Dictionary<string, string> eventSuccessDataMap)
		{
			if (eventSuccessDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyEventToken);
			string aJSON = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyJsonResponse);
			JSONNode jsonnode = JSON.Parse(aJSON);
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x000063B0 File Offset: 0x000047B0
		public AdjustEventSuccess(string jsonString)
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00006462 File Offset: 0x00004862
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000646A File Offset: 0x0000486A
		public string Adid { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006473 File Offset: 0x00004873
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000647B File Offset: 0x0000487B
		public string Message { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00006484 File Offset: 0x00004884
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000648C File Offset: 0x0000488C
		public string Timestamp { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00006495 File Offset: 0x00004895
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000649D File Offset: 0x0000489D
		public string EventToken { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000064A6 File Offset: 0x000048A6
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000064AE File Offset: 0x000048AE
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x0600010D RID: 269 RVA: 0x000064B8 File Offset: 0x000048B8
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

		// Token: 0x0600010E RID: 270 RVA: 0x000064F5 File Offset: 0x000048F5
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
