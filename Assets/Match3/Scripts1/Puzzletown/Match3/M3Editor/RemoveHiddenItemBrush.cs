using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000517 RID: 1303
	public class RemoveHiddenItemBrush : ABrush
	{
		// Token: 0x06002349 RID: 9033 RVA: 0x0009CA21 File Offset: 0x0009AE21
		public RemoveHiddenItemBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveHiddenItemBrushComponent());
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x0009CA3C File Offset: 0x0009AE3C
		public override bool RequiresRefreshAll
		{
			get
			{
				return true;
			}
		}
	}
}
