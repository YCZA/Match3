using System;
using System.Collections.Generic;

// Token: 0x0200077A RID: 1914
namespace Match3.Scripts1
{
	internal static class FBDictHelper
	{
		// Token: 0x06002F4D RID: 12109 RVA: 0x000DD788 File Offset: 0x000DBB88
		public static TReturnValue GetValue<TKey, TValue, TReturnValue>(this IDictionary<TKey, TValue> dict, TKey key, TReturnValue defaultValue) where TReturnValue : TValue
		{
			if (dict != null && dict.ContainsKey(key))
			{
				TReturnValue result = default(TReturnValue);
				try
				{
					result = (TReturnValue)((object)dict[key]);
				}
				catch (Exception)
				{
				}
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000DD7E4 File Offset: 0x000DBBE4
		public static TReturnVal GetIn<TKey, TValue, TReturnVal>(this IDictionary<TKey, TValue> dictionary, TKey[] keys, TReturnVal defaultValue)
		{
			IDictionary<TKey, object> dict = (IDictionary<TKey, object>)dictionary;
			for (int i = 0; i < keys.Length - 1; i++)
			{
				TKey key = keys[i];
				dict = (dict.GetValue(key, FBDictHelper.emptyDict) as IDictionary<TKey, object>);
			}
			return dict.GetValue(keys[keys.Length - 1], defaultValue);
		}

		// Token: 0x0400586A RID: 22634
		private static Dictionary<string, object> emptyDict = new Dictionary<string, object>();
	}
}
