using System;
using System.Collections.Generic;

namespace Shared.Pooling
{
	// Token: 0x02000B12 RID: 2834
	public static class Pool<T> where T : IPoolable, new()
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060042BC RID: 17084 RVA: 0x00155FF2 File Offset: 0x001543F2
		public static int Count
		{
			get
			{
				return Pool<T>.items.Count;
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00155FFE File Offset: 0x001543FE
		public static T Get()
		{
			if (Pool<T>.items.Count > 0)
			{
				return Pool<T>.items.Pop<T>();
			}
			return Activator.CreateInstance<T>();
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00156020 File Offset: 0x00154420
		public static void ReturnToPool(T item)
		{
			item.Release();
			Pool<T>.items.Add(item);
		}

		// Token: 0x04006B91 RID: 27537
		private static List<T> items = new List<T>();
	}
}
