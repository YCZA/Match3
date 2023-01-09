using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x0200001D RID: 29
	public class AdjustSessionSuccess
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00006802 File Offset: 0x00004C02
		public AdjustSessionSuccess()
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000680C File Offset: 0x00004C0C
		public AdjustSessionSuccess(Dictionary<string, string> sessionSuccessDataMap)
		{
			if (sessionSuccessDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyTimestamp);
			string aJSON = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyJsonResponse);
			JSONNode jsonnode = JSON.Parse(aJSON);
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000068A8 File Offset: 0x00004CA8
		public AdjustSessionSuccess(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006949 File Offset: 0x00004D49
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00006951 File Offset: 0x00004D51
		public string Adid { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000125 RID: 293 RVA: 0x0000695A File Offset: 0x00004D5A
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00006962 File Offset: 0x00004D62
		public string Message { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000696B File Offset: 0x00004D6B
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00006973 File Offset: 0x00004D73
		public string Timestamp { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000697C File Offset: 0x00004D7C
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00006984 File Offset: 0x00004D84
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x0600012B RID: 299 RVA: 0x00006990 File Offset: 0x00004D90
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

		// Token: 0x0600012C RID: 300 RVA: 0x000069CD File Offset: 0x00004DCD
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
