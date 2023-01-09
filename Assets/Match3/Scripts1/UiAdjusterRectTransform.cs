using System;
using UnityEngine;

// Token: 0x02000890 RID: 2192
namespace Match3.Scripts1
{
	public class UiAdjusterRectTransform : AUiAdjuster
	{
		// Token: 0x060035C3 RID: 13763 RVA: 0x00102A90 File Offset: 0x00100E90
		protected override void AdjustValues()
		{
			// 弃用
			return;
			UiAdjusterRectTransform.Setting matchingSetting = base.GetMatchingSetting<UiAdjusterRectTransform.Setting>(this.settings);
			if (matchingSetting == null)
			{
				return;
			}
			RectTransform component = base.GetComponent<RectTransform>();
			component.anchorMax = matchingSetting.anchorMax;
			component.anchorMin = matchingSetting.anchorMin;
			component.offsetMin = matchingSetting.leftBottom;
			component.offsetMax = -matchingSetting.rightTop;
		}

		// Token: 0x04005DBE RID: 23998
		public UiAdjusterRectTransform.Setting[] settings;

		// Token: 0x02000891 RID: 2193
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
			// Token: 0x04005DBF RID: 23999
			public Vector2 anchorMin;

			// Token: 0x04005DC0 RID: 24000
			public Vector2 anchorMax;

			// Token: 0x04005DC1 RID: 24001
			public Vector2 leftBottom;

			// Token: 0x04005DC2 RID: 24002
			public Vector2 rightTop;
		}
	}
}
