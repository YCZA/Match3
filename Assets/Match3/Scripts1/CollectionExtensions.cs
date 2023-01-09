using System.Collections.Generic;

// Token: 0x02000AC2 RID: 2754
namespace Match3.Scripts1
{
	public static class CollectionExtensions
	{
		// Token: 0x0600416D RID: 16749 RVA: 0x001524CF File Offset: 0x001508CF
		public static void AddIfNotNull<T>(this ICollection<T> collection, T item) where T : class
		{
			if (item != null)
			{
				collection.Add(item);
			}
		}
	}
}
