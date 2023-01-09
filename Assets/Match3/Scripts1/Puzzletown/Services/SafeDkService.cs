using Match3.Scripts1.Wooga.SafeDK;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007EE RID: 2030
	public class SafeDkService : AService
	{
		// Token: 0x06003248 RID: 12872 RVA: 0x000ECA7D File Offset: 0x000EAE7D
		public SafeDkService()
		{
			this.woogaSafeDkService = new SafeDKService();
			base.OnInitialized.Dispatch();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000ECA9B File Offset: 0x000EAE9B
		public string GetUserId()
		{
			return this.woogaSafeDkService.GetUserId();
		}

		// Token: 0x04005ABD RID: 23229
		public const string SAFE_DK_ID = "safe_dk_id";

		// Token: 0x04005ABE RID: 23230
		private ISafeDKService woogaSafeDkService;
	}
}
