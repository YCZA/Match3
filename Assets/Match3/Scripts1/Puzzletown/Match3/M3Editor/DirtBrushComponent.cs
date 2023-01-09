namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200052E RID: 1326
	public class DirtBrushComponent : ColorBrushComponent
	{
		// Token: 0x06002392 RID: 9106 RVA: 0x0009E715 File Offset: 0x0009CB15
		public DirtBrushComponent() : base(GemColor.Dirt)
		{
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0009E71F File Offset: 0x0009CB1F
		public override void PaintField(Field field, Fields fields)
		{
			if (field.gem.color != GemColor.Treasure)
			{
				field.gem.color = this.color;
			}
		}
	}
}
