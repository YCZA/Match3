namespace Wooga.UnityFramework
{
	// Token: 0x02000B5C RID: 2908
	public abstract class ASceneRoot<TParams> : ASceneRoot
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x0008F2FB File Offset: 0x0008D6FB
		protected override bool IsSetup
		{
			get
			{
				return this.isSetup;
			}
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0008F303 File Offset: 0x0008D703
		public virtual void Setup(TParams parameters)
		{
			this.parameters = parameters;
			this.isSetup = true;
		}

		// Token: 0x04006C55 RID: 27733
		protected TParams parameters;

		// Token: 0x04006C56 RID: 27734
		protected bool isSetup;
	}
}
