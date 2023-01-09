using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007D6 RID: 2006
	public class LivesDataService : ADataService
	{
		// Token: 0x06003178 RID: 12664 RVA: 0x000E8ADA File Offset: 0x000E6EDA
		public LivesDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000E8AE3 File Offset: 0x000E6EE3
		// (set) Token: 0x0600317A RID: 12666 RVA: 0x000E8AFB File Offset: 0x000E6EFB
		public DateTime LastLifeGiven
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(base.state.lives.lastLifeGiven, DateTimeKind.Utc);
			}
			set
			{
				base.state.lives.lastLifeGiven = value.ToUnixTimeStamp();
			}
		}
	}
}
