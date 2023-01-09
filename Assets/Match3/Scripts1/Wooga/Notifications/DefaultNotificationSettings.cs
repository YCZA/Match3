using UnityEngine;

namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x020008B4 RID: 2228
	public class DefaultNotificationSettings : ScriptableObject
	{
		// Token: 0x06003656 RID: 13910 RVA: 0x001073E0 File Offset: 0x001057E0
		public void ApplyTo(ref NotificationSettings settings)
		{
			if (string.IsNullOrEmpty(settings.Sound))
			{
				settings.Sound = this.Sound;
			}
			if (string.IsNullOrEmpty(settings.AndroidSettings.SmallIcon))
			{
				settings.AndroidSettings.SmallIcon = this.SmallIcon;
			}
			if (string.IsNullOrEmpty(settings.AndroidSettings.BigIcon))
			{
				settings.AndroidSettings.BigIcon = this.BigIcon;
			}
			if (string.IsNullOrEmpty(settings.AndroidSettings.VibratePattern))
			{
				settings.AndroidSettings.VibratePattern = this.VibratePattern;
			}
		}

		// Token: 0x04005E60 RID: 24160
		public string SmallIcon = "notification_icon_small";

		// Token: 0x04005E61 RID: 24161
		public string BigIcon = "notification_icon";

		// Token: 0x04005E62 RID: 24162
		public string Sound = "notification_sound";

		// Token: 0x04005E63 RID: 24163
		public string VibratePattern = "0, 500, 1000, 500, 1000, 500, 1000";
	}
}
