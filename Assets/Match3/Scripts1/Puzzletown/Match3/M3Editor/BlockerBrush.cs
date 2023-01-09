using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000509 RID: 1289
	public class BlockerBrush : FieldModifierBrush
	{
		// Token: 0x06002339 RID: 9017 RVA: 0x0009C3DC File Offset: 0x0009A7DC
		public BlockerBrush(Sprite sprite, int count, ABrush removal) : base(sprite, removal)
		{
			this.brushComponents.Add(new BlockerBrushComponent(count));
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveDefinedGemSpawnComponent());
		}
	}
}
