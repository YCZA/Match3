using System;
using System.Collections;
using Wooga.Core.Utilities;

namespace Wooga.Coroutines
{
	// Token: 0x020003CC RID: 972
	public abstract class Future : IEnumerator, IDisposable
	{
		// Token: 0x06001D78 RID: 7544 RVA: 0x0007E8B2 File Offset: 0x0007CCB2
		public static Future<T> FromResult<T>(T result)
		{
			return new Future<T>(result);
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x0007E8BA File Offset: 0x0007CCBA
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x0007E8C1 File Offset: 0x0007CCC1
		public static Action<Exception> DefaultExceptionHandler
		{
			get
			{
				return Future._defaultExceptionHandler;
			}
			set
			{
				Future._defaultExceptionHandler = value;
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0007E8C9 File Offset: 0x0007CCC9
		protected virtual void OnFaulted(Exception ex)
		{
			this._faulted(ex);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0007E8D7 File Offset: 0x0007CCD7
		protected void SetErrorHandler(Action<Exception> errorHandler)
		{
			this._faulted = (errorHandler ?? Future.DefaultExceptionHandler);
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0007E8EC File Offset: 0x0007CCEC
		protected virtual void Cleanup()
		{
			this._faulted = null;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0007E8F5 File Offset: 0x0007CCF5
		public void Dispose()
		{
			this.Cancel();
		}

		// Token: 0x06001D7F RID: 7551
		public abstract bool MoveNext();

		// Token: 0x06001D80 RID: 7552
		public abstract void Reset();

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001D81 RID: 7553
		public abstract object Current { get; }

		// Token: 0x06001D82 RID: 7554
		public abstract void Cancel();

		// Token: 0x040049C9 RID: 18889
		private static Action<Exception> _defaultExceptionHandler = delegate(Exception e)
		{
			global::UnityEngine.Debug.LogException(e);
			ExceptionUtils.RethrowException(e);
		};

		// Token: 0x040049CA RID: 18890
		public static bool UseProbing = false;

		// Token: 0x040049CB RID: 18891
		protected Action<Exception> _faulted;
	}
}
