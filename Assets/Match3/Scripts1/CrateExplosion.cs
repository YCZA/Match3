using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x0200060E RID: 1550
	public struct CrateExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x060027A7 RID: 10151 RVA: 0x000B04C5 File Offset: 0x000AE8C5
		public CrateExplosion(Field field, IntVector2 createdFrom)
		{
			this.position = field.gridPosition;
			this.newAmount = field.cratesIndex;
			this.createdFrom = createdFrom;
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000B04E6 File Offset: 0x000AE8E6
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000B04EE File Offset: 0x000AE8EE
		public string Type
		{
			get
			{
				return "crates";
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x000B04F5 File Offset: 0x000AE8F5
		public int NewAmount
		{
			get
			{
				return this.newAmount;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000B04FD File Offset: 0x000AE8FD
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x000B0505 File Offset: 0x000AE905
		public bool CountForObjective
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400520B RID: 21003
		private readonly IntVector2 position;

		// Token: 0x0400520C RID: 21004
		private readonly IntVector2 createdFrom;

		// Token: 0x0400520D RID: 21005
		private readonly int newAmount;
	}
}
