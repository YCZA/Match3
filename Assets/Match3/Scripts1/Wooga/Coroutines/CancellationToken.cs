using System;

namespace Wooga.Coroutines
{
	// Token: 0x020003C5 RID: 965
	public class CancellationToken : ICancellationToken
	{
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001D16 RID: 7446 RVA: 0x0007DC88 File Offset: 0x0007C088
		// (remove) Token: 0x06001D17 RID: 7447 RVA: 0x0007DCC0 File Offset: 0x0007C0C0
		////[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<CancellationToken> Canceled;



		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0007DCF6 File Offset: 0x0007C0F6
		public virtual bool IsCanceled
		{
			get
			{
				return this._canceled;
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x0007DCFE File Offset: 0x0007C0FE
		public void Cancel()
		{
			if (!this._canceled)
			{
				this._canceled = true;
				this.Canceled(this);
				this.Canceled = null;
			}
		}

		// Token: 0x040049BF RID: 18879
		private bool _canceled;
	}
}
