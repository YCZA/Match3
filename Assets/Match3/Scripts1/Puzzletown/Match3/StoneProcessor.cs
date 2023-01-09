namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000623 RID: 1571
	public class StoneProcessor : AMatchProcessor
	{
		// Token: 0x06002800 RID: 10240 RVA: 0x000B1D78 File Offset: 0x000B0178
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (!field.removedModifier && Stone.IsStone(field.blockerIndex))
			{
				field.blockerIndex--;
				this.results.Add(new StoneExplosion(field, createdFrom));
				field.removedModifier = true;
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000B1DCC File Offset: 0x000B01CC
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
	}
}
