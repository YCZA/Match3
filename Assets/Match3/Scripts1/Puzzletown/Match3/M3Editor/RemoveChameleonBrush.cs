using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000525 RID: 1317
	public class RemoveChameleonBrush : ABrush
	{
		// Token: 0x0600235F RID: 9055 RVA: 0x0009D0BF File Offset: 0x0009B4BF
		public RemoveChameleonBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveGemBrushComponent());
			this.brushComponents.Add(new RemoveChameleonComponent());
		}
	}
}
