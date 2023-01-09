using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Shared.Flows
{
	// Token: 0x02000ADA RID: 2778
	public abstract class AFlow<TInput>
	{
		// Token: 0x060041D0 RID: 16848 RVA: 0x00094AD5 File Offset: 0x00092ED5
		public Coroutine Start(TInput input)
		{
			return WooroutineRunner.StartCoroutine(this.FlowRoutine(input), null);
		}

		// Token: 0x060041D1 RID: 16849
		protected abstract IEnumerator FlowRoutine(TInput input);
	}
}
