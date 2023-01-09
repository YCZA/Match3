using System.Collections.Generic;

namespace Match3.Scripts1
{
	// Token: 0x020003F0 RID: 1008
	internal struct Breadcrumbs
	{
		// Token: 0x06001E3C RID: 7740 RVA: 0x0008054B File Offset: 0x0007E94B
		public Breadcrumbs(int size)
		{
			this._size = size;
			this._queue = new Queue<string>(size);
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x00080560 File Offset: 0x0007E960
		public void Append(string crumb)
		{
			object queue = this._queue;
			lock (queue)
			{
				while (this._queue.Count >= this._size)
				{
					this._queue.Dequeue();
				}
				this._queue.Enqueue(crumb);
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000805CC File Offset: 0x0007E9CC
		public string[] ToArray()
		{
			object queue = this._queue;
			string[] result;
			lock (queue)
			{
				result = this._queue.ToArray();
			}
			return result;
		}

		// Token: 0x04004A14 RID: 18964
		private readonly Queue<string> _queue;

		// Token: 0x04004A15 RID: 18965
		private readonly int _size;
	}
}
