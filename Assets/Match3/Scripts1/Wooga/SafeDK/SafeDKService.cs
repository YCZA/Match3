using Match3.Scripts1.Wooga.SafeDK.Android;

namespace Match3.Scripts1.Wooga.SafeDK
{
	// Token: 0x02000432 RID: 1074
	public class SafeDKService : ISafeDKService
	{
		// Token: 0x06001F61 RID: 8033 RVA: 0x000837D8 File Offset: 0x00081BD8
		public SafeDKService()
		{
			this._service = new AndroidSafeDKService();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000837EB File Offset: 0x00081BEB
		public string GetUserId()
		{
			return this._service.GetUserId();
		}

		// Token: 0x04004AE1 RID: 19169
		private readonly ISafeDKService _service;
	}
}
