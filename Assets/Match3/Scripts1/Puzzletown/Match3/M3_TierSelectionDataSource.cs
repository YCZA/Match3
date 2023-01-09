using System;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200072D RID: 1837
	public class M3_TierSelectionDataSource : ArrayDataSource<M3LevelSelectionItemTierView, M3LevelSelectionItemTier>
	{
		// Token: 0x06002D8F RID: 11663 RVA: 0x000D41C4 File Offset: 0x000D25C4
		public override int GetReusableIdForIndex(int index)
		{
			M3LevelSelectionItemTierView.State result = M3LevelSelectionItemTierView.State.Completed;
			if (index == this.selectedTier)
			{
				if (this.progressionService.IsCompleted(this.selectedLevel))
				{
					result = M3LevelSelectionItemTierView.State.CompletedCurrent;
				}
				else
				{
					result = M3LevelSelectionItemTierView.State.Current;
				}
			}
			else if (index > this.selectedTier)
			{
				result = M3LevelSelectionItemTierView.State.Pending;
			}
			return (int)result;
		}

		// Token: 0x04005727 RID: 22311
		public ProgressionDataService.Service progressionService;

		// Token: 0x04005728 RID: 22312
		[NonSerialized]
		public int selectedTier;

		// Token: 0x04005729 RID: 22313
		[NonSerialized]
		public int selectedLevel;
	}
}
