namespace Match3.Scripts1.Wooga.Services.Authentication
{
	// Token: 0x020003BC RID: 956
	public class SbsCredentials
	{
		// Token: 0x06001CD5 RID: 7381 RVA: 0x0007D7B0 File Offset: 0x0007BBB0
		public SbsCredentials()
		{
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0007D7B8 File Offset: 0x0007BBB8
		public SbsCredentials(string sbsId)
		{
			this.sbs_id = sbsId;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x0007D7C7 File Offset: 0x0007BBC7
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x0007D7CF File Offset: 0x0007BBCF
		public string sbs_id { get; set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x0007D7D8 File Offset: 0x0007BBD8
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x0007D7E0 File Offset: 0x0007BBE0
		public string user_id { get; set; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x0007D7E9 File Offset: 0x0007BBE9
		// (set) Token: 0x06001CDC RID: 7388 RVA: 0x0007D7F1 File Offset: 0x0007BBF1
		public string device_id { get; set; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x0007D7FA File Offset: 0x0007BBFA
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x0007D802 File Offset: 0x0007BC02
		public string password { get; set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x0007D80B File Offset: 0x0007BC0B
		// (set) Token: 0x06001CE0 RID: 7392 RVA: 0x0007D813 File Offset: 0x0007BC13
		public string facebook_id { get; set; }

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0007D81C File Offset: 0x0007BC1C
		public override string ToString()
		{
			return string.Format("sbs_id: {0}, user_id: {1}, device_id: {2}, password: {3}, facebook_id: {4}", new object[]
			{
				this.sbs_id,
				this.user_id,
				this.device_id,
				this.password,
				this.facebook_id
			});
		}
	}
}
