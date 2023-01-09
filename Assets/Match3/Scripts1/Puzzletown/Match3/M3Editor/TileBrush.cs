using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200050D RID: 1293
	public class TileBrush : FieldModifierBrush
	{
		// Token: 0x0600233D RID: 9021 RVA: 0x0009C5CA File Offset: 0x0009A9CA
		public TileBrush(Sprite sprite, int count, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new TileBrushComponent(count));
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
		}
	}
}
