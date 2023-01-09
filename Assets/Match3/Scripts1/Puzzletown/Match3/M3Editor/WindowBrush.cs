using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050F RID: 1295
	public class WindowBrush : FieldModifierBrush
	{
		// Token: 0x06002340 RID: 9024 RVA: 0x0009C654 File Offset: 0x0009AA54
		public WindowBrush(Sprite sprite, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveHiddenItemBrushComponent());
			this.brushComponents.Add(new ResetModifiersComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new WindowBrushComponent());
		}
	}
}
