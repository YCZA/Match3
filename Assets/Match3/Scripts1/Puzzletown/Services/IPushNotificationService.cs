using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020008B5 RID: 2229
	public interface IPushNotificationService
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003657 RID: 13911
		string PushNotificationStartId { get; }

		// Token: 0x06003658 RID: 13912
		Coroutine SendPush(string sbsUser, string titleText, string bodyText, string templateId);

		// Token: 0x06003659 RID: 13913
		void SendFBFriendsInitialMessage();

		// Token: 0x0600365A RID: 13914
		void SendNotificationToStranger(string sbsId, string messageType);

		// Token: 0x0600365B RID: 13915
		void SendFriendProgressNotifications();

		// Token: 0x0600365C RID: 13916
		void SendChapterCompleteNotification(int chapterNumber);

		// Token: 0x0600365D RID: 13917
		void SetNotificationsEnabled(bool enabled);
	}
}
