namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200061E RID: 1566
	public class ResistantBlockerProcessor : AMatchProcessor
	{
		// Token: 0x060027E8 RID: 10216 RVA: 0x000B1814 File Offset: 0x000AFC14
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (!field.removedModifier && field.CanExplosionHitGem() && field.IsResistantBlocker)
			{
				if (!this.canDestroyHighestHp && field.blockerIndex == 8)
				{
					return;
				}
				bool countForObjective = false;
				if (field.blockerIndex == 6)
				{
					field.blockerIndex = 0;
					countForObjective = true;
				}
				else
				{
					field.blockerIndex--;
				}
				this.results.Add(new ResistantBlockerExplosion(field, createdFrom, countForObjective));
				field.removedModifier = true;
			}
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000B18A4 File Offset: 0x000AFCA4
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.canDestroyHighestHp = ((match is IExplosionResult && !(match is RainbowExplosion)) || match is HammerMatch);
			base.ProcessMatch(match, fields);
			IMatchWithAffectedFields matchWithAffectedFields = match as IMatchWithAffectedFields;
			if (matchWithAffectedFields != null)
			{
				foreach (IntVector2 intVector in matchWithAffectedFields.Fields)
				{
					this.CheckField(fields[intVector], intVector);
				}
			}
		}

		// Token: 0x04005230 RID: 21040
		private bool canDestroyHighestHp;
	}
}
