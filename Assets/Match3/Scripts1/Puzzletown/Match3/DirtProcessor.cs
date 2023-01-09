namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000612 RID: 1554
	public class DirtProcessor : AMatchProcessor
	{
		// Token: 0x060027BC RID: 10172 RVA: 0x000B07EC File Offset: 0x000AEBEC
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			base.ProcessMatch(match, fields);
			if (match is IMatchWithAffectedFields)
			{
				foreach (IntVector2 intVector in ((IMatchWithAffectedFields)match).Fields)
				{
					this.CheckField(fields[intVector], intVector);
				}
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000B0868 File Offset: 0x000AEC68
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (field.gem.IsCoveredByDirt && !field.removedModifier)
			{
				bool flag = field.gem.modifier == GemModifier.DirtHp1;
				int newHp = 0;
				if (flag)
				{
					field.gem.modifier = GemModifier.Undefined;
				}
				else
				{
					newHp = ((field.gem.modifier != GemModifier.DirtHp3) ? 1 : 2);
					field.gem.modifier = field.gem.modifier - 1;
				}
				this.results.Add(new DirtExplosion(field.gridPosition, field.gem, newHp, createdFrom));
				if (flag)
				{
					if (field.gem.color == GemColor.Treasure)
					{
						this.results.Add(new TreasureCollected(field.gridPosition, field.gem, createdFrom));
					}
					field.HasGem = false;
				}
				field.removedModifier = true;
				field.isExploded = true;
			}
		}
	}
}
