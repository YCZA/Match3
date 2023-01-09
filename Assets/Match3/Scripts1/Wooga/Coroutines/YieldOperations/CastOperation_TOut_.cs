using System.Collections;

namespace Wooga.Coroutines.YieldOperations
{
	// Token: 0x020003DA RID: 986
	public class CastOperation<TOut> : AYieldOperation<TOut>
	{
		// Token: 0x06001DDC RID: 7644 RVA: 0x0007F70B File Offset: 0x0007DB0B
		public CastOperation(IEnumerator head) : base(head)
		{
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0007F714 File Offset: 0x0007DB14
		public override object GetContinuation()
		{
			return Nothing.AtAll;
		}
	}
}
