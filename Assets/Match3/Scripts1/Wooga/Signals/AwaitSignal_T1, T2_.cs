using System;
using System.Linq;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B70 RID: 2928
	public class AwaitSignal<T1, T2> : AwaitSignal<T1>
	{
		// Token: 0x06004479 RID: 17529 RVA: 0x0015C9DF File Offset: 0x0015ADDF
		public AwaitSignal()
		{
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0015C9E7 File Offset: 0x0015ADE7
		public AwaitSignal(Signal<T1, T2> signal)
		{
			signal.AddListenerOnce(new Action<T1, T2>(this.Dispatch));
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x0600447B RID: 17531 RVA: 0x0015CA01 File Offset: 0x0015AE01
		// (set) Token: 0x0600447C RID: 17532 RVA: 0x0015CA09 File Offset: 0x0015AE09
		public T2 Dispatched2 { get; protected set; }

		// Token: 0x0600447D RID: 17533 RVA: 0x0015CA12 File Offset: 0x0015AE12
		public static AwaitSignal<T1, T2> WaitFor(Signal<T1, T2> signal)
		{
			return new AwaitSignal<T1, T2>(signal);
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x0015CA1C File Offset: 0x0015AE1C
		public void AddListener(Action<T1, T2> callback)
		{
			if (base.WasDispatched)
			{
				callback(base.Dispatched, this.Dispatched2);
			}
			else if (this.listeners == null || !this.listeners.GetInvocationList().Contains(callback))
			{
				this.listeners = (Action<T1, T2>)Delegate.Combine(this.listeners, callback);
			}
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x0015CA83 File Offset: 0x0015AE83
		public void Dispatch(T1 val1, T2 val2)
		{
			base.Dispatched = val1;
			this.Dispatched2 = val2;
			base.WasDispatched = true;
			if (this.listeners != null)
			{
				this.listeners(val1, val2);
				this.listeners = null;
			}
		}

		// Token: 0x04006C7B RID: 27771
		private Action<T1, T2> listeners;
	}
}
