using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000606 RID: 1542
	public struct ChameleonTurn : IMatchResult
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000AF775 File Offset: 0x000ADB75
		public ChameleonTurn(GemDirection facingDirection, IntVector2 origin)
		{
			this.facingDirection = facingDirection;
			this.origin = origin;
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000AF785 File Offset: 0x000ADB85
		public GemDirection FacingDirection
		{
			get
			{
				return this.facingDirection;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000AF78D File Offset: 0x000ADB8D
		public IntVector2 Origin
		{
			get
			{
				return this.origin;
			}
		}

		// Token: 0x040051F2 RID: 20978
		private readonly GemDirection facingDirection;

		// Token: 0x040051F3 RID: 20979
		private readonly IntVector2 origin;
	}
}
