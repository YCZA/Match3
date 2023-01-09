using System;
using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x0200001C RID: 28
	public class AdjustSessionFailure
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000065D1 File Offset: 0x000049D1
		public AdjustSessionFailure()
		{
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000065DC File Offset: 0x000049DC
		public AdjustSessionFailure(Dictionary<string, string> sessionFailureDataMap)
		{
			if (sessionFailureDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyTimestamp);
			this.WillRetry = bool.Parse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyWillRetry));
			bool willRetry;
			if (bool.TryParse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
			{
				this.WillRetry = willRetry;
			}
			string aJSON = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyJsonResponse);
			JSONNode jsonnode = JSON.Parse(aJSON);
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000066AC File Offset: 0x00004AAC
		public AdjustSessionFailure(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00006763 File Offset: 0x00004B63
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000676B File Offset: 0x00004B6B
		public string Adid { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006774 File Offset: 0x00004B74
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000677C File Offset: 0x00004B7C
		public string Message { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006785 File Offset: 0x00004B85
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000678D File Offset: 0x00004B8D
		public string Timestamp { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006796 File Offset: 0x00004B96
		// (set) Token: 0x0600011B RID: 283 RVA: 0x0000679E File Offset: 0x00004B9E
		public bool WillRetry { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000067A7 File Offset: 0x00004BA7
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000067AF File Offset: 0x00004BAF
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x0600011E RID: 286 RVA: 0x000067B8 File Offset: 0x00004BB8
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

		// Token: 0x0600011F RID: 287 RVA: 0x000067F5 File Offset: 0x00004BF5
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
