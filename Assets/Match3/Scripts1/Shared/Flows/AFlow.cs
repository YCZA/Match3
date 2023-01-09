using System.Collections;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Shared.Flows
{
	// Token: 0x02000ADC RID: 2780
	public abstract class AFlow
	{
		// Token: 0x060041D6 RID: 16854 RVA: 0x00093882 File Offset: 0x00091C82
		public Coroutine Start()
		{
			return WooroutineRunner.StartCoroutine(this.FlowRoutine(), null);
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x00093890 File Offset: 0x00091C90
		public Wooroutine<T> Start<T>()
		{
			return WooroutineRunner.StartWooroutine<T>(this.FlowRoutine());
		}

		// Token: 0x060041D8 RID: 16856
		protected abstract IEnumerator FlowRoutine();
	}
}
