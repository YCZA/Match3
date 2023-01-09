namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000625 RID: 1573
	public class TileProcessor : AMatchProcessor
	{
		// Token: 0x06002809 RID: 10249 RVA: 0x000B1E94 File Offset: 0x000B0294
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			foreach (Gem gem in match.Group)
			{
				this.CheckSurroundings(gem.position, fields);
			}
			if (match is IMatchWithAffectedFields)
			{
				IMatchWithAffectedFields matchWithAffectedFields = (IMatchWithAffectedFields)match;
				foreach (IntVector2 pos in matchWithAffectedFields.Fields)
				{
					this.CheckSurroundings(pos, fields);
				}
			}
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000B1F58 File Offset: 0x000B0358
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
			this.CheckField(fields[pos], pos);
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000B1F68 File Offset: 0x000B0368
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (field.numTiles > 0 && field.numTiles <= 2 && !field.removedModifier)
			{
				field.numTiles--;
				field.removedModifier = true;
				this.results.Add(new TileExplosion(field));
			}
		}
	}
}
