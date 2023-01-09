using System;

namespace Match3.Scripts1.QStringUtil
{
	// Token: 0x02000B54 RID: 2900
	public static class QStringUtil
	{
		// Token: 0x060043D7 RID: 17367 RVA: 0x0015A51C File Offset: 0x0015891C
		public static bool QStartsWith(this string self, string substr)
		{
			int i = 0;
			int num = Math.Min(self.Length, substr.Length);
			while (i < num)
			{
				if (self[i] != substr[i])
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0015A564 File Offset: 0x00158964
		public static bool QCompare(this string self, string other)
		{
			if (self.Length != other.Length)
			{
				return false;
			}
			int i = 0;
			int length = self.Length;
			while (i < length)
			{
				if (self[i] != other[i])
				{
					return false;
				}
				i++;
			}
			return true;
		}
	}
}
