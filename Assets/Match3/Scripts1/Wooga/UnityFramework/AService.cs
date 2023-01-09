using Match3.Scripts1.Wooga.Signals;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B66 RID: 2918
	public abstract class AService : IService, IInitializable
	{
		// Token: 0x06004442 RID: 17474 RVA: 0x000D502F File Offset: 0x000D342F
		protected AService()
		{
			this.OnInitialized = new AwaitSignal();
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x000D5042 File Offset: 0x000D3442
		// (set) Token: 0x06004444 RID: 17476 RVA: 0x000D504A File Offset: 0x000D344A
		public AwaitSignal OnInitialized { get; private set; }

		// Token: 0x06004445 RID: 17477 RVA: 0x000D5053 File Offset: 0x000D3453
		public virtual void DeInit()
		{
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x000D5055 File Offset: 0x000D3455
		public virtual void OnSuspend()
		{
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x000D5057 File Offset: 0x000D3457
		public virtual void OnResume()
		{
		}
	}
}
