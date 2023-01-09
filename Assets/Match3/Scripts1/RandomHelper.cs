using System;
using System.Collections.Generic;

// Token: 0x02000ACA RID: 2762
namespace Match3.Scripts1
{
	public static class RandomHelper
	{
		// Token: 0x0600419B RID: 16795 RVA: 0x00152E78 File Offset: 0x00151278
		public static int Next(int excludedMaxValue)
		{
			return RandomHelper.r.Next(excludedMaxValue);
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x00152E85 File Offset: 0x00151285
		public static int Next(int includedMinValue, int excludedMaxValue)
		{
			return RandomHelper.r.Next(includedMinValue, excludedMaxValue);
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x00152E94 File Offset: 0x00151294
		public static T Next<T>(T[] array)
		{
			if (array.IsNullOrEmptyCollection())
			{
				return default(T);
			}
			return array[RandomHelper.Next(array.Length)];
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x00152EC4 File Offset: 0x001512C4
		public static T Next<T>(List<T> list)
		{
			if (list.IsNullOrEmptyCollection())
			{
				return default(T);
			}
			return list[RandomHelper.Next(list.Count)];
		}

		// Token: 0x04006AF1 RID: 27377
		public static Random r = new Random();
	}
}
