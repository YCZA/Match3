using System.Collections.Generic;

namespace Shared.Pooling
{
	// Token: 0x02000B14 RID: 2836
	public static class ListExtensions
	{
		// Token: 0x060042C5 RID: 17093 RVA: 0x00156108 File Offset: 0x00154508
		public static T Pop<T>(this List<T> list)
		{
			if (list.Count > 0)
			{
				T result = list[0];
				list.RemoveAt(0);
				return result;
			}
			return default(T);
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x0015613B File Offset: 0x0015453B
		public static void ReturnToPool<T>(this List<T> list)
		{
			ListPool<T>.Release(list);
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x00156144 File Offset: 0x00154544
		public static void ReleasePooled<T>(this List<T> list) where T : IPoolable, new()
		{
			for (int i = 0; i < list.Count; i++)
			{
				Pool<T>.ReturnToPool(list[i]);
			}
			ListPool<T>.Release(list);
		}
	}
}
