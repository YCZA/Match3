namespace Match3.Scripts1
{
	// Token: 0x020007DD RID: 2013
	internal struct Notification
	{
		// Token: 0x060031A6 RID: 12710 RVA: 0x000E9842 File Offset: 0x000E7C42
		public Notification(int id, int secondsToTrigger)
		{
			this.id = id;
			this.secondsToTrigger = secondsToTrigger;
			this.titleParameters = null;
		}

		// Token: 0x04005A23 RID: 23075
		public int id;

		// Token: 0x04005A24 RID: 23076
		public string[] titleParameters;

		// Token: 0x04005A25 RID: 23077
		public int secondsToTrigger;
	}
}
