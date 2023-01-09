namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200052F RID: 1327
	public class RandomColorBrushComponent : ColorBrushComponent
	{
		// Token: 0x06002394 RID: 9108 RVA: 0x0009E744 File Offset: 0x0009CB44
		public RandomColorBrushComponent() : base(GemColor.Random)
		{
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0009E750 File Offset: 0x0009CB50
		public override void PaintField(Field field, Fields fields)
		{
			if ((!field.HasGem || field.gem.CanNotBeCovered) && !field.IsResistantBlocker && !field.IsColorWheel)
			{
				field.gem.color = this.color;
			}
		}
	}
}
