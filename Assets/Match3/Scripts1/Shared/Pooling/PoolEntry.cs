using System;
using UnityEngine;

namespace Shared.Pooling
{
	// Token: 0x02000B0E RID: 2830
	[Serializable]
	public class PoolEntry
	{
		// Token: 0x04006B8C RID: 27532
		public GameObject prefab;

		// Token: 0x04006B8D RID: 27533
		public int numOfPreloadedInstances;
	}
}
