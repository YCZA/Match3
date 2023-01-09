namespace Match3.Scripts1.Wooga.Services.Tracking.Parameters
{
	// Token: 0x02000441 RID: 1089
	public class AndroidAdId
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001FC3 RID: 8131 RVA: 0x00085834 File Offset: 0x00083C34
		// (remove) Token: 0x06001FC4 RID: 8132 RVA: 0x00085868 File Offset: 0x00083C68
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event AndroidAdId.AdIdReciever AdIdRecieved;

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0008589C File Offset: 0x00083C9C
		// (set) Token: 0x06001FC6 RID: 8134 RVA: 0x000858A3 File Offset: 0x00083CA3
		public static string adId
		{
			get
			{
				return AndroidAdId._adId;
			}
			set
			{
				AndroidAdId._adId = value;
				if (AndroidAdId.AdIdRecieved != null)
				{
					AndroidAdId.AdIdRecieved(value);
				}
			}
		}

		// Token: 0x04004B38 RID: 19256
		private static string _adId;

		// Token: 0x02000442 RID: 1090
		// (Invoke) Token: 0x06001FC8 RID: 8136
		public delegate void AdIdReciever(string adid);
	}
}
