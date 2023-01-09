using System;
using System.Collections;
using System.Collections.Generic;

namespace Wooga.Coroutines
{
	// Token: 0x020003CA RID: 970
	public static class Enumerators
	{
		// Token: 0x06001D6F RID: 7535 RVA: 0x0007E877 File Offset: 0x0007CC77
		public static IEnumerator Empty()
		{
			return EmptyEnumerator.Instance;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0007E87E File Offset: 0x0007CC7E
		public static IEnumerator<T> Yield<T>(Func<T> factory)
		{
			return factory.Yield<T>();
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0007E886 File Offset: 0x0007CC86
		public static IEnumerator Yield(Action action)
		{
			return action.Yield();
		}
	}
}
