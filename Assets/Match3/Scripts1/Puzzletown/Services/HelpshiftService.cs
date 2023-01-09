using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Helpshift;
using Match3.Scripts1.Helpshift.Campaigns;
using Match3.Scripts1.Puzzletown.Build;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007C3 RID: 1987
	public class HelpshiftService : AService
	{
		// Token: 0x060030E7 RID: 12519 RVA: 0x000E5791 File Offset: 0x000E3B91
		public HelpshiftService()
		{
			WooroutineRunner.StartCoroutine(this.LoadRoutine(), null);
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000E57B4 File Offset: 0x000E3BB4
		public void ShowFAQ()
		{
			this.UpdateMetaData();
			HelpshiftSdk instance = HelpshiftSdk.getInstance();
			if (instance != null)
			{
				instance.showFAQs();
				WooroutineRunner.StartCoroutine(this.delayedRefresh(5f), null);
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"THERE IS NOT SDK INSTANCE"
				});
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000E5804 File Offset: 0x000E3C04
		public void ShowFAQSection(string FAQsectionID)
		{
			this.UpdateMetaData();
			HelpshiftSdk instance = HelpshiftSdk.getInstance();
			if (instance != null)
			{
				if (!string.IsNullOrEmpty(FAQsectionID))
				{
					instance.showFAQSection(FAQsectionID);
				}
				else
				{
					instance.showFAQs();
				}
				WooroutineRunner.StartCoroutine(this.delayedRefresh(5f), null);
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"HelpshiftService: No SDK instance found."
				});
			}
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000E586C File Offset: 0x000E3C6C
		public void ShowSingleFAQ(string FAQsectionID)
		{
			this.UpdateMetaData();
			HelpshiftSdk instance = HelpshiftSdk.getInstance();
			if (instance != null)
			{
				if (!string.IsNullOrEmpty(FAQsectionID))
				{
					instance.showSingleFAQ(FAQsectionID);
				}
				else
				{
					instance.showFAQs();
				}
			}
			else
			{
				WoogaDebug.Log(new object[]
				{
					"HelpshiftService: No SDK instance found."
				});
			}
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000E58C0 File Offset: 0x000E3CC0
		public void ShowRatingFeedback()
		{
			HelpshiftSdk.getInstance().showConversation();
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000E58CC File Offset: 0x000E3CCC
		private IEnumerator delayedRefresh(float seconds)
		{
			yield return new WaitForSecondsRealtime(seconds);
			this.TriggerRefresh();
			yield break;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000E58F0 File Offset: 0x000E3CF0
		private Dictionary<string, object> CommonMetaData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("version", BuildVersion.ShortVersion);
			dictionary.Add("sbsuserid", SBS.Authentication.GetUserContext().user_id);
			dictionary.Add("coins", this.gameState.Resources.GetAmount("coins"));
			dictionary.Add("diamonds", this.gameState.Resources.GetAmount("diamonds"));
			dictionary.Add("highest_level", this.progressionService.UnlockedLevel);
			int levelForLastCollectedQuest = this.questService.questManager.GetLevelForLastCollectedQuest();
			dictionary.Add("quest_collected_level", levelForLastCollectedQuest);
			dictionary.Add("rounds_played", this.tracking.RoundsPlayed);
			dictionary.Add("money_spent", Mathf.RoundToInt(this.gameState.TotalUserSpendUSD));
			dictionary.Add("days_since_install", this.GetDaysSinceInstall());
			dictionary.Add("safe_dk_id", this.safeDkService.GetUserId());
			dictionary.Add("ad_id", AndroidAdId.adId);
			dictionary.Add("keep_game_language_en", this.gameState.KeepGameLanguageEn);
			return dictionary;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000E5A48 File Offset: 0x000E3E48
		public void UpdateMetaData()
		{
			if (Application.isEditor)
			{
				return;
			}
			if (!SBS.IsAuthenticated())
			{
				return;
			}
			this.UpdateUserId();
			Dictionary<string, object> dictionary = this.CommonMetaData();
			if (this.facebookService.LoggedIn())
			{
				dictionary.Add("fbuserid", this.facebookService.FB_MY_ID);
			}
			HelpshiftSdk.getInstance().updateMetaData(dictionary);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000E5AAC File Offset: 0x000E3EAC
		public void UpdatePushData()
		{
			return;
			if (Application.isEditor)
			{
				return;
			}
			if (!SBS.IsAuthenticated())
			{
				return;
			}
			this.UpdateUserId();
			HelpshiftCampaigns instance = HelpshiftCampaigns.getInstance();
			if (instance != null)
			{
				Dictionary<string, object> dictionary = this.CommonMetaData();
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string text = keyValuePair.Value.GetType().ToString();
					if (text != null)
					{
						if (!(text == "System.String"))
						{
							if (text == "System.Int32")
							{
								instance.AddProperty(keyValuePair.Key, Convert.ToInt32(keyValuePair.Value));
							}
						}
						else
						{
							instance.AddProperty(keyValuePair.Key, keyValuePair.Value.ToString());
						}
					}
				}
			}
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000E5BAC File Offset: 0x000E3FAC
		private void UpdateUserId()
		{
			if (!SBS.IsAuthenticated())
			{
				return;
			}
			HelpshiftSdk.getInstance().setUserIdentifier(SBS.Authentication.GetUserContext().user_id);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000E5BD4 File Offset: 0x000E3FD4
		private int GetDaysSinceInstall()
		{
			DateTime d = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.InstallTimestamp, DateTimeKind.Utc);
			return (DateTime.UtcNow - d).Days;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000E5C08 File Offset: 0x000E4008
		private IEnumerator LoadRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			GameObject receiverObject = new GameObject("HELPSHIFT_OBJECT_RECEIVER");
			global::UnityEngine.Object.DontDestroyOnLoad(receiverObject);
			this.hsReceiver = receiverObject.AddComponent<HelpshiftReciever>();
			this.hsReceiver.Init(this);
			HelpshiftSdk helpshift = HelpshiftSdk.getInstance();
			helpshift.install();
			if (!string.IsNullOrEmpty(this.pushService.PushToken))
			{
				this.OnTokenReceived(this.pushService.PushToken);
			}
			this.pushService.onTokenReceived.AddListener(new Action<string>(this.OnTokenReceived));
			this.UpdateUserId();
			HelpshiftSdk.getInstance().getNotificationCount(true);
			base.OnInitialized.Dispatch();
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000E5C24 File Offset: 0x000E4024
		public override void DeInit()
		{
			base.DeInit();
			if (this.hsReceiver != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.hsReceiver.gameObject);
			}
			this.pushService.onTokenReceived.RemoveListener(new Action<string>(this.OnTokenReceived));
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x000E5C74 File Offset: 0x000E4074
		// (set) Token: 0x060030F5 RID: 12533 RVA: 0x000E5C7C File Offset: 0x000E407C
		public int notificationCount { get; private set; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000E5C88 File Offset: 0x000E4088
		public int inboxCount
		{
			get
			{
				if (Application.isEditor)
				{
					return 0;
				}
				int num = 0;
				List<HelpshiftInboxMessage> list = null;
				try
				{
					list = HelpshiftInbox.getInstance().GetAllInboxMessages();
				}
				catch (Exception)
				{
				}
				if (list != null)
				{
					foreach (HelpshiftInboxMessage helpshiftInboxMessage in list)
					{
						if (helpshiftInboxMessage != null && !helpshiftInboxMessage.GetSeenStatus())
						{
							DateTime dateTime = Scripts1.DateTimeExtensions.FromUnixTimeStamp((int)helpshiftInboxMessage.GetCreatedAt(), DateTimeKind.Utc);
							bool flag = DateTime.UtcNow < dateTime.AddSeconds((double)this.configService.general.notifications.attention_indicator_cooldown);
							if (flag)
							{
								num++;
							}
						}
					}
				}
				return num;
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000E5D68 File Offset: 0x000E4168
		public void receivedNotificationCount(int count)
		{
			this.notificationCount = count;
			this.OnNotificationCountChange.Dispatch(this.notificationCount);
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000E5D84 File Offset: 0x000E4184
		public void ShowInbox()
		{
			HelpshiftCampaigns instance = HelpshiftCampaigns.getInstance();
			if (instance != null)
			{
				instance.ShowInbox(new Dictionary<string, object>());
			}
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000E5DA8 File Offset: 0x000E41A8
		public void TriggerRefresh()
		{
			HelpshiftSdk.getInstance().getNotificationCount(true);
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000E5DB6 File Offset: 0x000E41B6
		public override void OnResume()
		{
			this.TriggerRefresh();
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000E5DC0 File Offset: 0x000E41C0
		public void OnTokenReceived(string token)
		{
			HelpshiftSdk instance = HelpshiftSdk.getInstance();
			instance.registerDeviceToken(token);
		}

		// Token: 0x04005999 RID: 22937
		public static string FAQ_SECTION_TOURNAMENT = "377";

		// Token: 0x0400599A RID: 22938
		private const string HELPSHIFT_RECIEVER_OBJECT = "HELPSHIFT_OBJECT_RECEIVER";

		// Token: 0x0400599B RID: 22939
		private const string PARAMETER_AD_ID = "ad_id";

		// Token: 0x0400599C RID: 22940
		[WaitForService(true, true)]
		private TrackingService tracking;

		// Token: 0x0400599D RID: 22941
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x0400599E RID: 22942
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x0400599F RID: 22943
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x040059A0 RID: 22944
		[WaitForService(true, true)]
		private PushNotificationService pushService;

		// Token: 0x040059A1 RID: 22945
		[WaitForService(true, true)]
		private SafeDkService safeDkService;

		// Token: 0x040059A2 RID: 22946
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040059A3 RID: 22947
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x040059A4 RID: 22948
		private HelpshiftReciever hsReceiver;

		// Token: 0x040059A6 RID: 22950
		public readonly Signal<int> OnNotificationCountChange = new Signal<int>();
	}
}
