using Match3.Scripts1.Wooga.Signals;
using UnityEngine.Events;

// Token: 0x02000B73 RID: 2931
namespace Match3.Scripts1
{
	public static class UnityEventExtensions
	{
		// Token: 0x0600448B RID: 17547 RVA: 0x0015CBBF File Offset: 0x0015AFBF
		public static AwaitSignal Await(this UnityEvent ev)
		{
			return new AwaitSignal(ev);
		}
	}
}
