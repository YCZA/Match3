using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Shared.Pooling;
using Match3.Scripts1.Town;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Notifications;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E0 RID: 2016
	public class LocalNotificationService : AService
	{
		// Token: 0x060031A8 RID: 12712 RVA: 0x000E9870 File Offset: 0x000E7C70
		public LocalNotificationService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x000E9A58 File Offset: 0x000E7E58
		public void ChangeSetting(ToggleSetting setting, bool value)
		{
			switch (setting)
			{
			case ToggleSetting.InactivityNotifications:
				this.inactivityNotifEnabled = value;
				this.tournamentNotifEnabled = value;
				break;
			case ToggleSetting.LivesFullNotification:
				this.livesFullNotifEnabled = value;
				break;
			case ToggleSetting.NewLevelsAvailableNotification:
				this.newLevelsAvailableNotifEnabled = value;
				break;
			case ToggleSetting.HarvestBuildNotifications:
				this.harvestBuildingsNotifEnabled = value;
				break;
			case ToggleSetting.SpinAvailableNotification:
				this.spinAvailableNotifEnabled = value;
				break;
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x000E9AC9 File Offset: 0x000E7EC9
		public override void OnSuspend()
		{
			this.ScheduleNotifications();
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x000E9AD1 File Offset: 0x000E7ED1
		public override void OnResume()
		{
			this.ClearPreviousNotifications();
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x000E9AD9 File Offset: 0x000E7ED9
		private void ClearPreviousNotifications()
		{
			if (this.readyToClearNotifications)
			{
				NotificationService.ClearAll();
				NotificationService.CancelAll();
			}
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x000E9AF0 File Offset: 0x000E7EF0
		private IEnumerator InitRoutine()
		{
			this.CheckAndHandleLaunchFromNotification();
			NotificationLogger.LogLevel = LogLevel.Warning;
			NotificationLogger.LogHandler = (Action<LogLevel, string>)Delegate.Combine(NotificationLogger.LogHandler, new Action<LogLevel, string>(delegate(LogLevel level, string msg)
			{
				if (level == LogLevel.Warning)
				{
					WoogaDebug.LogWarning(new object[]
					{
						msg
					});
				}
				else
				{
					WoogaDebug.LogError(new object[]
					{
						msg
					});
				}
			}));
			yield return ServiceLocator.Instance.Inject(this);
			this.isFirstSession = this.gameState.IsEmptyState();
			this.inactivityNotifEnabled = this.settings.GetToggle(ToggleSetting.InactivityNotifications);
			this.livesFullNotifEnabled = this.settings.GetToggle(ToggleSetting.LivesFullNotification);
			this.newLevelsAvailableNotifEnabled = this.settings.GetToggle(ToggleSetting.NewLevelsAvailableNotification);
			this.harvestBuildingsNotifEnabled = this.settings.GetToggle(ToggleSetting.HarvestBuildNotifications);
			this.spinAvailableNotifEnabled = this.settings.GetToggle(ToggleSetting.SpinAvailableNotification);
			this.fbConnectNotifEnabled = this.settings.GetToggle(ToggleSetting.FbConnectNotification);
			this.tournamentNotifEnabled = this.inactivityNotifEnabled;
			this.fbNotifScheduledTimeStamp = PlayerPrefs.GetInt("FB_CONNECT_NOTIFICATION_TIME_STAMP_KEY", int.MaxValue);
			if (this.fbConnectNotifEnabled && this.fbNotifScheduledTimeStamp < DateTime.UtcNow.ToUnixTimeStamp())
			{
				this.DisableFacebookConnectNotification();
			}
			this.CollectAlternatingNotifications();
			base.OnInitialized.Dispatch();
			yield break;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x000E9B0C File Offset: 0x000E7F0C
		private void CheckAndHandleLaunchFromNotification()
		{
			string text;
			if (NotificationService.CheckForLaunchFromNotification(out text))
			{
				if (string.IsNullOrEmpty(text))
				{
					WoogaDebug.LogWarning(new object[]
					{
						"started from notification but userInfoJson is empty"
					});
					return;
				}
				UserInfo idWhenLaunchFromNotification = JsonUtility.FromJson<UserInfo>(text);
				this.SetIdWhenLaunchFromNotification(idWhenLaunchFromNotification);
			}
			this.readyToClearNotifications = true;
			this.ClearPreviousNotifications();
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000E9B60 File Offset: 0x000E7F60
		private void SetIdWhenLaunchFromNotification(UserInfo userInfo)
		{
			int notificationId = userInfo.NotificationId;
			if (notificationId < 0)
			{
				WoogaDebug.LogWarning(new object[]
				{
					"started from notification but id is < 0"
				});
				return;
			}
			this.startedByLocalNotifcation = Enum.GetName(typeof(NotificationIds), notificationId);
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000E9BAC File Offset: 0x000E7FAC
		private void ScheduleNotifications()
		{
			this.notifications.Clear();
			this.CollectFullLivesNotification();
			this.CollectInactiveNotifications();
			this.CollectFBConnectNotification();
			this.CollectHarvestableBuildingsNotification();
			this.CollectSpinAvailableNotification();
			this.CollectTournamentNotifications();
			this.CollectDailyGiftNotification();
			this.CollectNewLevelsNotification();
			this.CollectEventNotifications();
			DateTime utcNow = DateTime.UtcNow;
			foreach (Notification notification in this.notifications)
			{
				string randomBodyKey = this.GetRandomBodyKey((NotificationIds)notification.id);
				if (!string.IsNullOrEmpty(randomBodyKey))
				{
					string titleKeyForBodyKey = this.GetTitleKeyForBodyKey(randomBodyKey);
					if (titleKeyForBodyKey.Equals(randomBodyKey))
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Notification string did not contain '_body' for ",
							notification.id,
							randomBodyKey
						});
					}
					string text = this.locaService.GetText(randomBodyKey, new LocaParam[0]);
					string text2 = this.locaService.GetText(titleKeyForBodyKey, new LocaParam[0]);
					if (notification.titleParameters != null && notification.titleParameters.Length > 0)
					{
						text2 = string.Format(text2, notification.titleParameters);
					}
					string userInfo = JsonUtility.ToJson(new UserInfo(notification.id));
					NotificationSettings notificationSettings = default(NotificationSettings);
					notificationSettings.Id = notification.id;
					notificationSettings.Body = text;
					notificationSettings.FireDate = utcNow.AddSeconds((double)notification.secondsToTrigger).ToLocalTime();
					notificationSettings.UserInfo = userInfo;
					notificationSettings.AndroidSettings.Title = text2;
					notificationSettings.IOSSettings.BadgeCount = 1;
					notificationSettings.IOSSettings.LaunchImage = "notification_icon";
					NotificationSettings notificationSettings2 = notificationSettings;
					NotificationService.Schedule(notificationSettings2);
				}
			}
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x000E9D94 File Offset: 0x000E8194
		private void CollectTournamentNotifications()
		{
			if (this.tournamentNotifEnabled && this.tournamentService != null && this.tournamentService.OnInitialized.WasDispatched && this.tournamentService.Status.IsUnlocked)
			{
				this.TryCollectTournamentStartNotification();
				this.TryCollectActiveTournamentRelatedNotifications();
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x000E9DF0 File Offset: 0x000E81F0
		private void TryCollectActiveTournamentRelatedNotifications()
		{
			TournamentEventConfig tournamentEventConfig = (this.tournamentService != null) ? this.tournamentService.GetActiveTournamentEventConfig() : null;
			if (tournamentEventConfig != null)
			{
				int num = tournamentEventConfig.end - this.tournamentService.Now;
				if (num >= 12600 && num > 10800)
				{
					Notification item = new Notification(10, num - 10800);
					item.titleParameters = new string[]
					{
						tournamentEventConfig.config.name
					};
					this.notifications.Add(item);
				}
				else if (num >= 3600 && num > 1800)
				{
					Notification item2 = new Notification(11, num - 1800);
					item2.titleParameters = new string[]
					{
						tournamentEventConfig.config.name
					};
					this.notifications.Add(item2);
				}
				if (this.tournamentService.HasPlayerEntered(tournamentEventConfig))
				{
					Notification item3 = new Notification(12, num);
					item3.titleParameters = new string[]
					{
						tournamentEventConfig.config.name
					};
					this.notifications.Add(item3);
				}
			}
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x000E9F18 File Offset: 0x000E8318
		private void TryCollectTournamentStartNotification()
		{
			TournamentEventConfig tournamentEventConfig = (this.tournamentService != null) ? this.tournamentService.GetFirstUpcomingTournamentEventConfig() : null;
			if (tournamentEventConfig != null && this.tournamentService.GetActiveTournamentEventConfig() == null)
			{
				int secondsToTrigger = tournamentEventConfig.start - this.tournamentService.Now;
				Notification item = new Notification(9, secondsToTrigger);
				item.titleParameters = new string[]
				{
					tournamentEventConfig.config.name
				};
				this.notifications.Add(item);
			}
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000E9F9C File Offset: 0x000E839C
		private void CollectEventNotifications()
		{
			if (!this.inactivityNotifEnabled || this.diveForTreasureService == null)
			{
				return;
			}
			List<EventConfigContainer> upcomingEventConfigs = this.diveForTreasureService.GetUpcomingEventConfigs();
			foreach (EventConfigContainer eventConfigContainer in upcomingEventConfigs)
			{
				if (eventConfigContainer != null)
				{
					if (eventConfigContainer.config.weeklyEventType == WeeklyEventType.DiveForTreasure && this.diveForTreasureService.FeatureAvailable())
					{
						int secondsToTrigger = eventConfigContainer.start - this.timeService.Now.ToUnixTimeStamp();
						Notification item = new Notification(15, secondsToTrigger);
						this.notifications.Add(item);
					}
				}
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x000EA06C File Offset: 0x000E846C
		private void CollectDailyGiftNotification()
		{
			if (!this.sbsService.SbsConfig.feature_switches.daily_gifts_enabled)
			{
				return;
			}
			DateTime dateTime = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.DailyGifts.LastReceived, DateTimeKind.Utc).AddHours(44.0);
			TimeSpan timeSpan = dateTime - DateTime.Now;
			if (dateTime > DateTime.Now)
			{
				Notification item = new Notification(13, (int)timeSpan.TotalSeconds);
				item.titleParameters = new string[]
				{
					(this.gameState.DailyGifts.NumConsecutiveDays + 1).ToString()
				};
				this.notifications.Add(item);
			}
			if (dateTime > DateTime.Now)
			{
				Notification item2 = new Notification(14, (int)(dateTime.AddDays(1.0) - DateTime.Now).TotalSeconds);
				this.notifications.Add(item2);
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x000EA170 File Offset: 0x000E8570
		private void CollectFullLivesNotification()
		{
			if (this.livesService == null || !this.livesFullNotifEnabled)
			{
				return;
			}
			int num = this.livesService.MaxLives - this.livesService.CurrentLives;
			if (num > 0)
			{
				int secondsToTrigger = num * this.livesService.RechargeRate;
				Notification item = new Notification(4, secondsToTrigger);
				this.notifications.Add(item);
			}
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x000EA1D8 File Offset: 0x000E85D8
		private void CollectInactiveNotifications()
		{
			if (this.configService == null || !this.inactivityNotifEnabled)
			{
				return;
			}
			Notification item = new Notification(1, this.configService.general.notifications.inactive_1_day);
			if (this.isFirstSession)
			{
				item.id = 0;
			}
			this.notifications.Add(item);
			Notification item2 = new Notification(2, this.configService.general.notifications.inactive_3_days);
			this.notifications.Add(item2);
			Notification item3 = new Notification(3, this.configService.general.notifications.inactive_7_days);
			this.notifications.Add(item3);
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x000EA28C File Offset: 0x000E868C
		private void CollectHarvestableBuildingsNotification()
		{
			if (this.gameState == null || !this.harvestBuildingsNotifEnabled)
			{
				return;
			}
			int num = 0;
			foreach (BuildingInstance.PersistentData persistentData in this.gameState.Buildings.Buildings)
			{
				BuildingConfig config = this.configService.buildingConfigList.GetConfig(persistentData.blueprintName);
				if (config == null)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Incompatible Gamestate detected: Harvest Building Local Notification not scheduled"
					});
					return;
				}
				if (config.CanHarvest)
				{
					int num2 = config.harvest_timer - persistentData.HarvestTime;
					num = ((num2 <= num) ? num : num2);
				}
			}
			if (num > this.configService.general.notifications.min_duration_harvest_building)
			{
				Notification item = new Notification(7, num);
				this.notifications.Add(item);
			}
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x000EA394 File Offset: 0x000E8794
		private void DisableFacebookConnectNotification()
		{
			this.settings.SetToggle(ToggleSetting.FbConnectNotification, false);
			this.fbConnectNotifEnabled = false;
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x000EA3AC File Offset: 0x000E87AC
		private void CollectFBConnectNotification()
		{
			if (this.gameState == null || this.facebookService == null || !this.fbConnectNotifEnabled || this.gameState.Progression.LastUnlockedArea < 1)
			{
				return;
			}
			if (this.facebookService.LoggedIn())
			{
				this.DisableFacebookConnectNotification();
			}
			else
			{
				int not_logged_in_fb = this.configService.general.notifications.not_logged_in_fb;
				Notification item = new Notification(6, not_logged_in_fb);
				this.notifications.Add(item);
				this.fbNotifScheduledTimeStamp = DateTime.Now.AddSeconds((double)not_logged_in_fb).ToUnixTimeStamp();
				PlayerPrefs.SetInt("FB_CONNECT_NOTIFICATION_TIME_STAMP_KEY", this.fbNotifScheduledTimeStamp);
				PlayerPrefs.Save();
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x000EA468 File Offset: 0x000E8868
		private void CollectSpinAvailableNotification()
		{
			if (this.gameState == null || !this.spinAvailableNotifEnabled)
			{
				return;
			}
			bool flag = false;
			DateTime t = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.NextSpinAvailable, DateTimeKind.Utc);
			if (t > DateTime.Now)
			{
				if (this.gameState.lastVideoWatchedDate.Day != t.Day)
				{
					flag = true;
				}
				else if (this.videoAdService != null && this.videoAdService.OnInitialized.WasDispatched && this.videoAdService.IsAllowedToWatchAd(AdPlacement.AdWheel))
				{
					flag = true;
				}
			}
			if (flag)
			{
				Notification item = new Notification(8, (int)(Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameState.NextSpinAvailable, DateTimeKind.Utc) - DateTime.Now).TotalSeconds);
				this.notifications.Add(item);
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x000EA548 File Offset: 0x000E8948
		private void CollectNewLevelsNotification()
		{
			if (!this.newLevelsAvailableNotifEnabled || this.questService.questManager.CurrentQuestData != null)
			{
				return;
			}
			DateTime? dateTime = this.contentUnlockService.NextQuestUnlockDate();
			if (dateTime != null)
			{
				Notification item = new Notification(5, (int)(dateTime.Value - DateTime.Now).TotalSeconds);
				this.notifications.Add(item);
			}
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x000EA5BC File Offset: 0x000E89BC
		private void CollectAlternatingNotifications()
		{
			this.CollectAlternatingNotification(NotificationIds.Inactive1DayFirstSession);
			this.CollectAlternatingNotification(NotificationIds.Inactive1Day);
			this.CollectAlternatingNotification(NotificationIds.Inactive3Days);
			this.CollectAlternatingNotification(NotificationIds.Inactive7Days);
			this.CollectAlternatingNotification(NotificationIds.LivesFull);
			this.CollectAlternatingNotification(NotificationIds.FbConnect);
			this.CollectAlternatingNotification(NotificationIds.HarvestBuilding);
			this.CollectAlternatingNotification(NotificationIds.SpinAvailable);
			this.CollectAlternatingNotification(NotificationIds.TournamentStart);
			this.CollectAlternatingNotification(NotificationIds.TournamentReactivate);
			this.CollectAlternatingNotification(NotificationIds.TournamentUrgency);
			this.CollectAlternatingNotification(NotificationIds.TournamentEnd);
			this.CollectAlternatingNotification(NotificationIds.DailyGiftStreak);
			this.CollectAlternatingNotification(NotificationIds.DailyGiftReset);
			this.CollectAlternatingNotification(NotificationIds.NewLevels);
			this.CollectAlternatingNotification(NotificationIds.DiveForTreasureStart);
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000EA640 File Offset: 0x000E8A40
		private void CollectAlternatingNotification(NotificationIds id)
		{
			this.alternatingNotifications[id].AddRange(this.locaService.GetKeysWithSubstring(this.baseNotification[id]));
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000EA66C File Offset: 0x000E8A6C
		private string GetRandomBodyKey(NotificationIds id)
		{
			List<string> list = this.alternatingNotifications[id];
			if (list.Count > 0)
			{
				return RandomHelper.Next<string>(list);
			}
			WoogaDebug.LogError(new object[]
			{
				"No Notification for ",
				id.ToString()
			});
			return null;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x000EA6BD File Offset: 0x000E8ABD
		private string GetTitleKeyForBodyKey(string body)
		{
			return body.Replace("_body", "_title");
		}

		// Token: 0x04005A38 RID: 23096
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x04005A39 RID: 23097
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005A3A RID: 23098
		[WaitForService(true, true)]
		private GameStateService gameState;

		// Token: 0x04005A3B RID: 23099
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005A3C RID: 23100
		[WaitForService(true, true)]
		private GameSettingsService settings;

		// Token: 0x04005A3D RID: 23101
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005A3E RID: 23102
		[WaitForService(false, false)]
		private TournamentService tournamentService;

		// Token: 0x04005A3F RID: 23103
		[WaitForService(false, false)]
		private DailyGiftsService dailyGiftService;

		// Token: 0x04005A40 RID: 23104
		[WaitForService(false, false)]
		private SBSService sbsService;

		// Token: 0x04005A41 RID: 23105
		[WaitForService(false, false)]
		private IVideoAdService videoAdService;

		// Token: 0x04005A42 RID: 23106
		[WaitForService(true, true)]
		private ContentUnlockService contentUnlockService;

		// Token: 0x04005A43 RID: 23107
		[WaitForService(true, true)]
		private QuestService questService;

		// Token: 0x04005A44 RID: 23108
		[WaitForService(true, true)]
		private DiveForTreasureService diveForTreasureService;

		// Token: 0x04005A45 RID: 23109
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005A46 RID: 23110
		public string startedByLocalNotifcation;

		// Token: 0x04005A47 RID: 23111
		private List<Notification> notifications = ListPool<Notification>.Create(9);

		// Token: 0x04005A48 RID: 23112
		private const string FB_NOTIFICATION_TIME_STAMP_KEY = "FB_CONNECT_NOTIFICATION_TIME_STAMP_KEY";

		// Token: 0x04005A49 RID: 23113
		private const int REACTIVATE_BEFORE_TOURNAMENT_END_DEADLINE_SECONDS = 12600;

		// Token: 0x04005A4A RID: 23114
		private const int REACTIVATE_NOTIFICATION_TIME_SECONDS = 10800;

		// Token: 0x04005A4B RID: 23115
		private const int TOURNAMENT_URGENCY_DEADLINE_SECONDS = 3600;

		// Token: 0x04005A4C RID: 23116
		private const int URGENCY_NOTIFICATION_TIME_SECONDS = 1800;

		// Token: 0x04005A4D RID: 23117
		private const int DAILY_GIFT_REMINDER_HOURS = 44;

		// Token: 0x04005A4E RID: 23118
		private const string KEY_ENDING_BODY = "_body";

		// Token: 0x04005A4F RID: 23119
		private const string KEY_ENDING_TITLE = "_title";

		// Token: 0x04005A50 RID: 23120
		private bool isFirstSession;

		// Token: 0x04005A51 RID: 23121
		private bool inactivityNotifEnabled;

		// Token: 0x04005A52 RID: 23122
		private bool livesFullNotifEnabled;

		// Token: 0x04005A53 RID: 23123
		private bool newLevelsAvailableNotifEnabled;

		// Token: 0x04005A54 RID: 23124
		private bool harvestBuildingsNotifEnabled;

		// Token: 0x04005A55 RID: 23125
		private bool spinAvailableNotifEnabled;

		// Token: 0x04005A56 RID: 23126
		private bool fbConnectNotifEnabled;

		// Token: 0x04005A57 RID: 23127
		private bool tournamentNotifEnabled;

		// Token: 0x04005A58 RID: 23128
		private int fbNotifScheduledTimeStamp;

		// Token: 0x04005A59 RID: 23129
		private bool readyToClearNotifications;

		// Token: 0x04005A5A RID: 23130
		private Dictionary<NotificationIds, string> baseNotification = new Dictionary<NotificationIds, string>
		{
			{
				NotificationIds.Inactive1DayFirstSession,
				"notif.local.android.inactive.1day_body"
			},
			{
				NotificationIds.Inactive1Day,
				"notif.local.android.inactive.1day_body"
			},
			{
				NotificationIds.Inactive3Days,
				"notif.local.android.inactive.3days_body"
			},
			{
				NotificationIds.Inactive7Days,
				"notif.local.android.inactive.7days_body"
			},
			{
				NotificationIds.LivesFull,
				"notif.local.android.lives_full_body"
			},
			{
				NotificationIds.FbConnect,
				"notif.local.android.fb_connect_body"
			},
			{
				NotificationIds.HarvestBuilding,
				"notif.local.android.harvest_building_body"
			},
			{
				NotificationIds.SpinAvailable,
				"notif.local.android.spin_available_body"
			},
			{
				NotificationIds.TournamentStart,
				"notif.local.android.tournament.start_body"
			},
			{
				NotificationIds.TournamentReactivate,
				"notif.local.android.tournament.reactivate_body"
			},
			{
				NotificationIds.TournamentUrgency,
				"notif.local.android.tournament.urgency_body"
			},
			{
				NotificationIds.TournamentEnd,
				"notif.local.android.tournament.end_body"
			},
			{
				NotificationIds.DailyGiftStreak,
				"notif.local.dailygift.reactivate_day2_body"
			},
			{
				NotificationIds.DailyGiftReset,
				"notif.local.dailygift.reactivate_day1_body"
			},
			{
				NotificationIds.NewLevels,
				"notif.local.levelunlocks_body"
			},
			{
				NotificationIds.DiveForTreasureStart,
				"notif.local.treasurediving_body"
			}
		};

		// Token: 0x04005A5B RID: 23131
		private Dictionary<NotificationIds, List<string>> alternatingNotifications = new Dictionary<NotificationIds, List<string>>
		{
			{
				NotificationIds.Inactive1DayFirstSession,
				ListPool<string>.Create(1)
			},
			{
				NotificationIds.Inactive1Day,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.Inactive3Days,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.Inactive7Days,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.LivesFull,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.FbConnect,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.HarvestBuilding,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.SpinAvailable,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.TournamentStart,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.TournamentReactivate,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.TournamentUrgency,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.TournamentEnd,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.DailyGiftStreak,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.DailyGiftReset,
				ListPool<string>.Create(5)
			},
			{
				NotificationIds.NewLevels,
				ListPool<string>.Create(1)
			},
			{
				NotificationIds.DiveForTreasureStart,
				ListPool<string>.Create(1)
			}
		};
	}
}
