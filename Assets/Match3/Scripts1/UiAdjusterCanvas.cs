using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200088E RID: 2190
namespace Match3.Scripts1
{
	public class UiAdjusterCanvas : AUiAdjuster
	{
		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x060035BE RID: 13758 RVA: 0x001029B5 File Offset: 0x00100DB5
		private CanvasScaler Scaler
		{
			get
			{
				return base.GetComponent<CanvasScaler>();
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x060035BF RID: 13759 RVA: 0x001029C0 File Offset: 0x00100DC0
		private Vector2 ReferenceResolution
		{
			get
			{
				Vector2 referenceResolution = this.Scaler.referenceResolution;
				if (AUiAdjuster.Orientation == ScreenOrientation.LandscapeLeft || AUiAdjuster.Orientation == ScreenOrientation.LandscapeRight)
				{
					referenceResolution = new Vector2(referenceResolution.y, referenceResolution.x);
				}
				return referenceResolution;
			}
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x00102A08 File Offset: 0x00100E08
		protected override void AdjustValues()
		{
			// 弃用
			return;
			UiAdjusterCanvas.Setting matchingSetting = base.GetMatchingSetting<UiAdjusterCanvas.Setting>(this.settings);
			if (matchingSetting == null)
			{
				return;
			}
			if (this.Scaler)
			{
				Vector2 referenceResolution = this.ReferenceResolution;
				float num = referenceResolution.x / referenceResolution.y;
				num -= 0.008f;
				this.Scaler.matchWidthOrHeight = ((AUiAdjuster.CurrentAspectRatio < num) ? matchingSetting.matchWidthOrHeight : matchingSetting.alternativeMatchWidthOrHeight);
			}
		}

		// Token: 0x04005DBB RID: 23995
		public UiAdjusterCanvas.Setting[] settings;

		// Token: 0x0200088F RID: 2191
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
			// Token: 0x04005DBC RID: 23996
			public float matchWidthOrHeight;

			// Token: 0x04005DBD RID: 23997
			public float alternativeMatchWidthOrHeight;
		}
	}
}
