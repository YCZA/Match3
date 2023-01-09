using System;
using Wooga.UnityFramework;

namespace Match3.Scripts1
{
	// Token: 0x0200084F RID: 2127
	[LoadOptions(true, true, false)]
	public class APtSceneRoot : ASceneRoot
	{
		// Token: 0x060034A1 RID: 13473 RVA: 0x0008D8C2 File Offset: 0x0008BCC2
		protected override void Awake()
		{
			this.ExecutePtExtensions();
			base.Awake();
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x0008D8D0 File Offset: 0x0008BCD0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			EAHelper.AddBreadcrumb("OnDestroy " + base.GetType());
		}
	}

	[LoadOptions(true, true, false)]
	public class APtSceneRoot<TParams> : ASceneRoot<TParams>
	{
		// Token: 0x060034A4 RID: 13476 RVA: 0x0008F31B File Offset: 0x0008D71B
		protected override void Awake()
		{
			this.ExecutePtExtensions();
			base.Awake();
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x0008F329 File Offset: 0x0008D729
		protected override void OnDestroy()
		{
			base.OnDestroy();
			EAHelper.AddBreadcrumb("OnDestroy " + base.GetType());
		}
	}

	[LoadOptions(true, true, false)]
	public class APtSceneRoot<TParams, TOutput> : ASceneRoot<TParams, TOutput>
	{
		// Token: 0x060034A6 RID: 13479 RVA: 0x00097CD0 File Offset: 0x000960D0
		protected override void Awake()
		{
			this.ExecutePtExtensions();
			base.Awake();
		}

		// Token: 0x060034A7 RID: 13480 RVA: 0x00097CDE File Offset: 0x000960DE
		protected override void OnDestroy()
		{
			base.OnDestroy();
			EAHelper.AddBreadcrumb("OnDestroy " + base.GetType());
		}
	}
}