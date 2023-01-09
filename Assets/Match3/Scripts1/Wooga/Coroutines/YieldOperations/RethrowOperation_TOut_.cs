using System;
using System.Collections;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E2 RID: 994
	public class RethrowOperation<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001DF9 RID: 7673 RVA: 0x0007FC36 File Offset: 0x0007E036
		public RethrowOperation(IEnumerator head, Action<Exception> catchBlock) : base(head)
		{
			this._catchBlock = catchBlock;
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0007FC46 File Offset: 0x0007E046
		public override bool HandlesException(Exception e)
		{
			return true;
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0007FC49 File Offset: 0x0007E049
		public override object HandleException(Exception e)
		{
			this._exception = e;
			if (this._catchBlock != null)
			{
				this._catchBlock(e);
			}
			return ExceptionUtils.RethrowException(e);
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0007FC6F File Offset: 0x0007E06F
		public override object GetContinuation()
		{
			if (this._exception != null)
			{
				throw this._exception;
			}
			return Nothing.AtAll;
		}

		// Token: 0x040049F0 RID: 18928
		private readonly Action<Exception> _catchBlock;

		// Token: 0x040049F1 RID: 18929
		private Exception _exception;
	}
}
