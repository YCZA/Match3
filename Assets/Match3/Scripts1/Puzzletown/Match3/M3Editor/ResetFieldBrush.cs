using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000520 RID: 1312
	public class ResetFieldBrush : ABrush
	{
		// Token: 0x0600235A RID: 9050 RVA: 0x0009CF44 File Offset: 0x0009B344
		public ResetFieldBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveHiddenItemBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new ResetModifiersComponent());
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new FieldBrushComponent(true));
		}
	}
}
