namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005FA RID: 1530
	public class CannonballProcessor : AMatchProcessor
	{
		// Token: 0x06002749 RID: 10057 RVA: 0x000AE604 File Offset: 0x000ACA04
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			for (int i = 0; i < match.Group.Count; i++)
			{
				Gem gem = match.Group[i];
				if (this.CheckAndRemoveGem(gem, fields[gem.position], gem.position))
				{
					match.Group.RemoveAt(i--);
				}
				if (gem.IsMatchable)
				{
					base.ProcessMatch(match, fields);
				}
			}
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000AE67E File Offset: 0x000ACA7E
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			this.CheckAndRemoveGem(field.gem, field, createdFrom);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000AE690 File Offset: 0x000ACA90
		private bool CheckAndRemoveGem(Gem gem, Field field, IntVector2 createdFrom)
		{
			bool result = false;
			if (gem.CanExplosionHit && field.CanExplosionHitGem() && gem.color == GemColor.Cannonball)
			{
				this.results.Add(new CannonballExplosion(gem.position, createdFrom));
				if (field.gem.color == GemColor.Cannonball)
				{
					field.HasGem = false;
				}
				result = true;
			}
			return result;
		}
	}
}
