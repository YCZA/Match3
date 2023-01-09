using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003ED RID: 1005
	public class YieldWithToeOperation2<TIn, TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001E1F RID: 7711 RVA: 0x0007FE40 File Offset: 0x0007E240
		public YieldWithToeOperation2(IEnumerator head, Func<TIn, TOut> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0007FE50 File Offset: 0x0007E250
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

		// Token: 0x040049FB RID: 18939
		private readonly Func<TIn, TOut> _continuation;
	}
}
