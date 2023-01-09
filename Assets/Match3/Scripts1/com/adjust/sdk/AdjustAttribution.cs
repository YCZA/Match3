using System.Collections.Generic;

namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000013 RID: 19
	public class AdjustAttribution
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00005C0F File Offset: 0x0000400F
		public AdjustAttribution()
		{
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005C18 File Offset: 0x00004018
		public AdjustAttribution(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.trackerName = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerToken);
			this.network = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyNetwork);
			this.campaign = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdgroup);
			this.creative = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyClickLabel);
			this.adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005CC8 File Offset: 0x000040C8
		public AdjustAttribution(Dictionary<string, string> dicAttributionData)
		{
			if (dicAttributionData == null)
			{
				return;
			}
			this.trackerName = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
			this.network = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
			this.campaign = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
			this.creative = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
			this.adid = AdjustAttribution.TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005D6A File Offset: 0x0000416A
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00005D72 File Offset: 0x00004172
		public string adid { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005D7B File Offset: 0x0000417B
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005D83 File Offset: 0x00004183
		public string network { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005D8C File Offset: 0x0000418C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00005D94 File Offset: 0x00004194
		public string adgroup { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005D9D File Offset: 0x0000419D
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005DA5 File Offset: 0x000041A5
		public string campaign { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005DAE File Offset: 0x000041AE
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00005DB6 File Offset: 0x000041B6
		public string creative { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005DBF File Offset: 0x000041BF
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005DC7 File Offset: 0x000041C7
		public string clickLabel { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005DD0 File Offset: 0x000041D0
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00005DD8 File Offset: 0x000041D8
		public string trackerName { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005DE1 File Offset: 0x000041E1
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00005DE9 File Offset: 0x000041E9
		public string trackerToken { get; set; }

		// Token: 0x060000CD RID: 205 RVA: 0x00005DF4 File Offset: 0x000041F4
		private static string TryGetValue(Dictionary<string, string> dic, string key)
		{
			string result;
			if (dic.TryGetValue(key, out result))
			{
				return result;
			}
			return null;
		}
	}
}
