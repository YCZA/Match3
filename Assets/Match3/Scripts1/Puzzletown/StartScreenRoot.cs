using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown
{
	// Token: 0x0200084C RID: 2124
	public class StartScreenRoot : APtSceneRoot, IHandler<TownOptionsCommand>
	{
		// Token: 0x06003491 RID: 13457 RVA: 0x000FBA46 File Offset: 0x000F9E46
		protected override void Awake()
		{
			this.background.raycastTarget = true;
			base.Awake();
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000FBA5C File Offset: 0x000F9E5C
		private IEnumerator Start()
		{
			yield return null;
			this.panelAnimation.Stop();
			this.logoAnimation.Stop();
			yield break;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000FBA78 File Offset: 0x000F9E78
		protected override void Go()
		{
			this.eventSystem.Enable();
			BackButtonManager.Instance.SetEnabled(true);
			this.panelAnimation["AdFacebookStartScreen"].time = 0f;
			this.logoAnimation["LogoStartScreen"].time = 0f;
			this.panelAnimation.gameObject.SetActive(false);
			foreach (GameObject gameObject in this.rewardIcons)
			{
				gameObject.gameObject.SetActive(!this.gamesStateService.Facebook.receivedLoginReward);
			}
			foreach (TextMeshProUGUI textMeshProUGUI in this.rewardAmountFields)
			{
				textMeshProUGUI.text = this.configService.general.balance.facebook_login_reward.ToString();
			}
			// 去除facebook按钮
			// this.facebookButton.gameObject.SetActive(!this.facebookService.LoggedIn());
			// this.infoButton.gameObject.SetActive(!this.facebookService.LoggedIn());
			AUiAdjuster.SetOrientationLock(false);
			
			// 直接进入游戏
			bool silentLogin = !this.configService.FeatureSwitchesConfig.initial_login_to_external_games_service;
			this.externalGamesService.LogIn(true, silentLogin);
			this.CloseStartScreen();
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000FBBF4 File Offset: 0x000F9FF4
		public void Handle(TownOptionsCommand evt)
		{
			if (evt != TownOptionsCommand.LogIn)
			{
				if (evt != TownOptionsCommand.Play)
				{
					if (evt == TownOptionsCommand.RequestSupport)
					{
						this.panelAnimation.gameObject.SetActive(true);
						this.panelAnimation.transform.parent.gameObject.SetActive(true);
						this.logoAnimation.Play();
						this.panelAnimation.Play();
						this.trackingService.TrackFunnelEvent("022_connect_now_benefits_open", 22, null);
						this.infoButton.gameObject.SetActive(false);
					}
				}
				else
				{
				}
			}
			else
			{
				// 去除facebook按钮
				// base.StartCoroutine(this.facebookLogin());
			}
		}
		
		// Token: 0x06003495 RID: 13461 RVA: 0x000FBCCC File Offset: 0x000FA0CC
		private IEnumerator facebookLogin()
		{
			this.facebookButton.interactable = false;
			Wooroutine<FacebookLoginContext> loginFlow = new FacebookLoginFlow(FacebookLoginContext.title_screen).Start<FacebookLoginContext>();
			yield return loginFlow;
			if (this.facebookService.LoggedIn())
			{
				this.CloseStartScreen();
			}
			else
			{
				this.facebookButton.interactable = true;
			}
			yield break;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000FBCE7 File Offset: 0x000FA0E7
		private void CloseStartScreen()
		{
			this.townMain.StartView(true, false);
			base.Destroy();
		}

		// Token: 0x04005C90 RID: 23696
		[WaitForService(true, true)]
		private GameStateService gamesStateService;

		// Token: 0x04005C91 RID: 23697
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005C92 RID: 23698
		[WaitForService(true, true)]
		private FacebookService facebookService;

		// Token: 0x04005C93 RID: 23699
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005C94 RID: 23700
		[WaitForService(true, true)]
		private ProgressionDataService.Service ProgressionService;

		// Token: 0x04005C95 RID: 23701
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x04005C96 RID: 23702
		[WaitForRoot(false, false)]
		private TownMainRoot townMain;

		// Token: 0x04005C97 RID: 23703
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005C98 RID: 23704
		public Animation logoAnimation;

		// Token: 0x04005C99 RID: 23705
		public Animation panelAnimation;

		// Token: 0x04005C9A RID: 23706
		public List<GameObject> rewardIcons;

		// Token: 0x04005C9B RID: 23707
		public List<TextMeshProUGUI> rewardAmountFields;

		// Token: 0x04005C9C RID: 23708
		public Button infoButton;

		// Token: 0x04005C9D RID: 23709
		public Button facebookButton;

		// Token: 0x04005C9E RID: 23710
		public Image background;

		// Token: 0x04005C9F RID: 23711
		private const string PANEL_ANIMATION_KEY = "AdFacebookStartScreen";

		// Token: 0x04005CA0 RID: 23712
		private const string LOGO_ANIMATION_KEY = "LogoStartScreen";
	}
}
