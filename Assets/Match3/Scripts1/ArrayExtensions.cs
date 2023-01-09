

// Token: 0x02000AC0 RID: 2752
namespace Match3.Scripts1
{
	public static class ArrayExtensions
	{
		// Token: 0x0600416B RID: 16747 RVA: 0x0015246C File Offset: 0x0015086C
		public static bool IsEqual<T>(this T[] a1, T[] a2)
		{
			if (a1.Length == a2.Length)
			{
				for (int i = 0; i < a1.Length; i++)
				{
					if (!a1[i].Equals(a2[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
