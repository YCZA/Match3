using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Shared.Flows
{
	// Token: 0x02000ADB RID: 2779
	public abstract class AFlow2<TInput1, TInput2>
	{
		// Token: 0x060041D3 RID: 16851 RVA: 0x00132449 File Offset: 0x00130849
		public Coroutine Start(TInput1 input1, TInput2 input2)
		{
			return WooroutineRunner.StartCoroutine(this.FlowRoutine(input1, input2), null);
		}

		// Token: 0x060041D4 RID: 16852
		protected abstract IEnumerator FlowRoutine(TInput1 input1, TInput2 input2);
	}
}
