

// Token: 0x02000AC7 RID: 2759
namespace Match3.Scripts1
{
	public static class IntExtensions
	{
		// Token: 0x06004186 RID: 16774 RVA: 0x00152A38 File Offset: 0x00150E38
		public static bool IsBetween(this int x, int inclusiveLower, int exclusiveUpper)
		{
			return x >= inclusiveLower && x < exclusiveUpper;
		}
	}
}
