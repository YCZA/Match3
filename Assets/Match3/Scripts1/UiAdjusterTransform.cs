using System;
using UnityEngine;

// Token: 0x02000894 RID: 2196
namespace Match3.Scripts1
{
	public class UiAdjusterTransform : AUiAdjuster
	{
		// Token: 0x060035CA RID: 13770 RVA: 0x00102B50 File Offset: 0x00100F50
		protected override void AdjustValues()
		{
			// 弃用
			return;
			UiAdjusterTransform.Setting matchingSetting = base.GetMatchingSetting<UiAdjusterTransform.Setting>(this.settings);
			if (matchingSetting == null)
			{
				return;
			}
			RectTransform component = base.GetComponent<RectTransform>();
			if (component != null)
			{
				component.anchoredPosition = matchingSetting.localPosition;
			}
			else
			{
				base.transform.localPosition = matchingSetting.localPosition;
			}
		}

		// Token: 0x04005DC5 RID: 24005
		public UiAdjusterTransform.Setting[] settings;

		// Token: 0x02000895 RID: 2197
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
			// Token: 0x04005DC6 RID: 24006
			public Vector3 localPosition;
		}
	}
}
