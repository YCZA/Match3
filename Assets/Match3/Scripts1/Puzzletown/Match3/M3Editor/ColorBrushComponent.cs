using System.Linq;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200052D RID: 1325
	public class ColorBrushComponent : IBrushComponent
	{
		// Token: 0x0600238F RID: 9103 RVA: 0x0009E6AF File Offset: 0x0009CAAF
		public ColorBrushComponent(GemColor color)
		{
			this.color = color;
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0009E6BE File Offset: 0x0009CABE
		public virtual void PaintField(Field field, Fields fields)
		{
			if (!field.IsColorWheel)
			{
				field.gem.color = this.color;
				if (ColorBrushComponent.removingTypeColors.Contains(this.color))
				{
					field.gem.type = GemType.Undefined;
				}
			}
		}

		// Token: 0x04004F4A RID: 20298
		protected readonly GemColor color;

		// Token: 0x04004F4B RID: 20299
		public static readonly GemColor[] removingTypeColors = new GemColor[]
		{
			GemColor.Cannonball,
			GemColor.Climber,
			GemColor.Dirt,
			GemColor.Droppable,
			GemColor.Rainbow,
			GemColor.Treasure
		};
	}
}
