using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DE RID: 990
	public class ContextOperation<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001DEA RID: 7658 RVA: 0x0007F83C File Offset: 0x0007DC3C
		public ContextOperation(IEnumerator head, Action<EnumerationContext> contextAction)
		{
			IYieldOperation yieldOperation = head as IYieldOperation;
			this._context = ((yieldOperation == null) ? EnumerationContext.WithHead(head) : yieldOperation.Context);
			contextAction(this._context);
			this._context.Add(this);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0007F88B File Offset: 0x0007DC8B
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}
	}
}
