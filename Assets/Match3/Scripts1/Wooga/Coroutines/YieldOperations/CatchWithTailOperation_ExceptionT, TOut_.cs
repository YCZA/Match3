using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DD RID: 989
	public class CatchWithTailOperation<ExceptionT, TOut> : AYieldOperation<TOut> where ExceptionT : Exception
	{
		// Token: 0x06001DE6 RID: 7654 RVA: 0x0007F7CE File Offset: 0x0007DBCE
		public CatchWithTailOperation(IEnumerator head, Func<ExceptionT, IEnumerator<TOut>> handler) : base(head)
		{
			this._handler = handler;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0007F7DE File Offset: 0x0007DBDE
		public override bool HandlesException(Exception e)
		{
			return e is ExceptionT;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0007F7EC File Offset: 0x0007DBEC
		public override object HandleException(Exception e)
		{
			Func<ExceptionT, IEnumerator<TOut>> handler = this._handler;
			ExceptionT exceptionT = e as ExceptionT;
			if (handler != null && exceptionT != null)
			{
				this._handler = null;
				return handler(exceptionT);
			}
			return ExceptionUtils.RethrowException(e);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0007F832 File Offset: 0x0007DC32
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}

		// Token: 0x040049E5 RID: 18917
		private Func<ExceptionT, IEnumerator<TOut>> _handler;
	}
}
