using System.Collections.Generic;

namespace Shared.Pooling
{
	// Token: 0x02000B13 RID: 2835
	public static class ListPool<T>
	{
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x00156046 File Offset: 0x00154446
		public static int Count
		{
			get
			{
				return ListPool<T>.items.Count;
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00156054 File Offset: 0x00154454
		public static void Init(int poolSize, int capacity = 0)
		{
			int num = poolSize - ListPool<T>.Count;
			for (int i = 0; i < num; i++)
			{
				ListPool<T>.items.Add(new List<T>(capacity));
			}
			WoogaDebug.Log(new object[]
			{
				string.Concat(new object[]
				{
					"created ",
					num,
					" lists with capacity ",
					capacity
				})
			});
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x001560C6 File Offset: 0x001544C6
		public static List<T> Create(int minCapacity = 10)
		{
			if (ListPool<T>.items.Count > 0)
			{
				return ListPool<T>.items.Pop<List<T>>();
			}
			return new List<T>(minCapacity);
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x001560E9 File Offset: 0x001544E9
		public static void Release(List<T> item)
		{
			item.Clear();
			ListPool<T>.items.Add(item);
		}

		// Token: 0x04006B92 RID: 27538
		private static List<List<T>> items = new List<List<T>>();
	}
}
