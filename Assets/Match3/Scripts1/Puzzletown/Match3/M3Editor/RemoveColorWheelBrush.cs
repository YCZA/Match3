using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x0200051A RID: 1306
	public class RemoveColorWheelBrush : ABrush
	{
		// Token: 0x06002353 RID: 9043 RVA: 0x0009CE5B File Offset: 0x0009B25B
		public RemoveColorWheelBrush(Sprite sprite) : base(sprite, null, 0)
		{
			this.brushComponents.Add(new RemoveColorWheelBrushComponent());
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x0009CE76 File Offset: 0x0009B276
		public override bool RequiresRefreshAll
		{
			get
			{
				return true;
			}
		}
	}
}
