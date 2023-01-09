using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000523 RID: 1315
	public class RemoveGrowingWindowBrush : ABrush
	{
		// Token: 0x0600235D RID: 9053 RVA: 0x0009D00B File Offset: 0x0009B40B
		public RemoveGrowingWindowBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveGrowingWindowComponent());
		}
	}
}
