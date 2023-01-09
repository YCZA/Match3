using System;
using System.Collections;
using System.Collections.Generic;

namespace Wooga.Coroutines
{
	// Token: 0x020003D4 RID: 980
	public class PoolableListEnumerator<T> : IEnumerator<T>, IPoolable, IEnumerator, IDisposable
	{
		// Token: 0x06001DAA RID: 7594 RVA: 0x0007F0E1 File Offset: 0x0007D4E1
		public void Init(IList<T> list)
		{
			this._list = list;
			this._index = -1;
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0007F0F4 File Offset: 0x0007D4F4
		public bool MoveNext()
		{
			return ++this._index < this._list.Count;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0007F11F File Offset: 0x0007D51F
		public void Reset()
		{
			this._index = -1;
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0007F128 File Offset: 0x0007D528
		public object Current
		{
			get
			{
				return this._list[this._index];
			}
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0007F140 File Offset: 0x0007D540
		public void Dispose()
		{
			this.Release();
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0007F148 File Offset: 0x0007D548
		T IEnumerator<T>.Current
		{
			get
			{
				T result = default(T);
				if (this.Current is T)
				{
					result = (T)((object)this.Current);
				}
				return result;
			}
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0007F17C File Offset: 0x0007D57C
		public void Release()
		{
			this._index = -1;
		}

		// Token: 0x040049DC RID: 18908
		private IList<T> _list;

		// Token: 0x040049DD RID: 18909
		private int _index;
	}
}
