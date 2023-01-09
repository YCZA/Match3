using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E8 RID: 1000
	public class YieldWithTailOperation0 : AYieldOperation<object>
	{
		// Token: 0x06001E15 RID: 7701 RVA: 0x0007FD23 File Offset: 0x0007E123
		public YieldWithTailOperation0(IEnumerator head, Func<IEnumerator> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0007FD33 File Offset: 0x0007E133
		public override object GetContinuation()
		{
			return this._continuation();
		}

		// Token: 0x040049F6 RID: 18934
		private readonly Func<IEnumerator> _continuation;
	}
}
