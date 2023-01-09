using System;
using System.Collections;
using System.Collections.Generic;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E9 RID: 1001
	public class YieldWithTailOperation1<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001E17 RID: 7703 RVA: 0x0007FD40 File Offset: 0x0007E140
		public YieldWithTailOperation1(IEnumerator head, Func<IEnumerator<TOut>> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0007FD50 File Offset: 0x0007E150
		public override object GetContinuation()
		{
			return this._continuation();
		}

		// Token: 0x040049F7 RID: 18935
		private readonly Func<IEnumerator<TOut>> _continuation;
	}
}
