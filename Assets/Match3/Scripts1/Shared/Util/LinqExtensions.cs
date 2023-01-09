using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Shared.Util
{
	// Token: 0x02000B4D RID: 2893
	internal static class LinqExtensions
	{
		// Token: 0x060043B8 RID: 17336 RVA: 0x00159EDC File Offset: 0x001582DC
		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
		{
			foreach (T obj in ie)
			{
				action(obj);
			}
		}
	}
}
