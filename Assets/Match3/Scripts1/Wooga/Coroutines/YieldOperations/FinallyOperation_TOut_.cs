using System;
using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E0 RID: 992
	public class FinallyOperation<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001DF4 RID: 7668 RVA: 0x0007FBE9 File Offset: 0x0007DFE9
		public FinallyOperation(IEnumerator head, Action finallyBlock) : base(head)
		{
			this._finallyBlock = finallyBlock;
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0007FBF9 File Offset: 0x0007DFF9
		public override void Finally()
		{
			if (this._finallyBlock != null)
			{
				this._finallyBlock();
				this._finallyBlock = null;
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0007FC18 File Offset: 0x0007E018
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}

		// Token: 0x040049EE RID: 18926
		private Action _finallyBlock;
	}
}
