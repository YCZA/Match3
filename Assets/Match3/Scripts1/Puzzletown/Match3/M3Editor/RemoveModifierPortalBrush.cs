using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051D RID: 1309
	public class RemoveModifierPortalBrush : ABrush
	{
		// Token: 0x06002357 RID: 9047 RVA: 0x0009CEBF File Offset: 0x0009B2BF
		public RemoveModifierPortalBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemovePortalBrushComponent());
		}
	}
}
