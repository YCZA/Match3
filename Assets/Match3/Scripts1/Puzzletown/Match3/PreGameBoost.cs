namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200057C RID: 1404
	public abstract class PreGameBoost : ABoost
	{
		// Token: 0x060024D7 RID: 9431 RVA: 0x000A4DB5 File Offset: 0x000A31B5
		protected PreGameBoost(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000A4DBF File Offset: 0x000A31BF
		public override bool IsValid()
		{
			return true;
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000A4DC4 File Offset: 0x000A31C4
		protected bool IsReplaceableNormalGem(IntVector2 position)
		{
			return !this.fields.prePositionedGems.Contains(position) && this.fields[position].gem.type == GemType.Undefined && this.fields[position].gem.modifier == GemModifier.Undefined;
		}
	}
}
