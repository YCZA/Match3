using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005FE RID: 1534
	public struct CannonExplosionTrigger : IProcessableMatch, IMatchResult
	{
		// Token: 0x06002756 RID: 10070 RVA: 0x000AE9F7 File Offset: 0x000ACDF7
		public CannonExplosionTrigger(IntVector2 cannonPosition)
		{
			this.cannonPosition = cannonPosition;
			this.IsProcessed = false;
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x000AEA07 File Offset: 0x000ACE07
		// (set) Token: 0x06002758 RID: 10072 RVA: 0x000AEA0F File Offset: 0x000ACE0F
		public bool IsProcessed { get; set; }

		// Token: 0x040051DF RID: 20959
		public IntVector2 cannonPosition;
	}
}
