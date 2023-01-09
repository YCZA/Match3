using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003EB RID: 1003
	public class YieldWithTailOperation3<TIn> : AYieldOperation<object>
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x0007FDBE File Offset: 0x0007E1BE
		public YieldWithTailOperation3(IEnumerator head, Func<TIn, IEnumerator> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0007FDD0 File Offset: 0x0007E1D0
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

		// Token: 0x040049F9 RID: 18937
		private readonly Func<TIn, IEnumerator> _continuation;
	}
}
