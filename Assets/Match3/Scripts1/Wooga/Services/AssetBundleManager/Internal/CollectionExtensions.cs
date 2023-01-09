using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager.Internal
{
	// Token: 0x0200030E RID: 782
	public static class CollectionExtensions
	{
		// Token: 0x06001881 RID: 6273 RVA: 0x0006FA90 File Offset: 0x0006DE90
		public static V Get<K, V>(this Dictionary<K, V> dict, K key, V defaultValue = default(V))
		{
			V result;
			if (!dict.TryGetValue(key, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0006FAB0 File Offset: 0x0006DEB0
		public static T Select<K, V, T>(this Dictionary<K, V> dict, K key, Func<V, T> found, Func<T> missing)
		{
			V arg;
			return (!dict.TryGetValue(key, out arg)) ? missing() : found(arg);
		}
	}
}
