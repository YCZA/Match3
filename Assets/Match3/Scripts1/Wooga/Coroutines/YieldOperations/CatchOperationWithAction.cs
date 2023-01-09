using System;
using System.Collections;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DB RID: 987
	public class CatchOperationWithAction : AYieldOperation<object>
	{
		// Token: 0x06001DDE RID: 7646 RVA: 0x0007F71B File Offset: 0x0007DB1B
		public CatchOperationWithAction(IEnumerator head, Action<Exception> catchBlock) : base(head)
		{
			this._catchBlock = catchBlock;
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0007F72B File Offset: 0x0007DB2B
		public override bool HandlesException(Exception e)
		{
			return true;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0007F72E File Offset: 0x0007DB2E
		public override object HandleException(Exception e)
		{
			if (this._catchBlock != null)
			{
				this._catchBlock(e);
				this._catchBlock = null;
				return Nothing.AtAll;
			}
			return ExceptionUtils.RethrowException(e);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0007F75A File Offset: 0x0007DB5A
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}

		// Token: 0x040049E3 RID: 18915
		private Action<Exception> _catchBlock;
	}
}
