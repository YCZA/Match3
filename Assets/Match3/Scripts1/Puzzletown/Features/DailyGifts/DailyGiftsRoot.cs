using System;
using System.Collections;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Features.DailyGifts
{
	// Token: 0x020008DB RID: 2267
	public class DailyGiftsRoot : APtSceneRoot, IDisposableDialog
	{
		// Token: 0x0600371F RID: 14111 RVA: 0x0010CF28 File Offset: 0x0010B328
		private void Start()
		{
			BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
			this.buttonClaim.onClick.AddListener(delegate()
			{
				this.HandleClaim(false);
			});
			this.buttonClaimWithBonus.onClick.AddListener(delegate()
			{
				this.HandleClaim(true);
			});
			this.buttonClose.onClick.AddListener(new UnityAction(this.Close));
			this.tableViewSnapper = base.GetComponentInChildren<TableViewSnapper>(true);
			this.tableView = base.GetComponentInChildren<TableView>(true);
			this.dataSource = base.GetComponent<DailyGiftsDataSource>();
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0010CFC8 File Offset: 0x0010B3C8
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				this.dailyGiftsService.ForceCurrentDay(this.mockCurrentDay);
			}
			this.ShowData();
			this.dialog.Show();
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			base.StartCoroutine(this.RefreshRoutine());
			this.windowShowTime = this.timeService.Now;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0010D034 File Offset: 0x0010B434
		private void ShowData()
		{
			bool isClaimable = this.dailyGiftsService.IsAvailable;
			this.buttonClaim.interactable = isClaimable;
			this.buttonClaimWithBonus.interactable = isClaimable;
			DailyGiftsConfig.Day currentDay = this.dailyGiftsService.CurrentDay;
			int collectedDay = (!this.dailyGiftsService.IsAvailable) ? currentDay.day : 0;
			if (collectedDay == 1)
			{
				collectedDay = this.dailyGiftsService.Days.Count<DailyGiftsConfig.Day>();
			}
			else if (collectedDay > 1)
			{
				collectedDay--;
			}
			IOrderedEnumerable<DailyGiftsDayView.Data> feed = from d in this.dailyGiftsService.Days
			select new DailyGiftsDayView.Data(d, currentDay.day == d.day && isClaimable, d.day < currentDay.day, collectedDay == d.day) into d
			orderby d.config.day descending
			select d;
			this.dataSource.Show(feed);
			float position = (float)(currentDay.day - 1) / (float)this.dailyGiftsService.Days.Count<DailyGiftsConfig.Day>();
			if (this.shouldScroll)
			{
				this.tableViewSnapper.ScrollTo(position, 0.5f);
			}
			else
			{
				this.tableViewSnapper.Snap(position);
				this.shouldScroll = true;
			}
			this.tableView.Reload();
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0010D1A0 File Offset: 0x0010B5A0
		private IEnumerator RefreshRoutine()
		{
			WaitForSeconds waitTime = new WaitForSeconds(1f);
			while (base.gameObject.activeInHierarchy)
			{
				this.Refresh();
				yield return waitTime;
			}
			yield break;
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0010D1BC File Offset: 0x0010B5BC
		private void Refresh()
		{
			if (this.hasClickedClaim)
			{
				return;
			}
			if (this.videoAdService.IsAllowedToWatchAd(AdPlacement.DailyGift) && this.videoAdService.IsVideoAvailable(true))
			{
				this.loadingSpinnerGameObject.SetActive(false);
				this.adReadyGameObject.SetActive(true);
				this.buttonClaimWithBonus.interactable = !this.hasClickedClaim;
				this.buttonTextLabel.text = this.localizationService.GetText("ui.dailygifts.claim_with_bonus.button", new LocaParam[0]);
			}
			else
			{
				this.loadingSpinnerGameObject.SetActive(true);
				this.adReadyGameObject.SetActive(false);
				this.buttonClaimWithBonus.interactable = false;
				this.buttonTextLabel.text = this.localizationService.GetText("ui.dailygifts.video_loading.button", new LocaParam[0]);
			}
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0010D28E File Offset: 0x0010B68E
		private void HandleBackButton()
		{
			if (!this.hasClickedClaim)
			{
				BackButtonManager.Instance.AddAction(new Action(this.HandleBackButton));
				this.HandleClaim(false);
			}
			else
			{
				this.Close();
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0010D2C3 File Offset: 0x0010B6C3
		private void HandleClaim(bool withBonus)
		{
			this.buttonClaim.interactable = false;
			this.buttonClaimWithBonus.interactable = false;
			base.StartCoroutine(this.ClaimRoutine(withBonus));
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0010D2EC File Offset: 0x0010B6EC
		private IEnumerator ClaimRoutine(bool withBonus)
		{
			DateTime claimTime = this.windowShowTime;
			if (this.timeService.IsTimeValid)
			{
				claimTime = this.timeService.Now;
			}
			if (withBonus)
			{
				if (Application.isEditor)
				{
					this.videoAdService.EditorPretendYouJustWatchedAnAd(AdPlacement.DailyGift);
				}
				else
				{
					Wooroutine<VideoShowResult> ad = WooroutineRunner.StartWooroutine<VideoShowResult>(
						videoAdService.ShowAd(AdPlacement.DailyGift));
					yield return ad;
					if (ad.ReturnValue != VideoShowResult.Success)
					{
						this.buttonClaim.interactable = true;
						this.buttonClaimWithBonus.interactable = true;
						yield break;
					}
				}
				this.dailyGiftsService.Claim(true, claimTime);
			}
			else
			{
				this.dailyGiftsService.Claim(false, claimTime);
			}

			this.hasClickedClaim = true;

			yield return TownRewardsRoot.ShowPendingRoutine();
			this.ShowData();
			this.bottomAnimation.Play();
			yield break;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0010D310 File Offset: 0x0010B710
		private void Close()
		{
			if (this.hasClickedClaim)
			{
				BackButtonManager.Instance.RemoveAction(new Action(this.HandleBackButton));
			}
			this.dialog.Hide();
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
		}

		// Token: 0x04005F49 RID: 24393
		private const int REFRESH_TIME_SECONDS = 1;

		// Token: 0x04005F4A RID: 24394
		private const float SCROLL_TIME = 0.5f;

		// Token: 0x04005F4B RID: 24395
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystem;

		// Token: 0x04005F4C RID: 24396
		[WaitForService(true, true)]
		private DailyGiftsService dailyGiftsService;

		// Token: 0x04005F4D RID: 24397
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04005F4E RID: 24398
		[WaitForService(true, true)]
		private TimeService timeService;

		// Token: 0x04005F4F RID: 24399
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005F50 RID: 24400
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005F51 RID: 24401
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005F52 RID: 24402
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005F53 RID: 24403
		[SerializeField]
		private Button buttonClaim;

		// Token: 0x04005F54 RID: 24404
		[SerializeField]
		private Button buttonClaimWithBonus;

		// Token: 0x04005F55 RID: 24405
		[SerializeField]
		private Button buttonClose;

		// Token: 0x04005F56 RID: 24406
		[SerializeField]
		private Animation bottomAnimation;

		// Token: 0x04005F57 RID: 24407
		[SerializeField]
		private GameObject loadingSpinnerGameObject;

		// Token: 0x04005F58 RID: 24408
		[SerializeField]
		private GameObject adReadyGameObject;

		// Token: 0x04005F59 RID: 24409
		[SerializeField]
		private TextMeshProUGUI buttonTextLabel;

		// Token: 0x04005F5A RID: 24410
		[Header("Testing")]
		[SerializeField]
		private int mockCurrentDay;

		// Token: 0x04005F5B RID: 24411
		private DateTime windowShowTime;

		// Token: 0x04005F5C RID: 24412
		private bool shouldScroll;

		// Token: 0x04005F5D RID: 24413
		private bool hasClickedClaim;

		// Token: 0x04005F5E RID: 24414
		private TableViewSnapper tableViewSnapper;

		// Token: 0x04005F5F RID: 24415
		private TableView tableView;

		// Token: 0x04005F60 RID: 24416
		private DailyGiftsDataSource dataSource;

		// Token: 0x020008DC RID: 2268
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x0600372B RID: 14123 RVA: 0x0010D37C File Offset: 0x0010B77C
			public Trigger(DailyGiftsService dailyGiftsService)
			{
				this.dailyGifts = dailyGiftsService;
			}

			// Token: 0x0600372C RID: 14124 RVA: 0x0010D38B File Offset: 0x0010B78B
			public override bool ShouldTrigger()
			{
				return this.dailyGifts.IsAvailable;
			}

			// Token: 0x0600372D RID: 14125 RVA: 0x0010D398 File Offset: 0x0010B798
			public override IEnumerator Run()
			{
				// 审核版隐藏dailyGift
				// #if !REVIEW_VERSION
				Wooroutine<DailyGiftsRoot> scene = SceneManager.Instance.LoadScene<DailyGiftsRoot>(null);
				yield return scene;
				yield return scene.ReturnValue.onDestroyed;
				// #endif
			}

			// Token: 0x04005F62 RID: 24418
			private DailyGiftsService dailyGifts;
		}
	}
}
