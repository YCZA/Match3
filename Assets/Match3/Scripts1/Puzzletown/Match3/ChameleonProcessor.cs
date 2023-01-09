namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000607 RID: 1543
	public class ChameleonProcessor : AMatchProcessor
	{
		// Token: 0x0600277F RID: 10111 RVA: 0x000AF795 File Offset: 0x000ADB95
		public ChameleonProcessor(GemFactory gemFactory)
		{
			this.gemFactory = gemFactory;
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000AF7B0 File Offset: 0x000ADBB0
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.matchColor = match.Group.Color;
			this.fields = fields;
			this.chameleonGems.Clear();
			foreach (Gem gem2 in match.Group)
			{
				if (gem2.IsChameleon && fields[gem2.position].CanExplode)
				{
					Gem chameleonGem = gem2;
					ChameleonVariant chameleonVariant = (!chameleonGem.IsAllColorChameleon) ? ChameleonVariant.Reduced : ChameleonVariant.All;
					ChameleonModel chameleonModel = fields.GetChameleonModel(chameleonVariant);
					bool flag = chameleonModel.ContainsColor(this.matchColor) || this.matchColor == GemColor.Undefined;
					if (flag)
					{
						if (chameleonModel.IsLastColor(chameleonGem.color))
						{
							this.CollectChameleon(chameleonGem);
						}
						else
						{
							this.chameleonGems.Add(gem2);
							GemColor nextColor = chameleonModel.GetNextColor(chameleonGem.color);
							GemColor nextColor2 = chameleonModel.GetNextColor(nextColor);
							this.ChangeChameleonColor(chameleonGem, nextColor, nextColor2);
						}
					}
				}
			}
			match.Group.RemoveAll((Gem gem) => this.chameleonGems.Contains(gem));
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000AF8FC File Offset: 0x000ADCFC
		private void CollectChameleon(Gem chameleonGem)
		{
			this.fields[chameleonGem.position].isExploded = true;
			this.results.Add(new ChameleonMatched(chameleonGem, GemColor.Coins, GemColor.Undefined, true));
			this.gemFactory.StopSpawningChameleonGems();
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000AF93C File Offset: 0x000ADD3C
		private void ChangeChameleonColor(Gem chameleonGem, GemColor nextColor, GemColor foreshadowingColor)
		{
			chameleonGem.color = nextColor;
			chameleonGem.processed = true;
			this.fields[chameleonGem.position].AssignGem(chameleonGem);
			this.results.Add(new ChameleonMatched(chameleonGem, nextColor, foreshadowingColor, false));
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000AF98A File Offset: 0x000ADD8A
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000AF98C File Offset: 0x000ADD8C
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
		}

		// Token: 0x040051F4 RID: 20980
		private GemColor matchColor;

		// Token: 0x040051F5 RID: 20981
		private Fields fields;

		// Token: 0x040051F6 RID: 20982
		private readonly Group chameleonGems = new Group();

		// Token: 0x040051F7 RID: 20983
		private readonly GemFactory gemFactory;
	}
}
