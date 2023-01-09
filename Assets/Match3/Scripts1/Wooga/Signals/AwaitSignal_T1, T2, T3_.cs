using System;
using System.Linq;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B71 RID: 2929
	public class AwaitSignal<T1, T2, T3> : AwaitSignal<T1, T2>
	{
		// Token: 0x06004480 RID: 17536 RVA: 0x0015CAB9 File Offset: 0x0015AEB9
		public AwaitSignal()
		{
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0015CAC1 File Offset: 0x0015AEC1
		public AwaitSignal(Signal<T1, T2, T3> signal)
		{
			signal.AddListenerOnce(new Action<T1, T2, T3>(this.Dispatch));
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x0015CADB File Offset: 0x0015AEDB
		// (set) Token: 0x06004483 RID: 17539 RVA: 0x0015CAE3 File Offset: 0x0015AEE3
		public T3 Dispatched3 { get; protected set; }

		// Token: 0x06004484 RID: 17540 RVA: 0x0015CAEC File Offset: 0x0015AEEC
		public static AwaitSignal<T1, T2, T3> WaitFor(Signal<T1, T2, T3> signal)
		{
			return new AwaitSignal<T1, T2, T3>(signal);
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0015CAF4 File Offset: 0x0015AEF4
		public void AddListener(Action<T1, T2, T3> callback)
		{
			if (base.WasDispatched)
			{
				callback(base.Dispatched, base.Dispatched2, this.Dispatched3);
			}
			else if (this.listeners == null || !this.listeners.GetInvocationList().Contains(callback))
			{
				this.listeners = (Action<T1, T2, T3>)Delegate.Combine(this.listeners, callback);
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0015CB61 File Offset: 0x0015AF61
		public void Dispatch(T1 val1, T2 val2, T3 val3)
		{
			base.Dispatched = val1;
			base.Dispatched2 = val2;
			this.Dispatched3 = val3;
			base.WasDispatched = true;
			if (this.listeners != null)
			{
				this.listeners(val1, val2, val3);
				this.listeners = null;
			}
		}

		// Token: 0x04006C7D RID: 27773
		private Action<T1, T2, T3> listeners;
	}
}
