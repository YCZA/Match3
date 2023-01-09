using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050C RID: 1292
	public class ColorCrateBrush : FieldModifierBrush
	{
		// Token: 0x0600233C RID: 9020 RVA: 0x0009C544 File Offset: 0x0009A944
		public ColorCrateBrush(Sprite sprite, GemColor color, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new ColorCratesBrushComponent(color));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveStoneBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RandomColorBrushComponent());
		}
	}
}
