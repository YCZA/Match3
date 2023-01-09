using System;

// Token: 0x02000773 RID: 1907
namespace Match3.Scripts1
{
	[Serializable]
	public class PendingFacebookOperation
	{
		// Token: 0x04005851 RID: 22609
		public PendingFacebookOperation.OpType Op;

		// Token: 0x04005852 RID: 22610
		public DateTime DelayUntil = new DateTime(0L);

		// Token: 0x04005853 RID: 22611
		public bool retired;

		// Token: 0x02000774 RID: 1908
		public enum OpType
		{
			// Token: 0x04005855 RID: 22613
			Request,
			// Token: 0x04005856 RID: 22614
			DeleteLifeRequest
		}
	}
}
