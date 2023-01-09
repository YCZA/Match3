using System.Threading;

namespace Match3.Scripts1.Wooga.Core.ThreadSafe
{
	// Token: 0x0200039C RID: 924
	public static class ThreadExtensions
	{
		// Token: 0x06001C03 RID: 7171 RVA: 0x0007B961 File Offset: 0x00079D61
		public static bool IsMainThread(this Thread t)
		{
			return t.Equals(Unity3D.Threads.MainThread);
		}
	}
}
