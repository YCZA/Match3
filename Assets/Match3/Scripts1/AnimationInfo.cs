using UnityEngine;

namespace Match3.Scripts1
{
	// Token: 0x020006AF RID: 1711
	public struct AnimationInfo
	{
		// Token: 0x06002ACC RID: 10956 RVA: 0x000C3EE7 File Offset: 0x000C22E7
		public AnimationInfo(AnimationClip clip, float duration)
		{
			this.clip = clip;
			this.duration = duration;
		}

		// Token: 0x04005418 RID: 21528
		public AnimationClip clip;

		// Token: 0x04005419 RID: 21529
		public float duration;
	}
}
