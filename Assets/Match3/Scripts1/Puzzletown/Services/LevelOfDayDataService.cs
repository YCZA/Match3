using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007CE RID: 1998
	public class LevelOfDayDataService : ADataService
	{
		// Token: 0x0600312E RID: 12590 RVA: 0x000E7388 File Offset: 0x000E5788
		public LevelOfDayDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000E7391 File Offset: 0x000E5791
		public bool TryGetSavedLevelOfDayModel(out LevelOfDayModel model)
		{
			model = null;
			if (base.state != null)
			{
				model = base.state.levelOfDayData;
			}
			return model != null;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000E73B6 File Offset: 0x000E57B6
		public void SaveLevelOfDayModel(LevelOfDayModel model)
		{
			if (base.state != null)
			{
				base.state.levelOfDayData = model;
			}
		}
	}
}
