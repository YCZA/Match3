using System;
using System.Collections;
using System.Collections.Generic;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E5 RID: 997
	public abstract class AYieldOperation<TOut> : IYieldOperation, IEnumerator<TOut>, IEnumerator, IDisposable
	{
		// Token: 0x06001E05 RID: 7685 RVA: 0x0007F648 File Offset: 0x0007DA48
		protected AYieldOperation()
		{
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0007F650 File Offset: 0x0007DA50
		protected AYieldOperation(IEnumerator head)
		{
			IYieldOperation yieldOperation = head as IYieldOperation;
			this._context = ((yieldOperation == null) ? EnumerationContext.WithHead(head) : yieldOperation.Context);
			this._context.Add(this);
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0007F693 File Offset: 0x0007DA93
		public EnumerationContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0007F69B File Offset: 0x0007DA9B
		public object Current
		{
			get
			{
				return this._context.Value;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0007F6A8 File Offset: 0x0007DAA8
		TOut IEnumerator<TOut>.Current
		{
			get
			{
				return (!(this._context.Value is TOut)) ? default(TOut) : ((TOut)((object)this._context.Value));
			}
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0007F6E8 File Offset: 0x0007DAE8
		public bool MoveNext()
		{
			return this._context.MoveNext();
		}

		// Token: 0x06001E0B RID: 7691
		public abstract object GetContinuation();

		// Token: 0x06001E0C RID: 7692 RVA: 0x0007F6F5 File Offset: 0x0007DAF5
		public virtual bool HandlesException(Exception e)
		{
			return false;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0007F6F8 File Offset: 0x0007DAF8
		public virtual object HandleException(Exception e)
		{
			return ExceptionUtils.RethrowException(e);
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0007F700 File Offset: 0x0007DB00
		public virtual void Finally()
		{
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0007F702 File Offset: 0x0007DB02
		public void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0007F709 File Offset: 0x0007DB09
		public void Dispose()
		{
		}

		// Token: 0x040049F3 RID: 18931
		protected EnumerationContext _context;
	}
}
