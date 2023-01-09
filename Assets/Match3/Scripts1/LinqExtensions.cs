using System;
using System.Collections.Generic;

// Token: 0x02000AC8 RID: 2760
namespace Match3.Scripts1
{
	internal static class LinqExtensions
	{
		// Token: 0x06004187 RID: 16775 RVA: 0x00152A48 File Offset: 0x00150E48
		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
		{
			foreach (T obj in ie)
			{
				action(obj);
			}
		}
	}
}
