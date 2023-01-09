using System;
using System.Linq;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B6F RID: 2927
	public class AwaitSignal<T> : AwaitSignal
	{
		// Token: 0x06004472 RID: 17522 RVA: 0x0015C915 File Offset: 0x0015AD15
		public AwaitSignal()
		{
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0015C91D File Offset: 0x0015AD1D
		public AwaitSignal(Signal<T> signal)
		{
			signal.AddListenerOnce(new Action<T>(this.Dispatch));
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06004474 RID: 17524 RVA: 0x0015C937 File Offset: 0x0015AD37
		// (set) Token: 0x06004475 RID: 17525 RVA: 0x0015C93F File Offset: 0x0015AD3F
		public T Dispatched { get; protected set; }

		// Token: 0x06004476 RID: 17526 RVA: 0x0015C948 File Offset: 0x0015AD48
		public static AwaitSignal<T> WaitFor(Signal<T> signal)
		{
			return new AwaitSignal<T>(signal);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0015C950 File Offset: 0x0015AD50
		public void AddListener(Action<T> callback)
		{
			if (base.WasDispatched)
			{
				callback(this.Dispatched);
			}
			else if (this.listeners == null || !this.listeners.GetInvocationList().Contains(callback))
			{
				this.listeners = (Action<T>)Delegate.Combine(this.listeners, callback);
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0015C9B1 File Offset: 0x0015ADB1
		public void Dispatch(T value)
		{
			this.Dispatched = value;
			base.WasDispatched = true;
			if (this.listeners != null)
			{
				this.listeners(value);
				this.listeners = null;
			}
		}

		// Token: 0x04006C79 RID: 27769
		private Action<T> listeners;
	}
}
