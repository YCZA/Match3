using System;

namespace Wooga.Core.Utilities
{
	// Token: 0x020003D9 RID: 985
	public static class ExceptionUtils
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x0007F642 File Offset: 0x0007DA42
		public static object RethrowException(Exception e)
		{
			throw e;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0007F645 File Offset: 0x0007DA45
		public static T RethrowException<T>(Exception e)
		{
			throw e;
		}
	}
}
