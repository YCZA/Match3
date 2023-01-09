using System;
using UnityEngine;

namespace Match3.Scripts1.Wooga.Notifications
{
	// Token: 0x02000422 RID: 1058
	public class NotificationEventReceiver : MonoBehaviour
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x00083280 File Offset: 0x00081680
		// (set) Token: 0x06001F18 RID: 7960 RVA: 0x00083288 File Offset: 0x00081688
		public Action OnApplicationUnpaused { get; set; }

		// Token: 0x06001F19 RID: 7961 RVA: 0x00083291 File Offset: 0x00081691
		private void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0008329E File Offset: 0x0008169E
		private void OnApplicationPause(bool isPaused)
		{
			if (!isPaused && this.OnApplicationUnpaused != null)
			{
				this.OnApplicationUnpaused();
			}
		}
	}
}
