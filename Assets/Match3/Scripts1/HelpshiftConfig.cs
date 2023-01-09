using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D5 RID: 469
namespace Match3.Scripts1
{
	[Serializable]
	public class HelpshiftConfig : ScriptableObject
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x000203A7 File Offset: 0x0001E7A7
		public static HelpshiftConfig Instance
		{
			get
			{
				HelpshiftConfig.instance = (Resources.Load("HelpshiftConfig") as HelpshiftConfig);
				if (HelpshiftConfig.instance == null)
				{
					HelpshiftConfig.instance = ScriptableObject.CreateInstance<HelpshiftConfig>();
				}
				return HelpshiftConfig.instance;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x000203DC File Offset: 0x0001E7DC
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x000203E4 File Offset: 0x0001E7E4
		public bool GotoConversation
		{
			get
			{
				return this.gotoConversation;
			}
			set
			{
				if (this.gotoConversation != value)
				{
					this.gotoConversation = value;
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x000203F9 File Offset: 0x0001E7F9
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x00020401 File Offset: 0x0001E801
		public int ContactUs
		{
			get
			{
				return this.contactUsOption;
			}
			set
			{
				if (this.contactUsOption != value)
				{
					this.contactUsOption = value;
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00020416 File Offset: 0x0001E816
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x0002041E File Offset: 0x0001E81E
		public bool PresentFullScreenOniPad
		{
			get
			{
				return this.presentFullScreen;
			}
			set
			{
				if (this.presentFullScreen != value)
				{
					this.presentFullScreen = value;
				}
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00020433 File Offset: 0x0001E833
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x00020441 File Offset: 0x0001E841
		public bool EnableInAppNotification
		{
			get
			{
				return this.enableInAppNotification != 0;
			}
			set
			{
				if (this.enableInAppNotification != Convert.ToInt32(value))
				{
					this.enableInAppNotification = Convert.ToInt32(value);
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00020460 File Offset: 0x0001E860
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x00020468 File Offset: 0x0001E868
		public bool RequireEmail
		{
			get
			{
				return this.requireEmail;
			}
			set
			{
				if (this.requireEmail != value)
				{
					this.requireEmail = value;
				}
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0002047D File Offset: 0x0001E87D
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00020485 File Offset: 0x0001E885
		public bool HideNameAndEmail
		{
			get
			{
				return this.hideNameAndEmail;
			}
			set
			{
				if (this.hideNameAndEmail != value)
				{
					this.hideNameAndEmail = value;
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x0002049A File Offset: 0x0001E89A
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000204A2 File Offset: 0x0001E8A2
		public bool EnablePrivacy
		{
			get
			{
				return this.enablePrivacy;
			}
			set
			{
				if (this.enablePrivacy != value)
				{
					this.enablePrivacy = value;
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x000204B7 File Offset: 0x0001E8B7
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x000204BF File Offset: 0x0001E8BF
		public bool ShowSearchOnNewConversation
		{
			get
			{
				return this.showSearchOnNewConversation;
			}
			set
			{
				if (this.showSearchOnNewConversation != value)
				{
					this.showSearchOnNewConversation = value;
				}
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x000204D4 File Offset: 0x0001E8D4
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x000204E2 File Offset: 0x0001E8E2
		public bool ShowConversationResolutionQuestion
		{
			get
			{
				return this.showConversationResolutionQuestion != 0;
			}
			set
			{
				if (this.showConversationResolutionQuestion != Convert.ToInt32(value))
				{
					this.showConversationResolutionQuestion = Convert.ToInt32(value);
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00020501 File Offset: 0x0001E901
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x0002050F File Offset: 0x0001E90F
		public bool EnableDefaultFallbackLanguage
		{
			get
			{
				return this.enableDefaultFallbackLanguage != 0;
			}
			set
			{
				if (this.enableDefaultFallbackLanguage != Convert.ToInt32(value))
				{
					this.enableDefaultFallbackLanguage = Convert.ToInt32(value);
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0002052E File Offset: 0x0001E92E
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00020536 File Offset: 0x0001E936
		public bool DisableEntryExitAnimations
		{
			get
			{
				return this.disableEntryExitAnimations;
			}
			set
			{
				if (this.disableEntryExitAnimations != value)
				{
					this.disableEntryExitAnimations = value;
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0002054B File Offset: 0x0001E94B
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x00020553 File Offset: 0x0001E953
		public string ConversationPrefillText
		{
			get
			{
				return this.conversationPrefillText;
			}
			set
			{
				if (this.conversationPrefillText != value)
				{
					this.conversationPrefillText = value;
				}
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0002056D File Offset: 0x0001E96D
		// (set) Token: 0x06000DB1 RID: 3505 RVA: 0x00020575 File Offset: 0x0001E975
		public string ApiKey
		{
			get
			{
				return this.apiKey;
			}
			set
			{
				if (this.apiKey != value)
				{
					this.apiKey = value;
				}
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0002058F File Offset: 0x0001E98F
		// (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00020597 File Offset: 0x0001E997
		public string DomainName
		{
			get
			{
				return this.domainName;
			}
			set
			{
				if (this.domainName != value)
				{
					this.domainName = value;
				}
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x000205B1 File Offset: 0x0001E9B1
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x000205B9 File Offset: 0x0001E9B9
		public string AndroidAppId
		{
			get
			{
				return this.androidAppId;
			}
			set
			{
				if (this.androidAppId != value)
				{
					this.androidAppId = value;
				}
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x000205D3 File Offset: 0x0001E9D3
		// (set) Token: 0x06000DB7 RID: 3511 RVA: 0x000205DB File Offset: 0x0001E9DB
		public string iOSAppId
		{
			get
			{
				return this.iosAppId;
			}
			set
			{
				if (this.iosAppId != value)
				{
					this.iosAppId = value;
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x000205F5 File Offset: 0x0001E9F5
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x000205FD File Offset: 0x0001E9FD
		public string UnityGameObject
		{
			get
			{
				return this.unityGameObject;
			}
			set
			{
				if (this.unityGameObject != value)
				{
					this.unityGameObject = value;
				}
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00020617 File Offset: 0x0001EA17
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x0002061F File Offset: 0x0001EA1F
		public string NotificationIcon
		{
			get
			{
				return this.notificationIcon;
			}
			set
			{
				if (this.notificationIcon != value)
				{
					this.notificationIcon = value;
				}
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00020639 File Offset: 0x0001EA39
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x00020641 File Offset: 0x0001EA41
		public string LargeNotificationIcon
		{
			get
			{
				return this.largeNotificationIcon;
			}
			set
			{
				if (this.largeNotificationIcon != value)
				{
					this.largeNotificationIcon = value;
				}
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0002065B File Offset: 0x0001EA5B
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00020663 File Offset: 0x0001EA63
		public string NotificationSound
		{
			get
			{
				return this.notificationSound;
			}
			set
			{
				if (this.notificationSound != value)
				{
					this.notificationSound = value;
				}
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0002067D File Offset: 0x0001EA7D
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x00020685 File Offset: 0x0001EA85
		public string CustomFont
		{
			get
			{
				return this.customFont;
			}
			set
			{
				if (this.customFont != value)
				{
					this.customFont = value;
				}
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0002069F File Offset: 0x0001EA9F
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x000206A7 File Offset: 0x0001EAA7
		public string SupportNotificationChannel
		{
			get
			{
				return this.supportNotificationChannel;
			}
			set
			{
				if (this.supportNotificationChannel != value)
				{
					this.supportNotificationChannel = value;
				}
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x000206C1 File Offset: 0x0001EAC1
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x000206C9 File Offset: 0x0001EAC9
		public string CampaignsNotificationChannel
		{
			get
			{
				return this.campaignsNotificationChannel;
			}
			set
			{
				if (this.campaignsNotificationChannel != value)
				{
					this.campaignsNotificationChannel = value;
				}
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x000206E3 File Offset: 0x0001EAE3
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x000206EB File Offset: 0x0001EAEB
		public bool EnableInboxPolling
		{
			get
			{
				return this.enableInboxPolling;
			}
			set
			{
				if (this.enableInboxPolling != value)
				{
					this.enableInboxPolling = value;
				}
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00020700 File Offset: 0x0001EB00
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x00020708 File Offset: 0x0001EB08
		public bool EnableLogging
		{
			get
			{
				return this.enableLogging;
			}
			set
			{
				if (this.enableLogging != value)
				{
					this.enableLogging = value;
				}
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0002071D File Offset: 0x0001EB1D
		// (set) Token: 0x06000DCB RID: 3531 RVA: 0x00020725 File Offset: 0x0001EB25
		public bool EnableTypingIndicator
		{
			get
			{
				return this.enableTypingIndicator;
			}
			set
			{
				if (this.enableTypingIndicator != value)
				{
					this.enableTypingIndicator = value;
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0002073A File Offset: 0x0001EB3A
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00020742 File Offset: 0x0001EB42
		public int ScreenOrientation
		{
			get
			{
				return this.screenOrientation;
			}
			set
			{
				if (this.screenOrientation != value)
				{
					this.screenOrientation = value;
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00020757 File Offset: 0x0001EB57
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x0002075F File Offset: 0x0001EB5F
		public string SupportedFileFormats
		{
			get
			{
				return this.supportedFileFormats;
			}
			set
			{
				if (this.supportedFileFormats != value)
				{
					this.supportedFileFormats = value;
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00020779 File Offset: 0x0001EB79
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00020781 File Offset: 0x0001EB81
		public bool ShowConversationInfoScreen
		{
			get
			{
				return this.showConversationInfoScreen;
			}
			set
			{
				if (this.showConversationInfoScreen != value)
				{
					this.showConversationInfoScreen = value;
				}
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00020796 File Offset: 0x0001EB96
		public Dictionary<string, object> InstallConfig
		{
			get
			{
				return HelpshiftConfig.instance.getInstallConfig();
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x000207A2 File Offset: 0x0001EBA2
		public Dictionary<string, object> ApiConfig
		{
			get
			{
				return HelpshiftConfig.instance.getApiConfig();
			}
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000207AE File Offset: 0x0001EBAE
		public void SaveConfig()
		{
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000207B0 File Offset: 0x0001EBB0
		public Dictionary<string, object> getApiConfig()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string value = HelpshiftConfig.instance.contactUsOptions[HelpshiftConfig.instance.contactUsOption];
			dictionary.Add("enableContactUs", value);
			dictionary.Add("gotoConversationAfterContactUs", (!HelpshiftConfig.instance.gotoConversation) ? "no" : "yes");
			dictionary.Add("presentFullScreenOniPad", (!HelpshiftConfig.instance.presentFullScreen) ? "no" : "yes");
			dictionary.Add("requireEmail", (!HelpshiftConfig.instance.requireEmail) ? "no" : "yes");
			dictionary.Add("hideNameAndEmail", (!HelpshiftConfig.instance.hideNameAndEmail) ? "no" : "yes");
			dictionary.Add("enableFullPrivacy", (!HelpshiftConfig.instance.enablePrivacy) ? "no" : "yes");
			dictionary.Add("showSearchOnNewConversation", (!HelpshiftConfig.instance.showSearchOnNewConversation) ? "no" : "yes");
			dictionary.Add("showConversationResolutionQuestion", (HelpshiftConfig.instance.showConversationResolutionQuestion != 1) ? "no" : "yes");
			dictionary.Add("enableTypingIndicator", (!HelpshiftConfig.instance.enableTypingIndicator) ? "no" : "yes");
			dictionary.Add("conversationPrefillText", HelpshiftConfig.instance.conversationPrefillText);
			dictionary.Add("showConversationInfoScreen", (!HelpshiftConfig.instance.showConversationInfoScreen) ? "no" : "yes");
			return dictionary;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00020970 File Offset: 0x0001ED70
		public Dictionary<string, object> getInstallConfig()
		{
			return new Dictionary<string, object>
			{
				{
					"sdkType",
					"unity"
				},
				{
					"pluginVersion",
					"4.1.0"
				},
				{
					"runtimeVersion",
					Application.unityVersion
				},
				{
					"unityGameObject",
					HelpshiftConfig.instance.unityGameObject
				},
				{
					"notificationIcon",
					HelpshiftConfig.instance.notificationIcon
				},
				{
					"largeNotificationIcon",
					HelpshiftConfig.instance.largeNotificationIcon
				},
				{
					"notificationSound",
					HelpshiftConfig.instance.notificationSound
				},
				{
					"font",
					HelpshiftConfig.instance.customFont
				},
				{
					"supportNotificationChannelId",
					HelpshiftConfig.instance.supportNotificationChannel
				},
				{
					"campaignsNotificationChannelId",
					HelpshiftConfig.instance.campaignsNotificationChannel
				},
				{
					"enableInAppNotification",
					(HelpshiftConfig.instance.enableInAppNotification != 0) ? "yes" : "no"
				},
				{
					"enableDefaultFallbackLanguage",
					(HelpshiftConfig.instance.enableDefaultFallbackLanguage != 0) ? "yes" : "no"
				},
				{
					"disableEntryExitAnimations",
					(!HelpshiftConfig.instance.disableEntryExitAnimations) ? "no" : "yes"
				},
				{
					"__hs__apiKey",
					HelpshiftConfig.instance.ApiKey
				},
				{
					"__hs__domainName",
					HelpshiftConfig.instance.DomainName
				},
				{
					"enableInboxPolling",
					(!HelpshiftConfig.instance.enableInboxPolling) ? "no" : "yes"
				},
				{
					"enableLogging",
					(!HelpshiftConfig.instance.enableLogging) ? "no" : "yes"
				},
				{
					"screenOrientation",
					HelpshiftConfig.instance.screenOrientation
				},
				{
					"supportedFileFormats",
					HelpshiftConfig.instance.supportedFileFormats
				}
			};
		}

		// Token: 0x04003FA8 RID: 16296
		private static HelpshiftConfig instance;

		// Token: 0x04003FA9 RID: 16297
		private const string helpshiftConfigAssetName = "HelpshiftConfig";

		// Token: 0x04003FAA RID: 16298
		private const string helpshiftConfigPath = "Helpshift/Resources";

		// Token: 0x04003FAB RID: 16299
		public const string pluginVersion = "4.1.0";

		// Token: 0x04003FAC RID: 16300
		[SerializeField]
		private string apiKey;

		// Token: 0x04003FAD RID: 16301
		[SerializeField]
		private string domainName;

		// Token: 0x04003FAE RID: 16302
		[SerializeField]
		private string iosAppId;

		// Token: 0x04003FAF RID: 16303
		[SerializeField]
		private string androidAppId;

		// Token: 0x04003FB0 RID: 16304
		[SerializeField]
		private int contactUsOption;

		// Token: 0x04003FB1 RID: 16305
		[SerializeField]
		private bool gotoConversation;

		// Token: 0x04003FB2 RID: 16306
		[SerializeField]
		private bool presentFullScreen;

		// Token: 0x04003FB3 RID: 16307
		[SerializeField]
		private int enableInAppNotification = 2;

		// Token: 0x04003FB4 RID: 16308
		[SerializeField]
		private bool requireEmail;

		// Token: 0x04003FB5 RID: 16309
		[SerializeField]
		private bool hideNameAndEmail;

		// Token: 0x04003FB6 RID: 16310
		[SerializeField]
		private bool enablePrivacy;

		// Token: 0x04003FB7 RID: 16311
		[SerializeField]
		private bool showSearchOnNewConversation;

		// Token: 0x04003FB8 RID: 16312
		[SerializeField]
		private int showConversationResolutionQuestion;

		// Token: 0x04003FB9 RID: 16313
		[SerializeField]
		private int enableDefaultFallbackLanguage = 2;

		// Token: 0x04003FBA RID: 16314
		[SerializeField]
		private bool disableEntryExitAnimations;

		// Token: 0x04003FBB RID: 16315
		[SerializeField]
		private string conversationPrefillText;

		// Token: 0x04003FBC RID: 16316
		[SerializeField]
		private bool enableInboxPolling = true;

		// Token: 0x04003FBD RID: 16317
		[SerializeField]
		private bool enableLogging;

		// Token: 0x04003FBE RID: 16318
		[SerializeField]
		private bool enableTypingIndicator;

		// Token: 0x04003FBF RID: 16319
		[SerializeField]
		private int screenOrientation = -1;

		// Token: 0x04003FC0 RID: 16320
		[SerializeField]
		private bool showConversationInfoScreen;

		// Token: 0x04003FC1 RID: 16321
		[SerializeField]
		private string supportedFileFormats;

		// Token: 0x04003FC2 RID: 16322
		private string[] contactUsOptions = new string[]
		{
			"always",
			"never",
			"after_viewing_faqs",
			"after_marking_answer_unhelpful"
		};

		// Token: 0x04003FC3 RID: 16323
		[SerializeField]
		private string unityGameObject;

		// Token: 0x04003FC4 RID: 16324
		[SerializeField]
		private string notificationIcon;

		// Token: 0x04003FC5 RID: 16325
		[SerializeField]
		private string largeNotificationIcon;

		// Token: 0x04003FC6 RID: 16326
		[SerializeField]
		private string notificationSound;

		// Token: 0x04003FC7 RID: 16327
		[SerializeField]
		private string customFont;

		// Token: 0x04003FC8 RID: 16328
		[SerializeField]
		private string supportNotificationChannel;

		// Token: 0x04003FC9 RID: 16329
		[SerializeField]
		private string campaignsNotificationChannel;
	}
}
