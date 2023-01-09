using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.Scripts1.Audio.Scripts
{
	// Token: 0x02000027 RID: 39
	public class MusicLibrary : MonoBehaviour
	{
		// Token: 0x06000160 RID: 352 RVA: 0x000084E8 File Offset: 0x000068E8
		public MusicLibrary.MusicCollection GetCollectionByName(string name)
		{
			return this.collections.FirstOrDefault((MusicLibrary.MusicCollection e) => e.collectionName == name);
		}

		// Token: 0x0400012C RID: 300
		[SerializeField]
		private List<MusicLibrary.MusicCollection> collections;

		// Token: 0x02000028 RID: 40
		[Serializable]
		public class MusicCollection
		{
			// Token: 0x0400012D RID: 301
			public string collectionName;

			// Token: 0x0400012E RID: 302
			public MusicTrack[] musicTracks;
		}
	}
}
