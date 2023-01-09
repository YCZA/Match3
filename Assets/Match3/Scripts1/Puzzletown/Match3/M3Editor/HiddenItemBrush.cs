using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000516 RID: 1302
	public class HiddenItemBrush : ABrush
	{
		// Token: 0x06002347 RID: 9031 RVA: 0x0009CA00 File Offset: 0x0009AE00
		public HiddenItemBrush(Sprite sprite, int size, bool isRandom, ABrush removal) : base(sprite, removal, 0)
		{
			this.brushComponents.Add(new HiddenItemBrushComponent(size, isRandom));
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0009CA1E File Offset: 0x0009AE1E
		public override bool RequiresRefreshAll
		{
			get
			{
				return true;
			}
		}
	}
}
