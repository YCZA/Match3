using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x0200061F RID: 1567
	public struct ResistantBlockerExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x060027EA RID: 10218 RVA: 0x000B1944 File Offset: 0x000AFD44
		public ResistantBlockerExplosion(Field field, IntVector2 createdFrom, bool countForObjective)
		{
			this.position = field.gridPosition;
			this.newAmount = field.blockerIndex;
			this.createdFrom = createdFrom;
			this.countForObjective = countForObjective;
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x000B196C File Offset: 0x000AFD6C
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x000B1974 File Offset: 0x000AFD74
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060027ED RID: 10221 RVA: 0x000B197C File Offset: 0x000AFD7C
		public string Type
		{
			get
			{
				return "resistant_blocker";
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x000B1983 File Offset: 0x000AFD83
		public int NewAmount
		{
			get
			{
				return this.newAmount;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060027EF RID: 10223 RVA: 0x000B198B File Offset: 0x000AFD8B
		public bool CountForObjective
		{
			get
			{
				return this.countForObjective;
			}
		}

		// Token: 0x04005231 RID: 21041
		private readonly IntVector2 position;

		// Token: 0x04005232 RID: 21042
		private readonly IntVector2 createdFrom;

		// Token: 0x04005233 RID: 21043
		private readonly int newAmount;

		// Token: 0x04005234 RID: 21044
		private readonly bool countForObjective;
	}
}
