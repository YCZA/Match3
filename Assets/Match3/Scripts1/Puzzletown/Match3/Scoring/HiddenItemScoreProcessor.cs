namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x02000641 RID: 1601
	public class HiddenItemScoreProcessor : AScoreProcessor<HiddenItemFound>
	{
		// Token: 0x060028B4 RID: 10420 RVA: 0x000B5D43 File Offset: 0x000B4143
		public HiddenItemScoreProcessor(Match3Score score) : base(score)
		{
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000B5D4C File Offset: 0x000B414C
		protected override void DoProcess(HiddenItemFound input)
		{
			this.score.AddAmount(new MaterialAmount(this.score.Config.data.hiddenItemName, 1, MaterialAmountUsage.Undefined, 0));
		}
	}
}
