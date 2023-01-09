using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E6 RID: 998
	public class YieldWithPostActionOperation1 : AYieldOperation<object>
	{
		// Token: 0x06001E11 RID: 7697 RVA: 0x0007FCA3 File Offset: 0x0007E0A3
		public YieldWithPostActionOperation1(IEnumerator head, Action continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0007FCB3 File Offset: 0x0007E0B3
		public override object GetContinuation()
		{
			this._continuation();
			return null;
		}

		// Token: 0x040049F4 RID: 18932
		private readonly Action _continuation;
	}
}
