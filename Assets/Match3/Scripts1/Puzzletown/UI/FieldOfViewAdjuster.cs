using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009CA RID: 2506
	public class FieldOfViewAdjuster : AUiAdjuster
	{
		// Token: 0x06003CB7 RID: 15543 RVA: 0x00130878 File Offset: 0x0012EC78
		protected override void AdjustValues()
		{
			FieldOfViewAdjuster.Setting matchingSetting = base.GetMatchingSetting<FieldOfViewAdjuster.Setting>(this.settings);
			if (matchingSetting != null && this.mainCamera != null)
			{
				this.mainCamera.fieldOfView = matchingSetting.fieldOfView;
			}
		}

		// Token: 0x04006575 RID: 25973
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x04006576 RID: 25974
		public FieldOfViewAdjuster.Setting[] settings;

		// Token: 0x020009CB RID: 2507
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
			// Token: 0x04006577 RID: 25975
			public float fieldOfView;
		}
	}
}
