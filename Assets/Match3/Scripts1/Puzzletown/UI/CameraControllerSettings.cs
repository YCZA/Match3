using System;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009C9 RID: 2505
	[Serializable]
	public class CameraControllerSettings : AUiAdjuster.UiAdjusterSetting
	{
		// Token: 0x04006572 RID: 25970
		public Vector3 cameraInitialPosition;

		// Token: 0x04006573 RID: 25971
		public float minY;

		// Token: 0x04006574 RID: 25972
		public float maxY;
	}
}
