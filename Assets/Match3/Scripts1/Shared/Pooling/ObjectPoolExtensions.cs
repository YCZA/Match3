using UnityEngine;

namespace Shared.Pooling
{
	// Token: 0x02000B0F RID: 2831
	public static class ObjectPoolExtensions
	{
		// Token: 0x060042B8 RID: 17080 RVA: 0x00155F74 File Offset: 0x00154374
		public static void Release(this GameObject go)
		{
			PoolMarker component = go.GetComponent<PoolMarker>();
			if (component != null)
			{
				ObjectPool pool = component.pool;
				if (pool != null)
				{
					pool.Release(go);
				}
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x00155FB0 File Offset: 0x001543B0
		public static void Release(this GameObject go, float delay)
		{
			PoolMarker component = go.GetComponent<PoolMarker>();
			if (component != null)
			{
				ObjectPool pool = component.pool;
				pool.Release(go, delay);
			}
		}
	}
}
