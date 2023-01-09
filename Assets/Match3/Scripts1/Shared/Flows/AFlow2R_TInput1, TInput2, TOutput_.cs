using System.Collections;
using Wooga.Coroutines;

namespace Match3.Scripts1.Shared.Flows
{
	// Token: 0x02000AD9 RID: 2777
	public abstract class AFlow2R<TInput1, TInput2, TOutput>
	{
		// Token: 0x060041CD RID: 16845 RVA: 0x00153975 File Offset: 0x00151D75
		public Wooroutine<TOutput> Start(TInput1 input1, TInput2 input2)
		{
			return WooroutineRunner.StartWooroutine<TOutput>(this.FlowRoutine(input1, input2));
		}

		// Token: 0x060041CE RID: 16846
		protected abstract IEnumerator FlowRoutine(TInput1 input1, TInput2 input2);
	}
}
