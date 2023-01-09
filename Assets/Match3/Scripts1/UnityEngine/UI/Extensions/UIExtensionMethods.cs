using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C35 RID: 3125
	public static class UIExtensionMethods
	{
		// Token: 0x060049B0 RID: 18864 RVA: 0x001792C4 File Offset: 0x001776C4
		public static Canvas GetParentCanvas(this RectTransform rt)
		{
			RectTransform rectTransform = rt;
			Canvas canvas = rt.GetComponent<Canvas>();
			int num = 0;
			while (canvas == null || num > 50)
			{
				canvas = rt.GetComponentInParent<Canvas>();
				if (canvas == null)
				{
					rectTransform = rectTransform.parent.GetComponent<RectTransform>();
					num++;
				}
			}
			return canvas;
		}
	}
}
