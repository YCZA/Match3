namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000646 RID: 1606
	public class SuperBombProcessor : AScoreProcessor<BombExplosion>
	{
		// Token: 0x060028BE RID: 10430 RVA: 0x000B5E9C File Offset: 0x000B429C
		public SuperBombProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000B5EA8 File Offset: 0x000B42A8
		protected override void DoProcess(BombExplosion input)
		{
			if (!this.score.success && input.isSuperBomb && input.gem.IsActivatedBombGem())
			{
				this.score.AddAmount(new MaterialAmount("bomb_bomb", 1, MaterialAmountUsage.Undefined, 0));
			}
		}
	}
}
