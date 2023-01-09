using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000602 RID: 1538
	public struct ChainExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x0600276B RID: 10091 RVA: 0x000AEF78 File Offset: 0x000AD378
		public ChainExplosion(Field field)
		{
			this.position = field.gridPosition;
			this.newAmount = field.numChains;
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600276C RID: 10092 RVA: 0x000AEF92 File Offset: 0x000AD392
		public string Type
		{
			get
			{
				return "chains";
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600276D RID: 10093 RVA: 0x000AEF99 File Offset: 0x000AD399
		public int NewAmount
		{
			get
			{
				return this.newAmount;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x000AEFA1 File Offset: 0x000AD3A1
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600276F RID: 10095 RVA: 0x000AEFA9 File Offset: 0x000AD3A9
		public bool CountForObjective
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040051E9 RID: 20969
		private readonly IntVector2 position;

		// Token: 0x040051EA RID: 20970
		private readonly int newAmount;
	}
}
