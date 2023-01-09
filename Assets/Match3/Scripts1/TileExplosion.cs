using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000626 RID: 1574
	public struct TileExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x0600280C RID: 10252 RVA: 0x000B1FC3 File Offset: 0x000B03C3
		public TileExplosion(Field field)
		{
			this.position = field.gridPosition;
			this.newAmount = field.numTiles;
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x000B1FDD File Offset: 0x000B03DD
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x000B1FE5 File Offset: 0x000B03E5
		public int NewAmount
		{
			get
			{
				return this.newAmount;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x000B1FED File Offset: 0x000B03ED
		public string Type
		{
			get
			{
				return "tiles";
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000B1FF4 File Offset: 0x000B03F4
		public bool CountForObjective
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400523A RID: 21050
		private readonly IntVector2 position;

		// Token: 0x0400523B RID: 21051
		private readonly int newAmount;
	}
}
