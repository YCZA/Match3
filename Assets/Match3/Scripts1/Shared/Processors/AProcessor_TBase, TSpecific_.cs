namespace Match3.Scripts1.Shared.Processors
{
	// Token: 0x02000B16 RID: 2838
	public abstract class AProcessor<TBase, TSpecific> : IProcessor<TBase> where TSpecific : TBase
	{
		// Token: 0x060042CA RID: 17098 RVA: 0x000B4D94 File Offset: 0x000B3194
		public void Process(TBase input)
		{
			if (input is TSpecific)
			{
				this.DoProcess((TSpecific)((object)input));
			}
		}

		// Token: 0x060042CB RID: 17099
		protected abstract void DoProcess(TSpecific input);
	}
}
