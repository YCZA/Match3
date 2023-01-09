using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003E1 RID: 993
	public class HeadYieldOpertation : AYieldOperation<object>
	{
		// Token: 0x06001DF7 RID: 7671 RVA: 0x0007FC1F File Offset: 0x0007E01F
		public HeadYieldOpertation(EnumerationContext context, IEnumerator head)
		{
			this._continuation = head;
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0007FC2E File Offset: 0x0007E02E
		public override object GetContinuation()
		{
			return this._continuation;
		}

		// Token: 0x040049EF RID: 18927
		private readonly IEnumerator _continuation;
	}
}
