using System;
using System.Collections.Generic;

// Token: 0x02000AC4 RID: 2756
namespace Match3.Scripts1
{
	public static class DictionaryExtensions
	{
		// Token: 0x06004177 RID: 16759 RVA: 0x00152654 File Offset: 0x00150A54
		public static IDictionary<TKey, TValue> MergeLeft<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
		{
			if (second == null)
			{
				return first;
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in second)
			{
				if (!first.ContainsKey(keyValuePair.Key))
				{
					first.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return first;
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x001526D0 File Offset: 0x00150AD0
		public static IDictionary<TKey, TValue> MergeRight<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
		{
			if (second == null)
			{
				return first;
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in second)
			{
				first[keyValuePair.Key] = keyValuePair.Value;
			}
			return first;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0015273C File Offset: 0x00150B3C
		public static void AddElementToList<TKey, TList, TItem>(this IDictionary<TKey, TList> dict, TKey key, TItem item) where TList : List<TItem>, new()
		{
			if (!dict.ContainsKey(key))
			{
				dict[key] = Activator.CreateInstance<TList>();
			}
			TList tlist = dict[key];
			tlist.Add(item);
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00152778 File Offset: 0x00150B78
		public static void AddOrIncrement<TKey>(this IDictionary<TKey, int> dict, TKey key, int value)
		{
			if (!dict.ContainsKey(key))
			{
				dict[key] = 0;
			}
			dict[key] += value;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x001527B0 File Offset: 0x00150BB0
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
		{
			return (dict == null || !dict.ContainsKey(key)) ? default(TValue) : dict[key];
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x001527E4 File Offset: 0x00150BE4
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
		{
			return (dict == null || !dict.ContainsKey(key)) ? defaultValue : dict[key];
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x00152805 File Offset: 0x00150C05
		public static TValue GetValueOrAddAndReturnDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultValue)
		{
			if (!dict.ContainsKey(key))
			{
				dict[key] = defaultValue();
			}
			return dict[key];
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x00152827 File Offset: 0x00150C27
		public static TValue GetValueOrCreateAndAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (!dict.ContainsKey(key))
			{
				dict[key] = Activator.CreateInstance<TValue>();
			}
			return dict[key];
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x00152848 File Offset: 0x00150C48
		public static Dictionary<TKey, TValue> AddAndReturnDict<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (dict != null)
			{
				dict[key] = value;
			}
			return dict;
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0015285C File Offset: 0x00150C5C
		public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
		{
			foreach (TKey key in keys)
			{
				dict.Remove(key);
			}
		}
	}
}
