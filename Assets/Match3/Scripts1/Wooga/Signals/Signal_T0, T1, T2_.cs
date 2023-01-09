using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B77 RID: 2935
	public class Signal<T0, T1, T2>
	{
		// Token: 0x060044A5 RID: 17573 RVA: 0x0015D048 File Offset: 0x0015B448
		public void Dispatch(T0 val1, T1 val2, T2 val3)
		{
			if (this.listeners != null)
			{
				foreach (Action<T0, T1, T2> action in this.listeners)
				{
					if (action != null)
					{
						action(val1, val2, val3);
					}
				}
			}
			if (this.onceListeners != null)
			{
				List<Action<T0, T1, T2>> list = this.onceListeners;
				this.onceListeners = null;
				while (list.Count > 0)
				{
					Action<T0, T1, T2> action2 = list.RemoveAndGetLast<Action<T0, T1, T2>>();
					if (action2 != null)
					{
						action2(val1, val2, val3);
					}
				}
			}
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x0015D0F8 File Offset: 0x0015B4F8
		public void AddListener(Action<T0, T1, T2> callback)
		{
			this.AddUnique(ref this.listeners, callback);
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x0015D107 File Offset: 0x0015B507
		public void AddListenerOnce(Action<T0, T1, T2> callback)
		{
			this.AddUnique(ref this.onceListeners, callback);
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x0015D116 File Offset: 0x0015B516
		public void RemoveListener(Action<T0, T1, T2> callback)
		{
			if (this.listeners != null)
			{
				this.listeners.Remove(callback);
			}
			if (this.onceListeners != null)
			{
				this.onceListeners.Remove(callback);
			}
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x0015D148 File Offset: 0x0015B548
		public void RemoveAllListeners()
		{
			if (this.listeners != null)
			{
				this.listeners.Clear();
			}
			if (this.onceListeners != null)
			{
				this.onceListeners.Clear();
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0015D176 File Offset: 0x0015B576
		public bool HasListener()
		{
			return !this.listeners.IsNullOrEmptyCollection() || !this.onceListeners.IsNullOrEmptyCollection();
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0015D199 File Offset: 0x0015B599
		private void AddUnique(ref List<Action<T0, T1, T2>> listeners, Action<T0, T1, T2> callback)
		{
			if (listeners == null)
			{
				listeners = new List<Action<T0, T1, T2>>();
			}
			if (!listeners.Contains(callback))
			{
				listeners.Add(callback);
			}
		}

		// Token: 0x04006C85 RID: 27781
		private List<Action<T0, T1, T2>> listeners;

		// Token: 0x04006C86 RID: 27782
		private List<Action<T0, T1, T2>> onceListeners;
	}
}
