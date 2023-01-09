using Match3.Scripts1.Wooga.Signals;

// Token: 0x02000B72 RID: 2930
namespace Match3.Scripts1
{
	public static class SignalExtensions
	{
		// Token: 0x06004487 RID: 17543 RVA: 0x0015CB9F File Offset: 0x0015AF9F
		public static AwaitSignal Await(this Signal signal)
		{
			return AwaitSignal.WaitFor(signal);
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0015CBA7 File Offset: 0x0015AFA7
		public static AwaitSignal<T> Await<T>(this Signal<T> signal)
		{
			return AwaitSignal<T>.WaitFor(signal);
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0015CBAF File Offset: 0x0015AFAF
		public static AwaitSignal<T1, T2> Await<T1, T2>(this Signal<T1, T2> signal)
		{
			return AwaitSignal<T1, T2>.WaitFor(signal);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0015CBB7 File Offset: 0x0015AFB7
		public static AwaitSignal<T1, T2, T3> Await<T1, T2, T3>(this Signal<T1, T2, T3> signal)
		{
			return AwaitSignal<T1, T2, T3>.WaitFor(signal);
		}
	}
}
