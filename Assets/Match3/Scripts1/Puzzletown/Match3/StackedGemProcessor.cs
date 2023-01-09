namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000620 RID: 1568
	public class StackedGemProcessor : AMatchProcessor
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x000B199C File Offset: 0x000AFD9C
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			foreach (Gem gem in match.Group)
			{
				if (gem.CanExplosionHit && gem.color != GemColor.Rainbow && fields[gem.position].CanExplosionHitGem() && gem.IsStackedGem)
				{
					int amount = Match3ConfigAmounts.typeValueMap[gem.type];
					fields[gem.position].isExploded = true;
					this.ExplodeStackGem(gem, amount, fields);
				}
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000B1A5C File Offset: 0x000AFE5C
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000B1A5E File Offset: 0x000AFE5E
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
		}

		// key point 堆叠宝石爆炸
		private void ExplodeStackGem(Gem gem, int amount, Fields fields)
		{
			Group group = StackedGemProcessorHelper.ReplaceGems(gem, amount, fields);
			this.results.Add(new StackedGemExplosion(gem, group));
		}
	}
}
