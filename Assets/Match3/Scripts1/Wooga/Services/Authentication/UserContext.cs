namespace Match3.Scripts1.Wooga.Services.Authentication
{
	// Token: 0x020003BF RID: 959
	public class UserContext
	{
		// Token: 0x06001CEE RID: 7406 RVA: 0x0007D9BD File Offset: 0x0007BDBD
		public UserContext(string device_id, string password, string user_id)
		{
			this.device_id = device_id;
			this.password = password;
			this.user_id = user_id;
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x0007D9DA File Offset: 0x0007BDDA
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x0007D9E2 File Offset: 0x0007BDE2
		public string password { get; private set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0007D9EB File Offset: 0x0007BDEB
		// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x0007D9F3 File Offset: 0x0007BDF3
		public string device_id { get; private set; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x0007D9FC File Offset: 0x0007BDFC
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x0007DA04 File Offset: 0x0007BE04
		public string user_id { get; private set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x0007DA10 File Offset: 0x0007BE10
		public string ReadableUserId
		{
			get
			{
				return string.Concat(new string[]
				{
					this.user_id.Substring(0, 4),
					"-",
					this.user_id.Substring(4, 4),
					"-",
					this.user_id.Substring(8)
				});
			}
		}
	}
}
