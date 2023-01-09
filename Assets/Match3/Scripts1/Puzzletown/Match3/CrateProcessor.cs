namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200060D RID: 1549
	public class CrateProcessor : AMatchProcessor
	{
		// Token: 0x060027A5 RID: 10149 RVA: 0x000B0380 File Offset: 0x000AE780
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.currentColor = match.Group.Color;
			this.ignoreColor = (this.currentColor == GemColor.Undefined || (match is IExplosionResult && !(match is IRainbowExplosion)));
			base.ProcessMatch(match, fields);
			if (match is IMatchWithAffectedFields)
			{
				foreach (IntVector2 intVector in ((IMatchWithAffectedFields)match).Fields)
				{
					this.CheckField(fields[intVector], intVector);
				}
			}
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000B043C File Offset: 0x000AE83C
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (!field.isExploded && field.HasCrates && (this.ignoreColor || Crate.MatchesColor(field.cratesIndex, this.currentColor)))
			{
				field.cratesIndex--;
				field.isExploded = true;
				field.removedModifier = true;
				this.results.Add(new CrateExplosion(field, createdFrom));
				if (!field.HasCrates)
				{
					field.cratesIndex = 0;
				}
			}
		}

		// Token: 0x04005209 RID: 21001
		private GemColor currentColor;

		// Token: 0x0400520A RID: 21002
		private bool ignoreColor;
	}
}
