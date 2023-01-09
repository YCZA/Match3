using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Shared.Pooling
{
	// Token: 0x02000B10 RID: 2832
	public class PoolMarker : MonoBehaviour
	{
		// Token: 0x04006B8E RID: 27534
		public readonly Signal onReleased = new Signal();

		// Token: 0x04006B8F RID: 27535
		public int prefabId;

		// Token: 0x04006B90 RID: 27536
		public ObjectPool pool;
	}
}
