using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D0 RID: 1488
	public struct ClimberSpawn : IFinalMovement, IMatchResult
	{
		// Token: 0x060026AB RID: 9899 RVA: 0x000AD44B File Offset: 0x000AB84B
		public ClimberSpawn(IntVector2 position, Gem gem, bool fromPortal = false)
		{
			this.spawn = new SpawnResult(position, gem);
			this.fromPortal = fromPortal;
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x000AD461 File Offset: 0x000AB861
		public IntVector2 Position
		{
			get
			{
				return this.spawn.position;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x000AD46E File Offset: 0x000AB86E
		// (set) Token: 0x060026AE RID: 9902 RVA: 0x000AD47B File Offset: 0x000AB87B
		public bool IsFinal
		{
			get
			{
				return this.spawn.IsFinal;
			}
			set
			{
				this.spawn.IsFinal = value;
			}
		}

		// Token: 0x04005174 RID: 20852
		public SpawnResult spawn;

		// Token: 0x04005175 RID: 20853
		public bool fromPortal;
	}
}
