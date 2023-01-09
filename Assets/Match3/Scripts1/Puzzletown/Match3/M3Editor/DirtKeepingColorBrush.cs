using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000501 RID: 1281
	public class DirtKeepingColorBrush : ABrush
	{
		// Token: 0x0600232F RID: 9007 RVA: 0x0009C0AC File Offset: 0x0009A4AC
		public DirtKeepingColorBrush(GemColor color, Sprite sprite, ABrush removal) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new ColorBrushComponent(color));
			this.brushComponents.Add(new AddDirtBrushComponent());
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
		}
	}
}
