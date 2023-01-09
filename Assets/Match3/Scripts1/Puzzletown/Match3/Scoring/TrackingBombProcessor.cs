namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063C RID: 1596
	public class TrackingBombProcessor : AScoreProcessor<BombExplosion>
	{
		// Token: 0x060028AA RID: 10410 RVA: 0x000B5B77 File Offset: 0x000B3F77
		public TrackingBombProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000B5B80 File Offset: 0x000B3F80
		protected override void DoProcess(BombExplosion result)
		{
			if (!this.score.success && result.gem.type == GemType.Undefined)
			{
				this.score.bombsActivated++;
			}
		}
	}
}
