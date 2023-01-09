using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Wooga.Core.Concurrent
{
	// Token: 0x02000346 RID: 838
	public class ConcurrentSet<T> : ICollection<T>, IEnumerable, IEnumerable<T>
	{
		// Token: 0x060019A4 RID: 6564 RVA: 0x00074079 File Offset: 0x00072479
		public ConcurrentSet()
		{
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00074098 File Offset: 0x00072498
		public ConcurrentSet(IEnumerable<T> enumerable)
		{
			foreach (T item in enumerable)
			{
				this.Set.Add(item);
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00074110 File Offset: 0x00072510
		public IEnumerator<T> GetEnumerator()
		{
			IEnumerator<T> result;
			try
			{
				this.rwLock.EnterWriteLock();
				result = new HashSet<T>(this.Set).GetEnumerator();
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
			return result;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00074160 File Offset: 0x00072560
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00074168 File Offset: 0x00072568
		public void Add(T item)
		{
			try
			{
				this.rwLock.EnterWriteLock();
				this.Set.Add(item);
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x000741B0 File Offset: 0x000725B0
		public void Clear()
		{
			try
			{
				this.rwLock.EnterWriteLock();
				this.Set.Clear();
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x000741F4 File Offset: 0x000725F4
		public bool Contains(T item)
		{
			bool result;
			try
			{
				this.rwLock.EnterReadLock();
				result = this.Set.Contains(item);
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0007423C File Offset: 0x0007263C
		public void CopyTo(T[] array, int arrayIndex)
		{
			try
			{
				this.rwLock.EnterReadLock();
				this.Set.CopyTo(array, arrayIndex);
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00074284 File Offset: 0x00072684
		public bool Remove(T item)
		{
			bool result;
			try
			{
				this.rwLock.EnterWriteLock();
				result = this.Set.Remove(item);
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
			return result;
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060019AD RID: 6573 RVA: 0x000742CC File Offset: 0x000726CC
		public int Count
		{
			get
			{
				int count;
				try
				{
					this.rwLock.EnterReadLock();
					count = this.Set.Count;
				}
				finally
				{
					this.rwLock.ExitReadLock();
				}
				return count;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x00074314 File Offset: 0x00072714
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400483B RID: 18491
		private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

		// Token: 0x0400483C RID: 18492
		private readonly HashSet<T> Set = new HashSet<T>();
	}
}
