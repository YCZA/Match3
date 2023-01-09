using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B75 RID: 2933
	public class Signal<T>
	{
		// Token: 0x06004495 RID: 17557 RVA: 0x0015CD4C File Offset: 0x0015B14C
		public void Dispatch(T value)
		{
			if (this.listeners != null)
			{
				foreach (Action<T> action in this.listeners)
				{
					if (action != null)
					{
						action(value);
					}
				}
			}
			if (this.onceListeners != null)
			{
				List<Action<T>> list = this.onceListeners;
				this.onceListeners = null;
				while (list.Count > 0)
				{
					Action<T> action2 = list.RemoveAndGetLast<Action<T>>();
					if (action2 != null)
					{
						action2(value);
					}
				}
			}
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0015CDF8 File Offset: 0x0015B1F8
		public void AddListener(Action<T> callback)
		{
			this.AddUnique(ref this.listeners, callback);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0015CE07 File Offset: 0x0015B207
		public void AddListenerOnce(Action<T> callback)
		{
			this.AddUnique(ref this.onceListeners, callback);
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0015CE16 File Offset: 0x0015B216
		public void RemoveListener(Action<T> callback)
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

		// Token: 0x06004499 RID: 17561 RVA: 0x0015CE48 File Offset: 0x0015B248
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

		// Token: 0x0600449A RID: 17562 RVA: 0x0015CE76 File Offset: 0x0015B276
		public bool HasListener()
		{
			return !this.listeners.IsNullOrEmptyCollection() || !this.onceListeners.IsNullOrEmptyCollection();
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0015CE99 File Offset: 0x0015B299
		private void AddUnique(ref List<Action<T>> listeners, Action<T> callback)
		{
			if (listeners == null)
			{
				listeners = new List<Action<T>>();
			}
			if (!listeners.Contains(callback))
			{
				listeners.Add(callback);
			}
		}

		// Token: 0x04006C81 RID: 27777
		private List<Action<T>> listeners;

		// Token: 0x04006C82 RID: 27778
		private List<Action<T>> onceListeners;
	}
}
