using System.Collections.Generic;
using Match3.Scripts1.Audio;
using UnityEngine;

// Token: 0x02000025 RID: 37
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "AudioRegistry", menuName = "Puzzletown/Audio Registry")]
	public class AudioRegistry : ScriptableObject
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00007DC0 File Offset: 0x000061C0
		public void Merge(AudioRegistry other)
		{
			foreach (KeyValuePair<AudioId, AudioItem> keyValuePair in other.audioItemDictionary)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.audioClip != null)
				{
					this.audioItemDictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0400011E RID: 286
		public AudioItemDictionary audioItemDictionary = new AudioItemDictionary();
	}
}
