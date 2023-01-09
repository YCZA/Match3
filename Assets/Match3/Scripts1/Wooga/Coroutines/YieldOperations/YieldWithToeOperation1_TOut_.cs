using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003EC RID: 1004
	public class YieldWithToeOperation1<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001E1D RID: 7709 RVA: 0x0007FE1E File Offset: 0x0007E21E
		public YieldWithToeOperation1(IEnumerator head, Func<TOut> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0007FE2E File Offset: 0x0007E22E
		public override object GetContinuation()
		{
			return this._continuation();
		}

		// Token: 0x040049FA RID: 18938
		private readonly Func<TOut> _continuation;
	}
}
