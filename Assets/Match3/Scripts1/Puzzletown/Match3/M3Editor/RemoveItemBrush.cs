using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051B RID: 1307
	public class RemoveItemBrush : ABrush
	{
		// Token: 0x06002355 RID: 9045 RVA: 0x0009CE79 File Offset: 0x0009B279
		public RemoveItemBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveBlockerBrushComponent());
		}
	}
}
