using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1;

namespace Wooga.Coroutines
{
	// Token: 0x020003D5 RID: 981
	public static class OptionalResultExtensions
	{
		// Token: 0x06001DB1 RID: 7601 RVA: 0x0007F185 File Offset: 0x0007D585
		public static OptionalResult<IEnumerator<T>> ToOption<T>(this IEnumerator<T> enumerator)
		{
			return OptionalResult.Some<IEnumerator<T>>(enumerator);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0007F18D File Offset: 0x0007D58D
		public static OptionalResult<IEnumerator> ToOption(this IEnumerator enumerator)
		{
			return OptionalResult.Some<IEnumerator>(enumerator);
		}
	}
}
