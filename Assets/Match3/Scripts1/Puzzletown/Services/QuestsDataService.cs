using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x02000935 RID: 2357
	public class QuestsDataService : ADataService
	{
		// Token: 0x0600394A RID: 14666 RVA: 0x0011A317 File Offset: 0x00118717
		public QuestsDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x0011A320 File Offset: 0x00118720
		public QuestProgressCollection Quests
		{
			get
			{
				return base.state.quests;
			}
		}
	}
}
