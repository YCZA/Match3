using System;
using System.Collections;
using System.Collections.Generic;

namespace Wooga.Coroutines
{
	// Token: 0x020003C2 RID: 962
	public class AwaitEnumerator<T> : AwaitEnumerator, IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x06001D0E RID: 7438 RVA: 0x0007DBE0 File Offset: 0x0007BFE0
		public void Dispose()
		{
			IDisposable disposable = this._value as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x0007DC0C File Offset: 0x0007C00C
		public override void Reset()
		{
			this._value = default(T);
			base.Reset();
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x0007DC2E File Offset: 0x0007C02E
		T IEnumerator<T>.Current
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x0007DC36 File Offset: 0x0007C036
		public void Signal(T value)
		{
			this._value = value;
			base.Signal();
		}

		// Token: 0x040049BD RID: 18877
		private T _value;
	}
}
