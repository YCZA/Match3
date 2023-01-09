namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000476 RID: 1142
	internal class AtomicCounter
	{
		// Token: 0x06002108 RID: 8456 RVA: 0x0008B231 File Offset: 0x00089631
		public AtomicCounter()
		{
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0008B239 File Offset: 0x00089639
		public AtomicCounter(int count)
		{
			this._count = count;
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x0008B248 File Offset: 0x00089648
		public int Count
		{
			get
			{
				int count;
				lock (this)
				{
					count = this._count;
				}
				return count;
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0008B284 File Offset: 0x00089684
		public void Increment()
		{
			lock (this)
			{
				this._count++;
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0008B2C4 File Offset: 0x000896C4
		public void Decrement()
		{
			lock (this)
			{
				this._count--;
			}
		}

		// Token: 0x04004BB5 RID: 19381
		private int _count;
	}
}
