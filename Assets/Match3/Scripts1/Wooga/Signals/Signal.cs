using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B74 RID: 2932
	public class Signal
	{
		// Token: 0x0600448D RID: 17549 RVA: 0x0015CBD0 File Offset: 0x0015AFD0
		public void Dispatch()
		{
			if (this.listeners != null)
			{
				foreach (Action action in this.listeners)
				{
					if (action != null)
					{
						action();
					}
				}
			}
			if (this.onceListeners != null)
			{
				List<Action> list = this.onceListeners;
				this.onceListeners = null;
				while (list.Count > 0)
				{
					Action action2 = list.RemoveAndGetLast<Action>();
					if (action2 != null)
					{
						action2();
					}
				}
			}
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0015CC7C File Offset: 0x0015B07C
		public void AddListener(Action callback)
		{
			this.AddUnique(ref this.listeners, callback);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0015CC8B File Offset: 0x0015B08B
		public void AddListenerOnce(Action callback)
		{
			this.AddUnique(ref this.onceListeners, callback);
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0015CC9A File Offset: 0x0015B09A
		public void RemoveListener(Action callback)
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

		// Token: 0x06004491 RID: 17553 RVA: 0x0015CCCC File Offset: 0x0015B0CC
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

		// Token: 0x06004492 RID: 17554 RVA: 0x0015CCFA File Offset: 0x0015B0FA
		public bool HasListener()
		{
			return !this.listeners.IsNullOrEmptyCollection() || !this.onceListeners.IsNullOrEmptyCollection();
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0015CD1D File Offset: 0x0015B11D
		private void AddUnique(ref List<Action> listeners, Action callback)
		{
			if (listeners == null)
			{
				listeners = new List<Action>();
			}
			if (!listeners.Contains(callback))
			{
				listeners.Add(callback);
			}
		}

		// Token: 0x04006C7F RID: 27775
		private List<Action> listeners;

		// Token: 0x04006C80 RID: 27776
		private List<Action> onceListeners;
	}
}
