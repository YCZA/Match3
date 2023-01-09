using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000624 RID: 1572
	public struct StoneExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x06002802 RID: 10242 RVA: 0x000B1E48 File Offset: 0x000B0248
		public StoneExplosion(Field field, IntVector2 createdFrom)
		{
			this.position = field.gridPosition;
			this.newAmount = field.blockerIndex;
			this.createdFrom = createdFrom;
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000B1E69 File Offset: 0x000B0269
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x000B1E71 File Offset: 0x000B0271
		public int NewAmount
		{
			get
			{
				return this.newAmount;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002805 RID: 10245 RVA: 0x000B1E79 File Offset: 0x000B0279
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x000B1E81 File Offset: 0x000B0281
		public string Type
		{
			get
			{
				return "stones";
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x000B1E88 File Offset: 0x000B0288
		public bool CountForObjective
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04005237 RID: 21047
		private readonly IntVector2 position;

		// Token: 0x04005238 RID: 21048
		private readonly IntVector2 createdFrom;

		// Token: 0x04005239 RID: 21049
		private readonly int newAmount;
	}
}
