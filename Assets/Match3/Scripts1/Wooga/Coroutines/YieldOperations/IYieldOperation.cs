using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E3 RID: 995
	public interface IYieldOperation : IEnumerator
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001DFD RID: 7677
		EnumerationContext Context { get; }

		// Token: 0x06001DFE RID: 7678
		object GetContinuation();

		// Token: 0x06001DFF RID: 7679
		bool HandlesException(Exception e);

		// Token: 0x06001E00 RID: 7680
		object HandleException(Exception e);

		// Token: 0x06001E01 RID: 7681
		void Finally();
	}
}
