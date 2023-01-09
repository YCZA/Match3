using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000614 RID: 1556
	public struct GrowingWindowExplosion : IMatchResult
	{
		// Token: 0x060027C1 RID: 10177 RVA: 0x000B0AA0 File Offset: 0x000AEEA0
		public GrowingWindowExplosion(IntVector2 position, IntVector2 belowPosition, IntVector2 createdFrom)
		{
			this.position = position;
			this.belowPosition = belowPosition;
			this.createdFrom = createdFrom;
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000B0AB7 File Offset: 0x000AEEB7
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000B0ABF File Offset: 0x000AEEBF
		public IntVector2 BelowPosition
		{
			get
			{
				return this.belowPosition;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x000B0AC7 File Offset: 0x000AEEC7
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x04005216 RID: 21014
		private readonly IntVector2 position;

		// Token: 0x04005217 RID: 21015
		private readonly IntVector2 belowPosition;

		// Token: 0x04005218 RID: 21016
		private readonly IntVector2 createdFrom;
	}
}
