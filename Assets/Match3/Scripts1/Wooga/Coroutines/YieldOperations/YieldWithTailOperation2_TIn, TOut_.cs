using System;
using System.Collections;
using System.Collections.Generic;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003EA RID: 1002
	public class YieldWithTailOperation2<TIn, TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001E19 RID: 7705 RVA: 0x0007FD5D File Offset: 0x0007E15D
		public YieldWithTailOperation2(IEnumerator head, Func<TIn, IEnumerator<TOut>> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0007FD70 File Offset: 0x0007E170
		public override object GetContinuation()
		{
			TIn arg;
			if (base.Context.Value is TIn)
			{
				arg = (TIn)((object)base.Context.Value);
			}
			else
			{
				arg = default(TIn);
			}
			return this._continuation(arg);
		}

		// Token: 0x040049F8 RID: 18936
		private readonly Func<TIn, IEnumerator<TOut>> _continuation;
	}
}
