using System;
using System.Collections;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DC RID: 988
	public class CatchWithToeOperation<ExceptionT, TOut> : AYieldOperation<TOut> where ExceptionT : Exception
	{
		// Token: 0x06001DE2 RID: 7650 RVA: 0x0007F761 File Offset: 0x0007DB61
		public CatchWithToeOperation(IEnumerator head, Func<ExceptionT, TOut> handler) : base(head)
		{
			this._handler = handler;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0007F771 File Offset: 0x0007DB71
		public override bool HandlesException(Exception e)
		{
			return e is ExceptionT;
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0007F77C File Offset: 0x0007DB7C
		public override object HandleException(Exception e)
		{
			Func<ExceptionT, TOut> handler = this._handler;
			ExceptionT exceptionT = e as ExceptionT;
			if (handler != null && exceptionT != null)
			{
				this._handler = null;
				return handler(exceptionT);
			}
			return ExceptionUtils.RethrowException(e);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0007F7C7 File Offset: 0x0007DBC7
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}

		// Token: 0x040049E4 RID: 18916
		private Func<ExceptionT, TOut> _handler;
	}
}
