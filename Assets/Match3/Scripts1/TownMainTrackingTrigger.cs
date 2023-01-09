using System.Collections;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x0200081D RID: 2077
namespace Match3.Scripts1
{
	public class TownMainTrackingTrigger : PopupManager.Trigger
	{
		// Token: 0x06003378 RID: 13176 RVA: 0x000F4534 File Offset: 0x000F2934
		public TownMainTrackingTrigger(ProgressionDataService.Service progression, TrackingService tracking)
		{
			this.progression = progression;
			this.tracking = tracking;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000F454C File Offset: 0x000F294C
		public override bool ShouldTrigger()
		{
			if (this.progression.LastUnlockedArea != this.progression.Data.EndOfContentReachedAtArea && this.progression.HasCompletedAllTiers())
			{
				this.progression.Data.EndOfContentReachedAtArea = this.progression.LastUnlockedArea;
				this.tracking.TrackEvent(new object[]
				{
					"all_m3_completed",
					this.progression.LastUnlockedArea,
					this.progression.UnlockedLevel - 1
				});
			}
			return false;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000F45E8 File Offset: 0x000F29E8
		public override IEnumerator Run()
		{
			yield break;
		}

		// Token: 0x04005B92 RID: 23442
		private ProgressionDataService.Service progression;

		// Token: 0x04005B93 RID: 23443
		private TrackingService tracking;
	}
}
