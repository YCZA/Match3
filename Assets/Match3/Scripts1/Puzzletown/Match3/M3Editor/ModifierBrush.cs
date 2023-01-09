using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000503 RID: 1283
	public class ModifierBrush : ABrush
	{
		// Token: 0x06002331 RID: 9009 RVA: 0x0009C18C File Offset: 0x0009A58C
		protected ModifierBrush(GemModifier modifier, Sprite sprite, ABrush removal = null) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new FieldBrushComponent(true));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
			this.brushComponents.Add(new RemoveCannonBrushComponent());
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
			this.brushComponents.Add(new ModifierBrushComponent(modifier));
		}
	}
}
