using System;
using System.Collections.Generic;

// Token: 0x02000AC6 RID: 2758
namespace Match3.Scripts1
{
	public static class Extensions
	{
		// Token: 0x06004184 RID: 16772 RVA: 0x001529CF File Offset: 0x00150DCF
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> self)
		{
			return new HashSet<T>(self);
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x001529D8 File Offset: 0x00150DD8
		public static int CountIf<T>(this IEnumerable<T> self, Func<T, bool> action)
		{
			int num = 0;
			foreach (T arg in self)
			{
				if (action(arg))
				{
					num++;
				}
			}
			return num;
		}
	}
}
