using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051C RID: 1308
	public class RemoveModifierTileBrush : ABrush
	{
		// Token: 0x06002356 RID: 9046 RVA: 0x0009CEA4 File Offset: 0x0009B2A4
		public RemoveModifierTileBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveTilesBrushComponent());
		}
	}
}
