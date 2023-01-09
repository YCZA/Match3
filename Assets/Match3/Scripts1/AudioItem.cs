using System;
using Match3.Scripts1.Audio;
using UnityEngine;

// Token: 0x02000023 RID: 35
namespace Match3.Scripts1
{
	[Serializable]
	public class AudioItem
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000798B File Offset: 0x00005D8B
		public AudioItem(AudioId id)
		{
			this.audioId = id;
			this.audioCategory = AudioCategory.Misc;
			this.description = "enter a description for the sound event";
			this.loadType = AudioClipLoadType.CompressedInMemory;
			this.volume = 1f;
		}

		// Token: 0x04000117 RID: 279
		public AudioId audioId;

		// Token: 0x04000118 RID: 280
		public AudioCategory audioCategory;

		// Token: 0x04000119 RID: 281
		public string description;

		// Token: 0x0400011A RID: 282
		public AudioClip audioClip;

		// Token: 0x0400011B RID: 283
		public AudioClipLoadType loadType;

		// Token: 0x0400011C RID: 284
		public float volume;

		// Token: 0x0400011D RID: 285
		[NonSerialized]
		public string bundleName;
	}
}
