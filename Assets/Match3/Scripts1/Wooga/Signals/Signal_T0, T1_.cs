using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B76 RID: 2934
	public class Signal<T0, T1>
	{
		// Token: 0x0600449D RID: 17565 RVA: 0x0015CEC8 File Offset: 0x0015B2C8
		public void Dispatch(T0 val1, T1 val2)
		{
			if (this.listeners != null)
			{
				foreach (Action<T0, T1> action in this.listeners)
				{
					if (action != null)
					{
						action(val1, val2);
					}
				}
			}
			if (this.onceListeners != null)
			{
				List<Action<T0, T1>> list = this.onceListeners;
				this.onceListeners = null;
				while (list.Count > 0)
				{
					Action<T0, T1> action2 = list.RemoveAndGetLast<Action<T0, T1>>();
					if (action2 != null)
					{
						action2(val1, val2);
					}
				}
			}
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0015CF78 File Offset: 0x0015B378
		public void AddListener(Action<T0, T1> callback)
		{
			this.AddUnique(ref this.listeners, callback);
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x0015CF87 File Offset: 0x0015B387
		public void AddListenerOnce(Action<T0, T1> callback)
		{
			this.AddUnique(ref this.onceListeners, callback);
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x0015CF96 File Offset: 0x0015B396
		public void RemoveListener(Action<T0, T1> callback)
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

		// Token: 0x060044A1 RID: 17569 RVA: 0x0015CFC8 File Offset: 0x0015B3C8
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

		// Token: 0x060044A2 RID: 17570 RVA: 0x0015CFF6 File Offset: 0x0015B3F6
		public bool HasListener()
		{
			return !this.listeners.IsNullOrEmptyCollection() || !this.onceListeners.IsNullOrEmptyCollection();
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0015D019 File Offset: 0x0015B419
		private void AddUnique(ref List<Action<T0, T1>> listeners, Action<T0, T1> callback)
		{
			if (listeners == null)
			{
				listeners = new List<Action<T0, T1>>();
			}
			if (!listeners.Contains(callback))
			{
				listeners.Add(callback);
			}
		}

		// Token: 0x04006C83 RID: 27779
		private List<Action<T0, T1>> listeners;

		// Token: 0x04006C84 RID: 27780
		private List<Action<T0, T1>> onceListeners;
	}
}
