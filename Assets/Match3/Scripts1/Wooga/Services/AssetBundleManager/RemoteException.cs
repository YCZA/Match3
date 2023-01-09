using System;

namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000327 RID: 807
	public class RemoteException : Exception
	{
		// Token: 0x0600191B RID: 6427 RVA: 0x0007216D File Offset: 0x0007056D
		public RemoteException()
		{
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x00072175 File Offset: 0x00070575
		public RemoteException(string statusText) : base(statusText)
		{
			this._statusText = statusText;
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x00072185 File Offset: 0x00070585
		public override string Message
		{
			get
			{
				return "RemoteException " + this._statusText;
			}
		}

		// Token: 0x040047EF RID: 18415
		private string _statusText;
	}
}
