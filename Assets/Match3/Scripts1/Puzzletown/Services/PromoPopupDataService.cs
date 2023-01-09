using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007B6 RID: 1974
	public class PromoPopupDataService : ADataService
	{
		// Token: 0x0600309F RID: 12447 RVA: 0x000E4135 File Offset: 0x000E2535
		public PromoPopupDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000E413E File Offset: 0x000E253E
		public void SetSeenToday()
		{
			base.state.promoPopupData.lastSeen = DateTime.Today.ToUnixTimeStamp();
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000E415C File Offset: 0x000E255C
		public bool SeenToday()
		{
			int num = DateTime.Today.ToUnixTimeStamp();
			return base.state.promoPopupData.lastSeen < num;
		}
	}
}
