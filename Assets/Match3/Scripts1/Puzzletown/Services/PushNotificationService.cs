using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Core.Network;
using Match3.Scripts1.Wooga.Core.ThreadSafe;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Services;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using Wooga.Foundation.Json; // using Firebase;
// using Firebase.Messaging;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020008B6 RID: 2230
	public class PushNotificationService : AService, IPushNotificationService
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x0010747B File Offset: 0x0010587B
		public PushNotificationService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x0010749B File Offset: 0x0010589B
		public string PushNotificationStartId
		{
			get
			{
				return this.pushNotificationStartId;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x001074A3 File Offset: 0x001058A3
		public string PushToken
		{
			get
			{
				return (!PlayerPrefs.HasKey("pushToken")) ? null : PlayerPrefs.GetString("pushToken");
			}
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x001074C4 File Offset: 0x001058C4
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.OnInitialized.Dispatch();
			// 			WoogaDebug.LogWarning(new object[]
   //      					{
   //      						"Firebase dependency error: {0}",
   //      						this.dependencyStatus
   //      					});	this.lastLevelChecked = this.progressionDataService.UnlockedLevel;
			// FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith((task)=>
			// {
			// 	this.dependencyStatus = task.Result;
			// 	if (this.dependencyStatus == DependencyStatus.Available)
			// 	{
			// 		object defaultInstance = FirebaseApp.DefaultInstance;
			// 		// lock (defaultInstance)
			// 		// {
			// 			// FirebaseMessaging.TokenReceived += this.OnTokenReceived;
			// 			// FirebaseMessaging.MessageReceived += this.OnMessageReceived;
			// 		// }
			// 		Debug.Log("firebase is normal.");
			// 	}
			// 	else
			// 	{
			// 		WoogaDebug.LogWarning(new object[]
			// 		{
			// 			"Firebase dependency error: {0}",
			// 			this.dependencyStatus
			// 		});
			// 	}
			// });
			if (PlayerPrefs.HasKey("pushToken") && PlayerPrefs.GetString("pushToken") != this.gameStateService.PushToken)
			{
				this.SetNotificationsEnabled(this.settings.GetToggle(ToggleSetting.FriendActionsNotification));
				this.gameStateService.PushToken = PlayerPrefs.GetString("pushToken");
			}
			yield break;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x001074E0 File Offset: 0x001058E0
		private void NotificationReceived(Dictionary<string, object> obj)
		{
			object obj2;
			obj.TryGetValue("template_id", out obj2);
			if (obj2 != null)
			{
				this.pushNotificationStartId = "pushy_" + obj2.ToString();
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00107518 File Offset: 0x00105918
		public override void DeInit()
		{
			// if (this.dependencyStatus == DependencyStatus.Available)
			// {
			// 	object defaultInstance = FirebaseApp.DefaultInstance;
			// 	lock (defaultInstance)
			// 	{
			// 		FirebaseMessaging.TokenReceived -= this.OnTokenReceived;
			// 		FirebaseMessaging.MessageReceived -= this.OnMessageReceived;
			// 	}
			// }
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0010757C File Offset: 0x0010597C
		public Coroutine SendPush(string sbsUser, string titleText, string bodyText, string templateId)
		{
			return WooroutineRunner.StartCoroutine(this.SendPushRoutine(sbsUser, titleText, bodyText, templateId), null);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x0010758F File Offset: 0x0010598F
		public void SendFBFriendsInitialMessage()
		{
			WooroutineRunner.StartCoroutine(this.SendFBFriendsMessageRoutine(), null);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x0010759E File Offset: 0x0010599E
		public void SendNotificationToStranger(string sbsId, string notificationType)
		{
			WooroutineRunner.StartCoroutine(this.GetRankAndSendNotificationRoutine(sbsId, notificationType), null);
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x001075AF File Offset: 0x001059AF
		public void SendFriendProgressNotifications()
		{
			WooroutineRunner.StartCoroutine(this.SendProgressNotificationRoutine(), null);
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x001075BE File Offset: 0x001059BE
		public void SendChapterCompleteNotification(int chapterNumber)
		{
			WooroutineRunner.StartCoroutine(this.SendChapterCompleteNotificationRoutine(chapterNumber), null);
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x001075D0 File Offset: 0x001059D0
		public void SetNotificationsEnabled(bool enabled)
		{
			if (PlayerPrefs.HasKey("pushToken"))
			{
				WooroutineRunner.StartCoroutine(this.sbsService.serviceRunner.WaitForSBSRequest(this.RegisterTokenRequest(PlayerPrefs.GetString("pushToken"), (!enabled) ? new string[0] : PushNotificationService.PUSH_TAG)), null);
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00107629 File Offset: 0x00105A29
		// private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
		// {
		// 	if (e != null && !string.IsNullOrEmpty(e.Token))
		// 	{
		// 		this.RegistrationSucceededEvent(e.Token);
		// 		this.onTokenReceived.Dispatch(e.Token);
		// 	}
		// }

		// Token: 0x0600366B RID: 13931 RVA: 0x00107660 File Offset: 0x00105A60
		// private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
		// {
		// 	if (e == null || e.Message == null)
		// 	{
		// 		return;
		// 	}
		// 	IDictionary<string, string> data = e.Message.Data;
		// 	if (data != null && data.ContainsKey("template_id"))
		// 	{
		// 		string text = data["template_id"];
		// 		if (text != null)
		// 		{
		// 			this.pushNotificationStartId = "pushy_" + text;
		// 		}
		// 	}
		// }

		// Token: 0x0600366C RID: 13932 RVA: 0x001076C4 File Offset: 0x00105AC4
		private void RegistrationSucceededEvent(string token)
		{
			
				Scheduler.ExecuteOnMainThread(
			delegate()
			{
				if (!PlayerPrefs.HasKey("pushToken"))
				{
					PlayerPrefs.SetString("pushToken", token);
				}
				if (token != PlayerPrefs.GetString("pushToken") || PlayerPrefs.GetString("pushToken") != this.gameStateService.PushToken)
				{
					this.sbsService.serviceRunner.WaitForSBSRequest(this.RegisterTokenRequest(token, (!this.settings.GetToggle(ToggleSetting.FriendActionsNotification)) ? new string[0] : PushNotificationService.PUSH_TAG));
				}
				PlayerPrefs.SetString("pushToken", token);
				this.gameStateService.PushToken = token;
			});
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x001076F8 File Offset: 0x00105AF8
		private SbsRequest RegisterTokenRequest(string token, string[] tags)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("token", token);
			dictionary.Add("tags", tags);
			dictionary.Add("apn_type", "FCM");
			return new SbsRequest
			{
				Method = HttpMethod.POST,
				// Host = "pushy.gbs.wooga.com",
				Host = "host2333",
				Path = "/pushy/register",
				UseSignature = true,
				UserContext = SBS.Authentication.GetUserContext(),
				Body = JSON.Serialize(dictionary, false, 1, ' '),
				SendAsBytes = true
			};
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00107788 File Offset: 0x00105B88
		private IEnumerator GetVillageRankFromSBSId(string sbsId)
		{
			Wooroutine<Dictionary<string, GameStateService.villageRankBucketData>> ranksBuckets = WooroutineRunner.StartWooroutine<Dictionary<string, GameStateService.villageRankBucketData>>(this.sbsService.doRetrieveFriendsRanks(new List<string>
			{
				sbsId
			}));
			yield return ranksBuckets;
			if (ranksBuckets.ReturnValue == null || !ranksBuckets.ReturnValue.ContainsKey(sbsId))
			{
				yield return default(GameStateService.villageRankBucketData);
			}
			else
			{
				yield return ranksBuckets.ReturnValue[sbsId];
			}
			yield break;
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x001077AC File Offset: 0x00105BAC
		private SbsRequest SendPushRequest(string sbsUser, string titleText, string bodyText, string templateId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("receiver_user_id", sbsUser);
			dictionary.Add("title", titleText);
			dictionary.Add("body", bodyText);
			dictionary.Add("template_id", templateId);
			dictionary.Add("tags", PushNotificationService.PUSH_TAG);
			return new SbsRequest
			{
				Method = HttpMethod.POST,
				// Host = "pushy.gbs.wooga.com",
				Host = "host2333",
				Path = "/pushy/send_message",
				UseSignature = true,
				UserContext = SBS.Authentication.GetUserContext(),
				Body = JSON.Serialize(dictionary, false, 1, ' '),
				SendAsBytes = true
			};
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00107854 File Offset: 0x00105C54
		private IEnumerator SendPushRoutine(string sbsUser, string titleText, string bodyText, string templateID)
		{
			if (this.configService.FeatureSwitchesConfig.pushy_notifications_enabled)
			{
				Wooroutine<SbsResponse> request = this.sbsService.serviceRunner.WaitForSBSRequest(this.SendPushRequest(sbsUser, titleText, bodyText, templateID));
				yield return request;
			}
			yield break;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x0010788C File Offset: 0x00105C8C
		private IEnumerator SendFBFriendsMessageRoutine()
		{
			if (this.facebookService.LoggedIn())
			{
				float timeWaitedSoFar = 0f;
				while (string.IsNullOrEmpty(this.gameStateService.Facebook.loggedInUserFirstName) && timeWaitedSoFar < 5f)
				{
					timeWaitedSoFar += Time.deltaTime;
					yield return null;
				}
				if (string.IsNullOrEmpty(this.gameStateService.Facebook.loggedInUserFirstName))
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Pushy: Facebook username not set, not sending push"
					});
					yield break;
				}
				Wooroutine<FacebookAPIRunner.FriendListData> getFriends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
				yield return getFriends;
				FacebookAPIRunner.FriendListData friends = getFriends.ReturnValue;
				if (friends != null)
				{
					foreach (FacebookData.Friend friend in friends.friends)
					{
						GameStateService.villageRankBucketData friendProgress = this.facebookService.FriendProgress(friend.ID);
						string message = this.localizationService.GetTextInLanguage("notif.pushy.started_playing", friendProgress.language, new LocaParam[]
						{
							new LocaParam("{user}", this.gameStateService.Facebook.loggedInUserFirstName)
						});
						if (!string.IsNullOrEmpty(friendProgress.sbsId))
						{
							yield return this.SendPush(friendProgress.sbsId, this.localizationService.GetTextInLanguage("notif.pushy.title.default", friendProgress.language, new LocaParam[0]), message, "start_playing");
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x001078A8 File Offset: 0x00105CA8
		private IEnumerator SendChapterCompleteNotificationRoutine(int chapterNumber)
		{
			if (this.facebookService.LoggedIn())
			{
				Wooroutine<FacebookAPIRunner.FriendListData> getFriends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
				yield return getFriends;
				FacebookAPIRunner.FriendListData friends = getFriends.ReturnValue;
				if (friends != null)
				{
					foreach (FacebookData.Friend friend in friends.friends)
					{
						GameStateService.villageRankBucketData friendProgress = this.facebookService.FriendProgress(friend.ID);
						if (!string.IsNullOrEmpty(friendProgress.sbsId))
						{
							string chapterTitle = this.localizationService.GetTextInLanguage("ui.levelselection.chapter", friendProgress.language, new LocaParam[0]) + " " + chapterNumber;
							string chapterName = this.localizationService.GetTextInLanguage("quest.chapter.name_" + chapterNumber, friendProgress.language, new LocaParam[0]);
							string chapterText = chapterTitle + " : " + chapterName;
							yield return this.SendNotificationRoutine(friendProgress.sbsId, "finished_chapter", friendProgress.language, true, chapterText);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x001078CC File Offset: 0x00105CCC
		private IEnumerator SendProgressNotificationRoutine()
		{
			if (this.facebookService.LoggedIn())
			{
				int currentLevel = this.progressionDataService.UnlockedLevel;
				Wooroutine<FacebookAPIRunner.FriendListData> getFriends = this.facebookService.GetFriends(FacebookData.Friend.Type.Playing);
				yield return getFriends;
				FacebookAPIRunner.FriendListData friends = getFriends.ReturnValue;
				if (friends != null && currentLevel != this.lastLevelChecked)
				{
					foreach (FacebookData.Friend friend in friends.friends)
					{
						GameStateService.villageRankBucketData friendProgress = this.facebookService.FriendProgress(friend.ID);
						if (!string.IsNullOrEmpty(friendProgress.sbsId))
						{
							if (currentLevel - friendProgress.Level == -5)
							{
								yield return this.SendNotificationRoutine(friendProgress.sbsId, "catching", friendProgress.language, true, string.Empty);
							}
							else if (currentLevel - friendProgress.Level == -1)
							{
								yield return this.SendNotificationRoutine(friendProgress.sbsId, "about_pass", friendProgress.language, true, string.Empty);
							}
							else if (currentLevel - friendProgress.Level == 1)
							{
								yield return this.SendNotificationRoutine(friendProgress.sbsId, "pass", friendProgress.language, true, string.Empty);
							}
							else if (currentLevel - friendProgress.Level == 5)
							{
								yield return this.SendNotificationRoutine(friendProgress.sbsId, "pull_away", friendProgress.language, true, string.Empty);
							}
						}
					}
				}
			}
			this.lastLevelChecked = this.progressionDataService.UnlockedLevel;
			yield break;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x001078E8 File Offset: 0x00105CE8
		private IEnumerator GetRankAndSendNotificationRoutine(string sbsId, string notificationType)
		{
			Wooroutine<GameStateService.villageRankBucketData> rankState = WooroutineRunner.StartWooroutine<GameStateService.villageRankBucketData>(this.GetVillageRankFromSBSId(sbsId));
			yield return rankState;
			bool fail = false;
			WoogaSystemLanguage language = WoogaSystemLanguage.English;
			try
			{
				language = rankState.ReturnValue.language;
			}
			catch (Exception ex)
			{
				fail = true;
				Log.Warning("Pushy", string.Format("Village Rank fetch exception: {0}", ex.Message), null);
			}
			if (!fail)
			{
				yield return this.SendNotificationRoutine(sbsId, notificationType, language, false, string.Empty);
			}
			yield break;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x00107914 File Offset: 0x00105D14
		private IEnumerator SendNotificationRoutine(string sbsId, string progressType, WoogaSystemLanguage language, bool sendToFacebookFriend = true, string subText = "")
		{
			string message = string.Empty;
			string title = this.localizationService.GetTextInLanguage("notif.pushy.title.default", language, new LocaParam[0]);
			LocaParam userNameAsLocaParam = this.GetUserNameAsLocaParam(!sendToFacebookFriend);
			if (string.IsNullOrEmpty(userNameAsLocaParam.value))
			{
				yield break;
			}
			if (progressType != null)
			{
				if (progressType == "catching")
				{
					message = this.localizationService.GetTextInLanguage("notif.pushy.catching_up", language, new LocaParam[]
					{
						userNameAsLocaParam
					});
					goto IL_2EF;
				}
				if (progressType == "about_pass")
				{
					message = this.localizationService.GetTextInLanguage("notif.pushy.about_pass", language, new LocaParam[]
					{
						userNameAsLocaParam
					});
					goto IL_2EF;
				}
				if (progressType == "pass")
				{
					message = this.localizationService.GetTextInLanguage("notif.pushy.pass", language, new LocaParam[]
					{
						userNameAsLocaParam
					});
					goto IL_2EF;
				}
				if (progressType == "pull_away")
				{
					message = this.localizationService.GetTextInLanguage("notif.pushy.pull_away", language, new LocaParam[]
					{
						userNameAsLocaParam
					});
					goto IL_2EF;
				}
				if (progressType == "finished_chapter")
				{
					message = this.localizationService.GetTextInLanguage("notif.pushy.finished_chapter", language, new LocaParam[]
					{
						userNameAsLocaParam,
						new LocaParam("{chapter}", subText)
					});
					goto IL_2EF;
				}
				if (progressType == "out_of_ten")
				{
					title = this.localizationService.GetTextInLanguage("notif.pushy.title.out_of_ten", language, new LocaParam[0]);
					message = this.localizationService.GetTextInLanguage("notif.pushy.out_of_ten", language, new LocaParam[]
					{
						userNameAsLocaParam
					});
					goto IL_2EF;
				}
			}
			WoogaDebug.LogWarning(new object[]
			{
				"Invalid push type: " + progressType
			});
			IL_2EF:
			if (!string.IsNullOrEmpty(message))
			{
				yield return this.SendPush(sbsId, title, message, progressType);
			}
			yield break;
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00107954 File Offset: 0x00105D54
		protected LocaParam GetUserNameAsLocaParam(bool allowFallbackToUserGivenName = false)
		{
			string value = this.gameStateService.Facebook.loggedInUserFirstName;
			if (string.IsNullOrEmpty(value) && allowFallbackToUserGivenName)
			{
				value = this.gameStateService.PlayerName;
				if (string.IsNullOrEmpty(value))
				{
					value = "Chief";
				}
			}
			return new LocaParam("{user}", value);
		}

		// Token: 0x04005E64 RID: 24164
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005E65 RID: 24165
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005E66 RID: 24166
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005E67 RID: 24167
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005E68 RID: 24168
		[WaitForService(true, true)]
		private GameSettingsService settings;

		// Token: 0x04005E69 RID: 24169
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x04005E6A RID: 24170
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005E6B RID: 24171
		public const string PUSH_TYPE_START_PLAYING = "start_playing";

		// Token: 0x04005E6C RID: 24172
		public const string PUSH_TYPE_CATCHING = "catching";

		// Token: 0x04005E6D RID: 24173
		public const string PUSH_TYPE_ABOUT_PASS = "about_pass";

		// Token: 0x04005E6E RID: 24174
		public const string PUSH_TYPE_PASS = "pass";

		// Token: 0x04005E6F RID: 24175
		public const string PUSH_TYPE_PULL_AWAY = "pull_away";

		// Token: 0x04005E70 RID: 24176
		public const string PUSH_TYPE_FINISHED_CHAPTER = "finished_chapter";

		// Token: 0x04005E71 RID: 24177
		public const string PUSH_TYPE_OUT_OF_TEN = "out_of_ten";

		// Token: 0x04005E72 RID: 24178
		public const string PUSH_TOKEN_KEY = "pushToken";

		// Token: 0x04005E73 RID: 24179
		private const string USER_PARAMETER = "{user}";

		// Token: 0x04005E74 RID: 24180
		private const string PUSHY_PREFIX = "pushy_";

		// Token: 0x04005E75 RID: 24181
		private const string TEMPLATE_ID = "template_id";

		// Token: 0x04005E76 RID: 24182
		private const string CHAPTER_KEY_TITLE = "quest.chapter.name_";

		// Token: 0x04005E77 RID: 24183
		private const int FB_NAME_TIMEOUT = 5;

		// Token: 0x04005E78 RID: 24184
		private const string APN_TYPE = "FCM";

		// Token: 0x04005E79 RID: 24185
		private static readonly string[] PUSH_TAG = new string[]
		{
			"friend_actions"
		};

		// Token: 0x04005E7A RID: 24186
		private int lastLevelChecked;

		// Token: 0x04005E7B RID: 24187
		public Signal<string> onTokenReceived = new Signal<string>();

		// Token: 0x04005E7C RID: 24188
		// private DependencyStatus dependencyStatus;

		// Token: 0x04005E7D RID: 24189
		private string pushNotificationStartId;
	}
}
