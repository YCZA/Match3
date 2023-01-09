using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000635 RID: 1589
	public struct GrowingWindowSpawn : IMatchResult
	{
		// Token: 0x06002863 RID: 10339 RVA: 0x000B4D2D File Offset: 0x000B312D
		public GrowingWindowSpawn(IntVector2 targetPosition, IntVector2 abovePosition, IntVector2 belowPosition, IntVector2 leftPosition, IntVector2 rightPosition, Gem destroyedGem)
		{
			this.destroyedGem = destroyedGem;
			this.targetPosition = targetPosition;
			this.abovePosition = abovePosition;
			this.belowPosition = belowPosition;
			this.leftPosition = leftPosition;
			this.rightPosition = rightPosition;
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002864 RID: 10340 RVA: 0x000B4D5C File Offset: 0x000B315C
		public IntVector2 Target
		{
			get
			{
				return this.targetPosition;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x000B4D64 File Offset: 0x000B3164
		public IntVector2 Above
		{
			get
			{
				return this.abovePosition;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x000B4D6C File Offset: 0x000B316C
		public IntVector2 Below
		{
			get
			{
				return this.belowPosition;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002867 RID: 10343 RVA: 0x000B4D74 File Offset: 0x000B3174
		public IntVector2 Left
		{
			get
			{
				return this.leftPosition;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002868 RID: 10344 RVA: 0x000B4D7C File Offset: 0x000B317C
		public IntVector2 Right
		{
			get
			{
				return this.rightPosition;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002869 RID: 10345 RVA: 0x000B4D84 File Offset: 0x000B3184
		public Gem DestroyedGem
		{
			get
			{
				return this.destroyedGem;
			}
		}

		// Token: 0x04005261 RID: 21089
		private readonly IntVector2 targetPosition;

		// Token: 0x04005262 RID: 21090
		private readonly IntVector2 abovePosition;

		// Token: 0x04005263 RID: 21091
		private readonly IntVector2 belowPosition;

		// Token: 0x04005264 RID: 21092
		private readonly IntVector2 leftPosition;

		// Token: 0x04005265 RID: 21093
		private readonly IntVector2 rightPosition;

		// Token: 0x04005266 RID: 21094
		private readonly Gem destroyedGem;
	}
}
