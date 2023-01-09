using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050A RID: 1290
	public class ChainBrush : FieldModifierBrush
	{
		// Token: 0x0600233A RID: 9018 RVA: 0x0009C434 File Offset: 0x0009A834
		public ChainBrush(Sprite sprite, int count, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new ChainBrushComponent(count));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveStoneBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new RandomColorBrushComponent());
		}
	}
}
