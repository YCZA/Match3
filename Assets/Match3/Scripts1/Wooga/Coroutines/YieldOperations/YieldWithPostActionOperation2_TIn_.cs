using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E7 RID: 999
	public class YieldWithPostActionOperation2<TIn> : AYieldOperation<object>
	{
		// Token: 0x06001E13 RID: 7699 RVA: 0x0007FCC1 File Offset: 0x0007E0C1
		public YieldWithPostActionOperation2(IEnumerator head, Action<TIn> continuation) : base(head)
		{
			this._continuation = continuation;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0007FCD4 File Offset: 0x0007E0D4
		public override object GetContinuation()
		{
			TIn obj;
			if (base.Context.Value is TIn)
			{
				obj = (TIn)((object)base.Context.Value);
			}
			else
			{
				obj = default(TIn);
			}
			this._continuation(obj);
			return null;
		}

		// Token: 0x040049F5 RID: 18933
		private readonly Action<TIn> _continuation;
	}
}
