using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000524 RID: 1316
	public class GrowingWindowBrush : ABrush
	{
		// Token: 0x0600235E RID: 9054 RVA: 0x0009D028 File Offset: 0x0009B428
		public GrowingWindowBrush(Sprite sprite, ABrush removal) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new GrowingWindowBrushComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
		}
	}
}
