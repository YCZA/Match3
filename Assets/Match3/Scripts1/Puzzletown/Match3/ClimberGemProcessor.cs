namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000609 RID: 1545
	public class ClimberGemProcessor : AMatchProcessor
	{
		// Token: 0x0600278D RID: 10125 RVA: 0x000AF9EC File Offset: 0x000ADDEC
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			foreach (Gem originGem in match.Group)
			{
				if (originGem.type == GemType.ClimberGem && fields[originGem.position].CanExplode)
				{
					IntVector2 climberPosition = ClimberProcessor.GetClimberPosition(fields);
					if (climberPosition != Fields.invalidPos)
					{
						this.results.Add(new ClimberGemCollected(originGem, climberPosition));
					}
				}
			}
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000AFA94 File Offset: 0x000ADE94
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000AFA96 File Offset: 0x000ADE96
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
		}
	}
}
