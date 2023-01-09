namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000645 RID: 1605
	public class RainbowSuperGemProcessor : AScoreProcessor<RainbowSuperGemExplosion>
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x000B5E0F File Offset: 0x000B420F
		public RainbowSuperGemProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000B5E18 File Offset: 0x000B4218
		protected override void DoProcess(RainbowSuperGemExplosion input)
		{
			if (!this.score.success && input.superGem.IsLineGem())
			{
				this.score.AddAmount(new MaterialAmount("linegem_rainbow", 1, MaterialAmountUsage.Undefined, 0));
			}
			else if (!this.score.success && input.superGem.IsAnyBombGem())
			{
				this.score.AddAmount(new MaterialAmount("bomb_rainbow", 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
