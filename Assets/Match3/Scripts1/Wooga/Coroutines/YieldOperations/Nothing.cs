namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E4 RID: 996
	public sealed class Nothing : EmptyEnumerator
	{
		// Token: 0x06001E02 RID: 7682 RVA: 0x0007FC88 File Offset: 0x0007E088
		private Nothing()
		{
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0007FC90 File Offset: 0x0007E090
		public static Nothing AtAll
		{
			get
			{
				return Nothing.atAll;
			}
		}

		// Token: 0x040049F2 RID: 18930
		private static readonly Nothing atAll = new Nothing();
	}
}
