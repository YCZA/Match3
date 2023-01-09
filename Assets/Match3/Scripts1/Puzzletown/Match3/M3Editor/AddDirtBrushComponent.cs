namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000534 RID: 1332
	public class AddDirtBrushComponent : ModifierBrushComponent
	{
		// Token: 0x0600239F RID: 9119 RVA: 0x0009E8DD File Offset: 0x0009CCDD
		public AddDirtBrushComponent() : base(GemModifier.DirtHp1)
		{
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0009E8E6 File Offset: 0x0009CCE6
		public override void PaintField(Field field, Fields fields)
		{
			if (field.gem.modifier < GemModifier.DirtHp1 || field.gem.modifier > GemModifier.DirtHp3)
			{
				field.gem.modifier = this.modifier;
			}
		}
	}
}
