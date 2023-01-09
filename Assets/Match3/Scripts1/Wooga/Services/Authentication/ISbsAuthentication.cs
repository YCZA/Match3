using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.Authentication
{
	// Token: 0x020003BA RID: 954
	public interface ISbsAuthentication
	{
		// Token: 0x06001CC0 RID: 7360
		IEnumerator<bool> Authenticate(int timeout);

		// Token: 0x06001CC1 RID: 7361
		void Logout();

		// Token: 0x06001CC2 RID: 7362
		void UpdateUserId(string userId);

		// Token: 0x06001CC3 RID: 7363
		bool IsAuthenticated();

		// Token: 0x06001CC4 RID: 7364
		UserContext GetUserContext();

		// Token: 0x06001CC5 RID: 7365
		UserContext GetUserContextForUserId(string userId);
	}
}
