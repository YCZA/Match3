using System.Collections;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003A9 RID: 937
	public static class IEnumerableExtensions
	{
		// Token: 0x06001C48 RID: 7240 RVA: 0x0007C602 File Offset: 0x0007AA02
		public static bool IsNullOrEmptyCollection(this ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x0007C616 File Offset: 0x0007AA16
		public static bool IsNullOrEmptyEnumerable(this IEnumerable enumerable)
		{
			return enumerable == null || !enumerable.GetEnumerator().MoveNext();
		}
	}
}
