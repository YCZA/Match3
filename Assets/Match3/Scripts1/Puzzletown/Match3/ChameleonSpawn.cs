using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005CC RID: 1484
	public class ChameleonSpawn : IFinalMovement, IMatchResult
	{
		// Token: 0x06002697 RID: 9879 RVA: 0x000AD327 File Offset: 0x000AB727
		public ChameleonSpawn(IntVector2 position, IntVector2 facingDirection, Gem gem)
		{
			gem.processed = true;
			this.facingDirection = facingDirection;
			this.spawn = new SpawnResult(position, gem);
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002698 RID: 9880 RVA: 0x000AD34B File Offset: 0x000AB74B
		public IntVector2 Position
		{
			get
			{
				return this.spawn.position;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x000AD358 File Offset: 0x000AB758
		public SpawnResult Spawn
		{
			get
			{
				return this.spawn;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600269A RID: 9882 RVA: 0x000AD360 File Offset: 0x000AB760
		// (set) Token: 0x0600269B RID: 9883 RVA: 0x000AD36D File Offset: 0x000AB76D
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

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x0600269C RID: 9884 RVA: 0x000AD37B File Offset: 0x000AB77B
		public IntVector2 FacingDirection
		{
			get
			{
				return this.facingDirection;
			}
		}

		// Token: 0x0400516D RID: 20845
		private SpawnResult spawn;

		// Token: 0x0400516E RID: 20846
		private readonly IntVector2 facingDirection;
	}
}
