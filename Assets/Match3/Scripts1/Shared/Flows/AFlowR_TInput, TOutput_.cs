using System.Collections;
using Wooga.Coroutines;

namespace Match3.Scripts1.Shared.Flows
{
	// Token: 0x02000AD8 RID: 2776
	public abstract class AFlowR<TInput, TOutput>
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x00090890 File Offset: 0x0008EC90
		public virtual bool ThrowImmediate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x00090894 File Offset: 0x0008EC94
		public Wooroutine<TOutput> Start(TInput input)
		{
			Wooroutine<TOutput> wooroutine = WooroutineRunner.StartWooroutine<TOutput>(this.FlowRoutine(input));
			wooroutine.ThrowImmediate = this.ThrowImmediate;
			return wooroutine;
		}

		// Token: 0x060041CB RID: 16843
		protected abstract IEnumerator FlowRoutine(TInput input);
	}
}
