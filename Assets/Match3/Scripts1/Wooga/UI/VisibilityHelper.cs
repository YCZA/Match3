using UnityEngine;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B45 RID: 2885
	public class VisibilityHelper
	{
		// Token: 0x060043AB RID: 17323 RVA: 0x00159AD9 File Offset: 0x00157ED9
		public VisibilityHelper(RectTransform container)
		{
			this.container = container;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x00159AF4 File Offset: 0x00157EF4
		public bool IsVisible(RectTransform rt)
		{
			Rect worldRect = this.GetWorldRect(this.container);
			return this.GetWorldRect(rt).Overlaps(worldRect);
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x00159B20 File Offset: 0x00157F20
		private Rect GetWorldRect(RectTransform rt)
		{
			Rect result = default(Rect);
			rt.GetWorldCorners(this.corners);
			result.min = this.corners[0];
			result.max = this.corners[2];
			return result;
		}

		// Token: 0x04006C10 RID: 27664
		private RectTransform container;

		// Token: 0x04006C11 RID: 27665
		private Vector3[] corners = new Vector3[4];
	}
}
