using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x020004FF RID: 1279
	public class ColorBrush : ABrush
	{
		// Token: 0x0600232D RID: 9005 RVA: 0x0009BFC0 File Offset: 0x0009A3C0
		public ColorBrush(GemColor color, Sprite sprite, ABrush removal) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveDirtAndTreasureComponent());
			this.brushComponents.Add(new ColorBrushComponent(color));
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveChameleonComponent());
			if (!CannonBrush.IsColorValidForCannon(color))
			{
				this.brushComponents.Add(new RemoveCannonBrushComponent());
			}
		}
	}
}
