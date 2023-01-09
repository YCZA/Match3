using System;

namespace Match3.Scripts1
{
	// Token: 0x02000426 RID: 1062
	public struct NotificationSettings
	{
		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000833BE File Offset: 0x000817BE
		// (set) Token: 0x06001F2C RID: 7980 RVA: 0x000833C6 File Offset: 0x000817C6
		public int Id { get; set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x000833CF File Offset: 0x000817CF
		// (set) Token: 0x06001F2E RID: 7982 RVA: 0x000833D7 File Offset: 0x000817D7
		public DateTime FireDate { get; set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000833E0 File Offset: 0x000817E0
		// (set) Token: 0x06001F30 RID: 7984 RVA: 0x000833E8 File Offset: 0x000817E8
		public string Body { get; set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001F31 RID: 7985 RVA: 0x000833F1 File Offset: 0x000817F1
		// (set) Token: 0x06001F32 RID: 7986 RVA: 0x000833F9 File Offset: 0x000817F9
		public string Sound { get; set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x00083402 File Offset: 0x00081802
		// (set) Token: 0x06001F34 RID: 7988 RVA: 0x0008340A File Offset: 0x0008180A
		public string UserInfo { get; set; }

		// Token: 0x04004AC9 RID: 19145
		public NotificationSettings.IOSSpecificSettings IOSSettings;

		// Token: 0x04004ACA RID: 19146
		public NotificationSettings.AndroidSpecificSettings AndroidSettings;

		// Token: 0x02000427 RID: 1063
		public struct AndroidSpecificSettings
		{
			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00083413 File Offset: 0x00081813
			// (set) Token: 0x06001F36 RID: 7990 RVA: 0x0008341B File Offset: 0x0008181B
			public string Tag { get; set; }

			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x06001F37 RID: 7991 RVA: 0x00083424 File Offset: 0x00081824
			// (set) Token: 0x06001F38 RID: 7992 RVA: 0x0008342C File Offset: 0x0008182C
			public string SmallIcon { get; set; }

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x06001F39 RID: 7993 RVA: 0x00083435 File Offset: 0x00081835
			// (set) Token: 0x06001F3A RID: 7994 RVA: 0x0008343D File Offset: 0x0008183D
			public string BigIcon { get; set; }

			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x06001F3B RID: 7995 RVA: 0x00083446 File Offset: 0x00081846
			// (set) Token: 0x06001F3C RID: 7996 RVA: 0x0008344E File Offset: 0x0008184E
			public string Title { get; set; }

			// Token: 0x170004C6 RID: 1222
			// (get) Token: 0x06001F3D RID: 7997 RVA: 0x00083457 File Offset: 0x00081857
			// (set) Token: 0x06001F3E RID: 7998 RVA: 0x0008345F File Offset: 0x0008185F
			public string VibratePattern { get; set; }

			// Token: 0x170004C7 RID: 1223
			// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00083468 File Offset: 0x00081868
			// (set) Token: 0x06001F40 RID: 8000 RVA: 0x00083470 File Offset: 0x00081870
			public int Number { get; set; }

			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x06001F41 RID: 8001 RVA: 0x00083479 File Offset: 0x00081879
			// (set) Token: 0x06001F42 RID: 8002 RVA: 0x00083481 File Offset: 0x00081881
			public string TickerText { get; set; }
		}

		// Token: 0x02000428 RID: 1064
		public struct IOSSpecificSettings
		{
			// Token: 0x170004C9 RID: 1225
			// (get) Token: 0x06001F43 RID: 8003 RVA: 0x0008348A File Offset: 0x0008188A
			// (set) Token: 0x06001F44 RID: 8004 RVA: 0x00083492 File Offset: 0x00081892
			public string AlertAction { get; set; }

			// Token: 0x170004CA RID: 1226
			// (get) Token: 0x06001F45 RID: 8005 RVA: 0x0008349B File Offset: 0x0008189B
			// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000834A3 File Offset: 0x000818A3
			public bool HasAction { get; set; }

			// Token: 0x170004CB RID: 1227
			// (get) Token: 0x06001F47 RID: 8007 RVA: 0x000834AC File Offset: 0x000818AC
			// (set) Token: 0x06001F48 RID: 8008 RVA: 0x000834B4 File Offset: 0x000818B4
			public int BadgeCount { get; set; }

			// Token: 0x170004CC RID: 1228
			// (get) Token: 0x06001F49 RID: 8009 RVA: 0x000834BD File Offset: 0x000818BD
			// (set) Token: 0x06001F4A RID: 8010 RVA: 0x000834C5 File Offset: 0x000818C5
			public string LaunchImage { get; set; }
		}
	}
}
