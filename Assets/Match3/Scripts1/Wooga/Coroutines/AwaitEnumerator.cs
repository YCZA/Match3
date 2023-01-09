using System.Collections;

namespace Wooga.Coroutines
{
	// Token: 0x020003C1 RID: 961
	public class AwaitEnumerator : IEnumerator
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x0007DBA7 File Offset: 0x0007BFA7
		// (set) Token: 0x06001D08 RID: 7432 RVA: 0x0007DBAF File Offset: 0x0007BFAF
		public bool IsSignaled { get; private set; }

		// Token: 0x06001D09 RID: 7433 RVA: 0x0007DBB8 File Offset: 0x0007BFB8
		public bool MoveNext()
		{
			return !this.IsSignaled;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x0007DBC3 File Offset: 0x0007BFC3
		public virtual void Reset()
		{
			this.IsSignaled = false;
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x0007DBCC File Offset: 0x0007BFCC
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x0007DBCF File Offset: 0x0007BFCF
		public void Signal()
		{
			this.IsSignaled = true;
		}
	}
}
