using System;
using UnityEngine;

// Token: 0x020004DE RID: 1246
namespace Match3.Scripts1
{
	[Serializable]
	public class CameraShakeStyle
	{
		// Token: 0x04004E42 RID: 20034
		public AnimationCurve ShakeStrength;

		// Token: 0x04004E43 RID: 20035
		public AnimationCurve ShakeFrequency;

		// Token: 0x04004E44 RID: 20036
		public float AnimationLength = 0.2f;

		// Token: 0x04004E45 RID: 20037
		public bool Loop;
	}
}
