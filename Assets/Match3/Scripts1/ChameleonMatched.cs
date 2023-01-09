using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000608 RID: 1544
	public struct ChameleonMatched : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x06002786 RID: 10118 RVA: 0x000AF99C File Offset: 0x000ADD9C
		public ChameleonMatched(Gem gem, GemColor nextColor, GemColor foreshadowingColor, bool countForObjective)
		{
			this.gem = gem;
			this.nextColor = nextColor;
			this.foreshadowingColor = foreshadowingColor;
			this.countForObjective = countForObjective;
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000AF9BB File Offset: 0x000ADDBB
		public Gem OriginGem
		{
			get
			{
				return this.gem;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000AF9C3 File Offset: 0x000ADDC3
		public GemColor NextColor
		{
			get
			{
				return this.nextColor;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002789 RID: 10121 RVA: 0x000AF9CB File Offset: 0x000ADDCB
		public GemColor ForeshadowingColor
		{
			get
			{
				return this.foreshadowingColor;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x000AF9D3 File Offset: 0x000ADDD3
		public string Type
		{
			get
			{
				return "chameleon";
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000AF9DA File Offset: 0x000ADDDA
		public bool CountForObjective
		{
			get
			{
				return this.countForObjective;
			}
		}

		// Token: 0x040051F8 RID: 20984
		private readonly Gem gem;

		// Token: 0x040051F9 RID: 20985
		private readonly GemColor nextColor;

		// Token: 0x040051FA RID: 20986
		private readonly GemColor foreshadowingColor;

		// Token: 0x040051FB RID: 20987
		private readonly bool countForObjective;
	}
}
