using System;
using UnityEngine;

namespace Match3.Scripts1.Audio
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public class MusicTrack
	{
		// Token: 0x04000135 RID: 309
		public AudioClip track;

		// Token: 0x04000136 RID: 310
		public AudioId audioId;

		// Token: 0x04000137 RID: 311
		[Range(0f, 1f)]
		public float volume = 1f;
	}
}
