using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000AC5 RID: 2757
namespace Match3.Scripts1
{
	public static class EnumHelper
	{
		// Token: 0x06004181 RID: 16769 RVA: 0x001528B4 File Offset: 0x00150CB4
		public static T TryParse<T>(string value, T defaultEnum) where T : struct, IConvertible
		{
			Dictionary<string, T> dictionary = EnumHelper.ConstructEnumDictionary<T>();
			if (dictionary.ContainsKey(value))
			{
				return dictionary[value];
			}
			return defaultEnum;
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x001528DC File Offset: 0x00150CDC
		private static Dictionary<string, T> ConstructEnumDictionary<T>() where T : struct, IConvertible
		{
			Dictionary<string, T> dictionary = new Dictionary<string, T>();
			foreach (T value in Enum.GetValues(typeof(T)).Cast<T>())
			{
				dictionary.Add(value.ToString(), value);
			}
			return dictionary;
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00152958 File Offset: 0x00150D58
		public static IEnumerable<T> GetValues<T>(params T[] excluded) where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return from item in (T[])Enum.GetValues(typeof(T))
				where !excluded.Contains(item)
				select item;
		}
	}
}
