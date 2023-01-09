using System;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006B2 RID: 1714
	[Serializable]
	public class GemViewNotFoundException : ExceptionWithMetaData<GemViewNotFoundData>
	{
		// Token: 0x06002AD3 RID: 10963 RVA: 0x000C414B File Offset: 0x000C254B
		public GemViewNotFoundException(GemViewNotFoundData data) : base(data)
		{
		}
	}
}
