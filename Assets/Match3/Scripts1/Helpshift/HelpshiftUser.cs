namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001DA RID: 474
	public class HelpshiftUser
	{
		// Token: 0x06000DF7 RID: 3575 RVA: 0x00020CD2 File Offset: 0x0001F0D2
		private HelpshiftUser(string identifier, string email, string name, string authToken)
		{
			this.identifier = identifier;
			this.email = email;
			this.name = name;
			this.authToken = authToken;
		}

		// Token: 0x04003FD4 RID: 16340
		public readonly string identifier;

		// Token: 0x04003FD5 RID: 16341
		public readonly string email;

		// Token: 0x04003FD6 RID: 16342
		public readonly string name;

		// Token: 0x04003FD7 RID: 16343
		public readonly string authToken;

		// Token: 0x020001DB RID: 475
		public sealed class Builder
		{
			// Token: 0x06000DF8 RID: 3576 RVA: 0x00020CF7 File Offset: 0x0001F0F7
			public Builder(string identifier, string email)
			{
				this.email = email;
				this.identifier = identifier;
			}

			// Token: 0x06000DF9 RID: 3577 RVA: 0x00020D0D File Offset: 0x0001F10D
			public HelpshiftUser.Builder setName(string name)
			{
				this.name = name;
				return this;
			}

			// Token: 0x06000DFA RID: 3578 RVA: 0x00020D17 File Offset: 0x0001F117
			public HelpshiftUser.Builder setAuthToken(string authToken)
			{
				this.authToken = authToken;
				return this;
			}

			// Token: 0x06000DFB RID: 3579 RVA: 0x00020D21 File Offset: 0x0001F121
			public HelpshiftUser build()
			{
				return new HelpshiftUser(this.identifier, this.email, this.name, this.authToken);
			}

			// Token: 0x04003FD8 RID: 16344
			private string identifier;

			// Token: 0x04003FD9 RID: 16345
			private string email;

			// Token: 0x04003FDA RID: 16346
			private string name;

			// Token: 0x04003FDB RID: 16347
			private string authToken;
		}
	}
}
