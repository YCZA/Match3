

// Token: 0x02000585 RID: 1413
namespace Match3.Scripts1
{
	public abstract class AField
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002515 RID: 9493 RVA: 0x000A515E File Offset: 0x000A355E
		public virtual bool NeedsGem
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002516 RID: 9494 RVA: 0x000A5161 File Offset: 0x000A3561
		public virtual bool IsBlocked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002517 RID: 9495 RVA: 0x000A5164 File Offset: 0x000A3564
		public virtual bool GemBlocked
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002518 RID: 9496 RVA: 0x000A5167 File Offset: 0x000A3567
		public virtual bool CanMove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000A516A File Offset: 0x000A356A
		public virtual bool CanSwap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x000A516D File Offset: 0x000A356D
		public virtual bool CanBeCharged
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x000A5170 File Offset: 0x000A3570
		// (set) Token: 0x0600251C RID: 9500 RVA: 0x000A5184 File Offset: 0x000A3584
		public bool HasGem
		{
			get
			{
				return this.gem.color != GemColor.Undefined;
			}
			set
			{
				if (!value)
				{
					this.AssignGem(default(Gem));
				}
			}
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000A51A6 File Offset: 0x000A35A6
		public void AssignGem(Gem gem)
		{
			this.gem = gem;
			this.gem.position = this.gridPosition;
		}

		// Token: 0x04005080 RID: 20608
		public bool isOn;

		// Token: 0x04005081 RID: 20609
		public Gem gem;

		// Token: 0x04005082 RID: 20610
		public IntVector2 gridPosition;
	}
}
