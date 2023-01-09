using System;
using System.Collections.Generic;

namespace Wooga.Core.Storage
{
	// Token: 0x02000395 RID: 917
	public class SbsContextLock
	{
		// Token: 0x06001BD7 RID: 7127 RVA: 0x0007AF20 File Offset: 0x00079320
		public void With(string context, Action func)
		{
			try
			{
				object @lock = this.GetLock(context);
				lock (@lock)
				{
					func();
				}
			}
			finally
			{
				this.ReturnLock(context);
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0007AF78 File Offset: 0x00079378
		public T With<T>(string context, Func<T> func)
		{
			T result;
			try
			{
				object @lock = this.GetLock(context);
				lock (@lock)
				{
					result = func();
				}
			}
			finally
			{
				this.ReturnLock(context);
			}
			return result;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0007AFD0 File Offset: 0x000793D0
		public object GetLock(string context)
		{
			SbsContextLock.LockObject lockObject = null;
			object obj = this.contextLocks;
			lock (obj)
			{
				if (!this.contextLocks.ContainsKey(context))
				{
					this.contextLocks[context] = new SbsContextLock.LockObject
					{
						locker = new object(),
						count = 0
					};
				}
				lockObject = this.contextLocks[context];
				lockObject.count++;
			}
			return lockObject.locker;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0007B068 File Offset: 0x00079468
		public void ReturnLock(string context)
		{
			object obj = this.contextLocks;
			lock (obj)
			{
				if (this.contextLocks.ContainsKey(context))
				{
					SbsContextLock.LockObject lockObject = this.contextLocks[context];
					lockObject.count--;
					if (lockObject.count < 1)
					{
						this.contextLocks.Remove(context);
					}
				}
			}
		}

		// Token: 0x04004970 RID: 18800
		private readonly Dictionary<string, SbsContextLock.LockObject> contextLocks = new Dictionary<string, SbsContextLock.LockObject>();

		// Token: 0x02000396 RID: 918
		private class LockObject
		{
			// Token: 0x04004971 RID: 18801
			public object locker;

			// Token: 0x04004972 RID: 18802
			public volatile int count;
		}
	}
}
