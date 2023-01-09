using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

// Token: 0x02000AC9 RID: 2761
namespace Match3.Scripts1
{
	public static class ListExtensions
	{
		// Token: 0x06004188 RID: 16776 RVA: 0x00152A9C File Offset: 0x00150E9C
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = RandomHelper.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x00152AE8 File Offset: 0x00150EE8
		public static T RandomElement<T>(this IList<T> list, bool removeFromList = false)
		{
			if (list == null || list.Count == 0)
			{
				return default(T);
			}
			int index = RandomHelper.Next(list.Count);
			T result = list[index];
			if (removeFromList)
			{
				list.RemoveAt(index);
			}
			return result;
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x00152B34 File Offset: 0x00150F34
		public static T PtFirst<T>(this IList<T> list, Predicate<T> condition)
		{
			T t = list.PtFirstOrDefault(condition);
			if (t == null)
			{
				throw new AccessViolationException("Didn't find matching element in list!");
			}
			return t;
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x00152B60 File Offset: 0x00150F60
		public static T PtFirstOrDefault<T>(this IList<T> list, Predicate<T> condition)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (condition(list[i]))
				{
					return list[i];
				}
			}
			return default(T);
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x00152BA8 File Offset: 0x00150FA8
		public static T RemoveAndGetAt<T>(this IList<T> list, int index = 0)
		{
			T result = list[index];
			list.RemoveAt(index);
			return result;
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x00152BC5 File Offset: 0x00150FC5
		public static T RemoveAndGetLast<T>(this IList<T> list)
		{
			return list.RemoveAndGetAt(list.Count - 1);
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x00152BD8 File Offset: 0x00150FD8
		public static List<string> ConvertToList(this string str, char delimiter = ',', bool removeWhiteSpace = true, bool includeEmptyEntries = true)
		{
			string text = str;
			if (removeWhiteSpace)
			{
				text = str.Replace(" ", string.Empty);
			}
			if (string.IsNullOrEmpty(text))
			{
				return new List<string>();
			}
			if (includeEmptyEntries)
			{
				return new List<string>(text.Split(new char[]
				{
					delimiter
				}));
			}
			return (from stringValue in new List<string>(text.Split(new char[]
				{
					delimiter
				}))
				where !string.IsNullOrEmpty(stringValue)
				select stringValue).ToList<string>();
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x00152C67 File Offset: 0x00151067
		public static List<int> ConvertToIntList(this string str, char delimiter = ',')
		{
			List<string> list = str.ConvertToList(delimiter, true, false);
			if (ListExtensions._003C_003Ef__mg_0024cache0 == null)
			{
				ListExtensions._003C_003Ef__mg_0024cache0 = new Converter<string, int>(Convert.ToInt32);
			}
			return list.ConvertAll<int>(ListExtensions._003C_003Ef__mg_0024cache0);
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x00152C94 File Offset: 0x00151094
		public static T AddIfNotAlreadyPresent<T>(this List<T> itemList, T itemToAdd, bool allowNull = false)
		{
			if ((itemToAdd != null || allowNull) && !itemList.Contains(itemToAdd))
			{
				itemList.Add(itemToAdd);
			}
			return itemToAdd;
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x00152CBC File Offset: 0x001510BC
		public static List<T> AddRangeElementsIfNotAlreadyPresent<T>(this List<T> itemList, IEnumerable<T> elementsToAdd, bool allowNull = false)
		{
			if (!elementsToAdd.IsNullOrEmptyEnumerable())
			{
				foreach (T itemToAdd in elementsToAdd)
				{
					itemList.AddIfNotAlreadyPresent(itemToAdd, allowNull);
				}
			}
			return itemList;
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x00152D20 File Offset: 0x00151120
		public static List<T> AddElements<T>(this List<T> itemList, params T[] itemsToAdd)
		{
			itemList.AddRange(itemsToAdd);
			return itemList;
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x00152D2C File Offset: 0x0015112C
		public static void PopulateWithNew<T>(this IList<T> list, int count = 0) where T : new()
		{
			if (list == null)
			{
				throw new NullReferenceException();
			}
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				list.Add(Activator.CreateInstance<T>());
			}
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x00152D68 File Offset: 0x00151168
		public static void CopyWithCapacityChange<T>(this IList<T> list, int count = 0) where T : new()
		{
			if (list == null)
			{
				throw new NullReferenceException();
			}
			List<T> list2 = new List<T>(list);
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				if (i < list2.Count)
				{
					list.Add(list2[i]);
				}
				else
				{
					list.Add(Activator.CreateInstance<T>());
				}
			}
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x00152DCC File Offset: 0x001511CC
		public static T ElementAtOrLastElement<T>(this List<T> list, int index)
		{
			if (list.IsNullOrEmptyCollection())
			{
				return default(T);
			}
			int index2 = Math.Min(list.Count - 1, index);
			return list[index2];
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x00152E04 File Offset: 0x00151204
		public static T ElementAtOrDefault<T>(this List<T> list, int index)
		{
			return (index <= list.Count - 1) ? list[index] : default(T);
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x00152E34 File Offset: 0x00151234
		public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
		{
			return (from item in listToClone
				select (T)((object)item.Clone())).ToList<T>();
		}

		// Token: 0x04006AF0 RID: 27376
		[CompilerGenerated]
		private static Converter<string, int> _003C_003Ef__mg_0024cache0;
	}
}
