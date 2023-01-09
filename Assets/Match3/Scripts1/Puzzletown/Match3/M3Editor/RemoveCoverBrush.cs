using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051E RID: 1310
	public class RemoveCoverBrush : ABrush
	{
		// Token: 0x06002358 RID: 9048 RVA: 0x0009CEDC File Offset: 0x0009B2DC
		public RemoveCoverBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveCratesBrushComponent());
			this.brushComponents.Add(new RemoveChainsBrushComponent());
			this.brushComponents.Add(new RemoveGemModifierComponent());
			this.brushComponents.Add(new RemoveDirtAndTreasureComponent());
			this.brushComponents.Add(new RemoveCannonBrushComponent());
		}
	}
}
