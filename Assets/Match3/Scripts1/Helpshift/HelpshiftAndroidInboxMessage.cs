using System;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001C7 RID: 455
	public class HelpshiftAndroidInboxMessage : HelpshiftInboxMessage
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x0001EABC File Offset: 0x0001CEBC
		private HelpshiftAndroidInboxMessage()
		{
			if (HelpshiftAndroidInboxMessage.inboxInterfaceClass == null || HelpshiftAndroidInboxMessage.inboxInterfaceClass.GetRawObject().ToInt32() == 0)
			{
				// HelpshiftAndroidInboxMessage.inboxInterfaceClass = new AndroidJavaClass("com.helpshift.campaigns.models.InboxMessage");
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0001EAFF File Offset: 0x0001CEFF
		public string GetIdentifier()
		{
			return this.identifier;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0001EB08 File Offset: 0x0001CF08
		public string GetCoverImageFilePath()
		{
			string result = null;
			try
			{
				result = this.inboxMessageJavaInstance.Get<string>("coverImageFilePath");
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.Log("Error getting inbox message cover image :" + ex.Message);
			}
			try
			{
				this.inboxMessageJavaInstance.Call<AndroidJavaObject>("getCoverImage", new object[0]);
			}
			catch (Exception ex2)
			{
				global::UnityEngine.Debug.Log("Expected error. Java returning null for message cover image bitmap:" + ex2.Message);
			}
			return result;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0001EB9C File Offset: 0x0001CF9C
		public string GetIconImageFilePath()
		{
			try
			{
				return this.inboxMessageJavaInstance.Get<string>("iconImageFilePath");
			}
			catch (Exception ex)
			{
				global::UnityEngine.Debug.Log("Error getting inbox message icon image :" + ex.Message);
			}
			return null;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0001EBF0 File Offset: 0x0001CFF0
		public string GetTitle()
		{
			return this.title;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0001EBF8 File Offset: 0x0001CFF8
		public string GetTitleColor()
		{
			return this.titleColor;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0001EC00 File Offset: 0x0001D000
		public string GetBody()
		{
			return this.body;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0001EC08 File Offset: 0x0001D008
		public string GetBodyColor()
		{
			return this.bodyColor;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0001EC10 File Offset: 0x0001D010
		public string GetBackgroundColor()
		{
			return this.backgroundColor;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0001EC18 File Offset: 0x0001D018
		public double GetCreatedAt()
		{
			return Convert.ToDouble(this.createdAt);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0001EC25 File Offset: 0x0001D025
		public double GetExpiryTimeStamp()
		{
			return Convert.ToDouble(this.expiryTimeStamp);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0001EC32 File Offset: 0x0001D032
		public bool HasExpiryTimestamp()
		{
			return this.expiryTimeStamp != HelpshiftAndroidInboxMessage.inboxInterfaceClass.GetStatic<long>("NO_EXPIRY_TIME_STAMP");
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0001EC4E File Offset: 0x0001D04E
		public bool GetReadStatus()
		{
			return this.readStatus;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0001EC56 File Offset: 0x0001D056
		public bool GetSeenStatus()
		{
			return this.seenStatus;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0001EC5E File Offset: 0x0001D05E
		public int GetCountOfActions()
		{
			return this.actionsCount;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0001EC66 File Offset: 0x0001D066
		public string GetActionTitle(int index)
		{
			return this.inboxMessageJavaInstance.Call<string>("getActionTitle", new object[]
			{
				index
			});
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0001EC87 File Offset: 0x0001D087
		public string GetActionTitleColor(int index)
		{
			return this.inboxMessageJavaInstance.Call<string>("getActionTitleColor", new object[]
			{
				index
			});
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0001ECA8 File Offset: 0x0001D0A8
		public bool IsActionGoalCompletion(int index)
		{
			return this.inboxMessageJavaInstance.Call<bool>("isActionGoalCompletion", new object[]
			{
				index
			});
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0001ECCC File Offset: 0x0001D0CC
		public void ExecuteAction(int index, object activity)
		{
			if (activity == null)
			{
				// AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				// activity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			this.inboxMessageJavaInstance.Call("executeAction", new object[]
			{
				index,
				activity
			});
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0001ED1A File Offset: 0x0001D11A
		public string GetActionData(int index)
		{
			return this.inboxMessageJavaInstance.Call<string>("getActionData", new object[]
			{
				index
			});
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0001ED3C File Offset: 0x0001D13C
		public HelpshiftInboxMessageActionType GetActionType(int index)
		{
			AndroidJavaObject androidJavaObject = this.inboxMessageJavaInstance.Call<AndroidJavaObject>("getActionType", new object[]
			{
				index
			});
			int num = androidJavaObject.Call<int>("ordinal", new object[0]);
			HelpshiftInboxMessageActionType result = HelpshiftInboxMessageActionType.UNKNOWN;
			if (num == 0)
			{
				result = HelpshiftInboxMessageActionType.UNKNOWN;
			}
			else if (num == 1)
			{
				result = HelpshiftInboxMessageActionType.OPEN_DEEP_LINK;
			}
			else if (num == 2)
			{
				result = HelpshiftInboxMessageActionType.SHOW_FAQS;
			}
			else if (num == 3)
			{
				result = HelpshiftInboxMessageActionType.SHOW_FAQ_SECTION;
			}
			else if (num == 4)
			{
				result = HelpshiftInboxMessageActionType.SHOW_CONVERSATION;
			}
			else if (num == 5)
			{
				result = HelpshiftInboxMessageActionType.SHOW_SINGLE_FAQ;
			}
			else if (num == 6)
			{
				result = HelpshiftInboxMessageActionType.SHOW_ALERT_TO_RATE_APP;
			}
			return result;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0001EDDC File Offset: 0x0001D1DC
		public static HelpshiftAndroidInboxMessage createInboxMessage(AndroidJavaObject message)
		{
			if (message == null || message.GetRawObject().ToInt32() == 0)
			{
				return null;
			}
			return new HelpshiftAndroidInboxMessage
			{
				inboxMessageJavaInstance = message,
				identifier = message.Call<string>("getIdentifier", new object[0]),
				title = message.Call<string>("getTitle", new object[0]),
				titleColor = message.Call<string>("getTitleColor", new object[0]),
				body = message.Call<string>("getBody", new object[0]),
				bodyColor = message.Call<string>("getBodyColor", new object[0]),
				backgroundColor = message.Call<string>("getBackgroundColor", new object[0]),
				createdAt = message.Call<long>("getCreatedAt", new object[0]),
				expiryTimeStamp = message.Call<long>("getExpiryTimeStamp", new object[0]),
				seenStatus = message.Call<bool>("getSeenStatus", new object[0]),
				readStatus = message.Call<bool>("getReadStatus", new object[0]),
				actionsCount = message.Call<int>("getCountOfActions", new object[0])
			};
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0001EF10 File Offset: 0x0001D310
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Title : ",
				this.title,
				"  Identifier : ",
				this.identifier,
				"\n Body : ",
				this.body,
				"  CreatedAt : ",
				this.createdAt,
				"\n Seen state :",
				this.seenStatus,
				"  Read state : ",
				this.readStatus,
				"\n Count of Actions :",
				this.actionsCount,
				"  Expiry : ",
				this.expiryTimeStamp,
				"\n BodyColor : ",
				this.bodyColor,
				" BackgroundColor : ",
				this.backgroundColor,
				"\n Action count : ",
				this.actionsCount
			});
		}

		// Token: 0x04003F5D RID: 16221
		private AndroidJavaObject inboxMessageJavaInstance;

		// Token: 0x04003F5E RID: 16222
		private static AndroidJavaClass inboxInterfaceClass;

		// Token: 0x04003F5F RID: 16223
		private string identifier;

		// Token: 0x04003F60 RID: 16224
		private string title;

		// Token: 0x04003F61 RID: 16225
		private string titleColor;

		// Token: 0x04003F62 RID: 16226
		private string body;

		// Token: 0x04003F63 RID: 16227
		private string bodyColor;

		// Token: 0x04003F64 RID: 16228
		private string backgroundColor;

		// Token: 0x04003F65 RID: 16229
		private long createdAt;

		// Token: 0x04003F66 RID: 16230
		private long expiryTimeStamp;

		// Token: 0x04003F67 RID: 16231
		private bool readStatus;

		// Token: 0x04003F68 RID: 16232
		private bool seenStatus;

		// Token: 0x04003F69 RID: 16233
		private int actionsCount;
	}
}
