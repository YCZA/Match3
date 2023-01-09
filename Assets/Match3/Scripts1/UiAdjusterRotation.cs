using System;
using UnityEngine;

// Token: 0x02000892 RID: 2194
namespace Match3.Scripts1
{
	public class UiAdjusterRotation : AUiAdjuster
	{
		// Token: 0x060035C6 RID: 13766 RVA: 0x00102AFD File Offset: 0x00100EFD
		private void OnEnable()
		{
			this.AdjustValues();
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x00102B08 File Offset: 0x00100F08
		protected override void AdjustValues()
		{
			// 弃用
			return;
			UiAdjusterRotation.Setting matchingSetting = base.GetMatchingSetting<UiAdjusterRotation.Setting>(this.settings);
			if (matchingSetting == null)
			{
				return;
			}
			base.transform.localRotation = Quaternion.Euler(matchingSetting.rotation);
		}

		// Token: 0x04005DC3 RID: 24003
		public UiAdjusterRotation.Setting[] settings;

		// Token: 0x02000893 RID: 2195
		[Serializable]
		public class Setting : AUiAdjuster.UiAdjusterSetting
		{
			// Token: 0x04005DC4 RID: 24004
			public Vector3 rotation;
		}
	}
}
