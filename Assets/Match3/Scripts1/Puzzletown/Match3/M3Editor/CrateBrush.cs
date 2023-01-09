using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050B RID: 1291
	public class CrateBrush : FieldModifierBrush
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x0009C4BC File Offset: 0x0009A8BC
		public CrateBrush(Sprite sprite, int count, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new CratesBrushComponent(count));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveStoneBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RandomColorBrushComponent());
		}
	}
}
