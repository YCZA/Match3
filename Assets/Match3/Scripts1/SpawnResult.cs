using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F6 RID: 1526
	public struct SpawnResult : IFinalMovement, IMatchResult
	{
		// Token: 0x06002736 RID: 10038 RVA: 0x000AE3FD File Offset: 0x000AC7FD
		public SpawnResult(IntVector2 position, Gem gem)
		{
			this.position = position;
			this.direction = IntVector2.Down;
			this.gem = gem;
			this.isFinal = true;
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x000AE41F File Offset: 0x000AC81F
		// (set) Token: 0x06002738 RID: 10040 RVA: 0x000AE427 File Offset: 0x000AC827
		public bool IsFinal
		{
			get
			{
				return this.isFinal;
			}
			set
			{
				this.isFinal = value;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x000AE430 File Offset: 0x000AC830
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000AE438 File Offset: 0x000AC838
		public override string ToString()
		{
			return string.Format("[SpawnResult: gem={0}]", this.gem);
		}

		// Token: 0x040051CE RID: 20942
		public IntVector2 position;

		// Token: 0x040051CF RID: 20943
		public IntVector2 direction;

		// Token: 0x040051D0 RID: 20944
		public Gem gem;

		// Token: 0x040051D1 RID: 20945
		private bool isFinal;
	}
}
