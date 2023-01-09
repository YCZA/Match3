using System;
using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007F1 RID: 2033
	[Serializable]
	public class SaleData : WeeklyEventData
	{
		// Token: 0x06003259 RID: 12889 RVA: 0x000ECCFA File Offset: 0x000EB0FA
		public override void SetActiveConfig(EventConfigContainer eventConfig)
		{
			this.currentConfigName = eventConfig.config.sale.configName;
			base.SetActiveConfig(eventConfig);
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x000ECD19 File Offset: 0x000EB119
		public override void UpdateConfig(EventConfigContainer eventConfig)
		{
			this.currentConfigName = eventConfig.config.sale.configName;
			base.UpdateConfig(eventConfig);
		}

		// Token: 0x04005ACE RID: 23246
		public string lastBoughtSaleId;

		// Token: 0x04005ACF RID: 23247
		public int lastSeen;

		// Token: 0x04005AD0 RID: 23248
		public string currentConfigName;
	}
}
