using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020004DA RID: 1242
	[CreateAssetMenu(menuName = "Puzzletown/MapClipToGameObject")]
	public class MapClipToGameObject : ScriptableObject
	{
		// Token: 0x0600229C RID: 8860 RVA: 0x000993D4 File Offset: 0x000977D4
		public MapClipToGameObject.GemMapEntry FindEntry(AnimationClip clip)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].key == clip)
				{
					return this.entries[i];
				}
			}
			return null;
		}

		// Token: 0x04004E34 RID: 20020
		public List<MapClipToGameObject.GemMapEntry> entries;

		// Token: 0x020004DB RID: 1243
		[Serializable]
		public class GemMapEntry : AssetMappingEntry<AnimationClip, GameObject>
		{
		}
	}
}
