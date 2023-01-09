using System;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200079E RID: 1950
	public class ADataService
	{
		// Token: 0x06002FCE RID: 12238 RVA: 0x000D8902 File Offset: 0x000D6D02
		public ADataService(Func<GameState> i_getState)
		{
			this.getState = i_getState;
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x000D8911 File Offset: 0x000D6D11
		protected GameState state
		{
			get
			{
				return this.getState();
			}
		}

		// Token: 0x040058EF RID: 22767
		private Func<GameState> getState;
	}
}
