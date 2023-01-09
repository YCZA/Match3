using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Match3.Scripts1.Wooga.Signals
{
	// Token: 0x02000B6E RID: 2926
	public class AwaitSignal : CustomYieldInstruction
	{
		// Token: 0x06004466 RID: 17510 RVA: 0x0015C7C6 File Offset: 0x0015ABC6
		public AwaitSignal()
		{
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0015C7CE File Offset: 0x0015ABCE
		public AwaitSignal(Signal signal)
		{
			signal.AddListenerOnce(new Action(this.Dispatch));
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0015C7E8 File Offset: 0x0015ABE8
		public AwaitSignal(UnityEvent ev)
		{
			AwaitSignal _0024this = this;
			ev.AddListener(delegate()
			{
				_0024this.HandleUnityEvent(ev);
			});
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06004469 RID: 17513 RVA: 0x0015C826 File Offset: 0x0015AC26
		// (set) Token: 0x0600446A RID: 17514 RVA: 0x0015C82E File Offset: 0x0015AC2E
		public bool WasDispatched { get; protected set; }

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x0600446B RID: 17515 RVA: 0x0015C837 File Offset: 0x0015AC37
		public override bool keepWaiting
		{
			get
			{
				return !this.WasDispatched;
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0015C844 File Offset: 0x0015AC44
		public void AddListener(Action callback)
		{
			if (this.listeners == null || !this.listeners.GetInvocationList().Contains(callback))
			{
				this.listeners = (Action)Delegate.Combine(this.listeners, callback);
			}
			if (this.WasDispatched)
			{
				callback();
			}
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0015C89A File Offset: 0x0015AC9A
		public static AwaitSignal WaitFor(Signal signal)
		{
			return new AwaitSignal(signal);
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0015C8A2 File Offset: 0x0015ACA2
		public static AwaitSignal WaitFor(UnityEvent ev)
		{
			return new AwaitSignal(ev);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0015C8AA File Offset: 0x0015ACAA
		public void Dispatch()
		{
			this.WasDispatched = true;
			if (this.listeners != null)
			{
				this.listeners();
				this.listeners = null;
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0015C8D0 File Offset: 0x0015ACD0
		public void Clear()
		{
			this.listeners = null;
			this.WasDispatched = false;
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0015C8E0 File Offset: 0x0015ACE0
		private void HandleUnityEvent(UnityEvent ev)
		{
			ev.RemoveListener(new UnityAction(this.Dispatch));
			this.Dispatch();
		}

		// Token: 0x04006C78 RID: 27768
		private Action listeners;
	}
}
