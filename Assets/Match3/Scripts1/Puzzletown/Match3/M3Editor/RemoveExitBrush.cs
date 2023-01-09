using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000522 RID: 1314
	public class RemoveExitBrush : ABrush
	{
		// Token: 0x0600235C RID: 9052 RVA: 0x0009CFF0 File Offset: 0x0009B3F0
		public RemoveExitBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveExitComponent());
		}
	}
}
