using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x0200088A RID: 2186
	[LoadOptions(true, true, false)]
	public class WheelRoot : APtSceneRoot, IHandler<AdSpinOperation>, IDisposableDialog
	{
		// Token: 0x06003597 RID: 13719 RVA: 0x0010160C File Offset: 0x000FFA0C
		protected override void Go()
		{
			this.dialog.Show();
			this.trackingService.TrackAdWheel("open", null);
			BackButtonManager.Instance.AddAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			this.DistributePrizeIndices();
			base.StartCoroutine(this.UpdateJackpotBar(false));
			base.StartCoroutine(this.UpdateTimerText());
			for (int i = 0; i < this.prizeViews.Count; i++)
			{
				if (i % 2 == 0)
				{
					this.prizeViews[i].pending.backgroundImage.sprite = this.wheelBackgrounds.GetSimilar("ui_wheel_stone_pink");
				}
				else
				{
					this.prizeViews[i].pending.backgroundImage.sprite = this.wheelBackgrounds.GetSimilar("ui_wheel_stone_yellow");
				}
			}
			this.ResetPrizeViews();
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00101708 File Offset: 0x000FFB08
		private IEnumerator UpdateTimerText()
		{
			WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
			TimeSpan timeRemaining = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameStateService.NextSpinAvailable, DateTimeKind.Utc) - DateTime.Now;
			this.coolDownTimerLabel.gameObject.SetActive(true);
			this.spinCoolDownWindow.gameObject.SetActive(true);
			this.videosUnavailableBlocker.gameObject.SetActive(false);
			this.dialogBubble.gameObject.SetActive(true);
			foreach (TextMeshProUGUI textMeshProUGUI in this.dialogLabels)
			{
				textMeshProUGUI.text = this.localizationService.GetText("spinwheel.dialog.videos.not.available", new LocaParam[0]);
			}
			bool userReachedDailyLimit = !this.videoAdservice.IsAllowedToWatchAd(AdPlacement.AdWheel);
			while (timeRemaining.TotalSeconds > 0.0 && !userReachedDailyLimit)
			{
				this.coolDownTimerLabel.text = TimeFormatter.FormatTime(timeRemaining);
				yield return waitForOneSecond;
				timeRemaining = Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.gameStateService.NextSpinAvailable, DateTimeKind.Utc) - DateTime.Now;
			}
			base.StartCoroutine(this.UpdateJackpotBar(false));
			this.spinCoolDownWindow.gameObject.SetActive(false);
			if (!this.videoAdservice.FreeSpinAvailable)
			{
				this.SetupUiWhenNoSpinAvailable();
			}
			else
			{
				this.SetupUiForWhenFreeSpinAvailable();
			}
			this.spinButton.interactable = true;
			this.spinButtonAnimation.enabled = true;
			this.ResetPrizeViews();
			yield break;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x00101724 File Offset: 0x000FFB24
		private void SetupUiWhenNoSpinAvailable()
		{
			bool flag = !this.videoAdservice.IsAllowedToWatchAd(AdPlacement.AdWheel);
			if (!this.videoAdservice.IsVideoAvailable(true) || flag)
			{
				this.spinCoolDownWindow.gameObject.SetActive(true);
				this.coolDownTimerLabel.gameObject.SetActive(false);
				this.videosUnavailableBlocker.SetActive(true);
				this.watchAdContainer.gameObject.SetActive(false);
				this.dialogBubble.gameObject.SetActive(false);
				foreach (TextMeshProUGUI textMeshProUGUI in this.dialogLabels)
				{
					textMeshProUGUI.text = this.localizationService.GetText("spinwheel.dialog.videos.not.available", new LocaParam[0]);
				}
			}
			else
			{
				this.videosUnavailableBlocker.gameObject.SetActive(false);
				this.watchAdButton.interactable = true;
				this.watchAdContainer.gameObject.SetActive(true);
				this.dialogBubble.gameObject.SetActive(true);
				foreach (TextMeshProUGUI textMeshProUGUI2 in this.dialogLabels)
				{
					textMeshProUGUI2.text = this.localizationService.GetText("spinwheel.dialog.videos.available", new LocaParam[0]);
				}
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x001018B4 File Offset: 0x000FFCB4
		private void SetupUiForWhenFreeSpinAvailable()
		{
			this.dialogBubble.gameObject.SetActive(false);
			this.videosUnavailableBlocker.SetActive(false);
			this.watchAdContainer.gameObject.SetActive(false);
			this.spinCoolDownWindow.gameObject.SetActive(false);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x00101900 File Offset: 0x000FFD00
		private void DistributePrizeIndices()
		{
			this.prizeIndices = new List<int>();
			for (int i = 0; i < this.configService.general.spin_prizes.Count; i++)
			{
				WheelPrize wheelPrize = this.configService.general.spin_prizes[i];
				for (int j = 0; j < wheelPrize.probability; j++)
				{
					this.prizeIndices.Add(i);
				}
				this.prizeViews[wheelPrize.position].SetPrize(wheelPrize);
			}
			this.jackpotPrizeIndices = new List<int>();
			for (int k = 0; k < this.configService.general.jackpot_prizes.Count; k++)
			{
				JackpotPrize jackpotPrize = this.configService.general.jackpot_prizes[k];
				for (int l = 0; l < jackpotPrize.probability; l++)
				{
					this.jackpotPrizeIndices.Add(k);
				}
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00101A00 File Offset: 0x000FFE00
		private void ResetPrizeViews()
		{
			foreach (WheelPrizeView wheelPrizeView in this.prizeViews)
			{
				wheelPrizeView.active.gameObject.SetActive(false);
			}
			this.spinnerSparkleMaterial.color = Color.clear;
		}

		// eli key point 广告转盘
		private IEnumerator ShowAd()
		{
			this.dialogBubble.gameObject.SetActive(false);
			bool adSuccessful = false;
			if (Application.isEditor)
			{
				this.videoAdservice.EditorPretendYouJustWatchedAnAd(AdPlacement.AdWheel);
				adSuccessful = true;
				yield return null;
			}
			else
			{
				Wooroutine<VideoShowResult> showAdRoutine = WooroutineRunner.StartWooroutine<VideoShowResult>(this.videoAdservice.ShowAd(AdPlacement.AdWheel));
				yield return showAdRoutine;
				if (showAdRoutine.ReturnValue == VideoShowResult.Success)
				{
					adSuccessful = true;
				}
			}
			this.watchAdButton.interactable = true;
			if (adSuccessful)
			{
				this.watchAdContainer.gameObject.SetActive(false);
				this.videoAdservice.FreeSpinAvailable = true;
			}
			else
			{
				this.SetupUiWhenNoSpinAvailable();
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x00101A94 File Offset: 0x000FFE94
		private IEnumerator OnSpinButtonPressed()
		{
			this.closingDisabled = true;
			int randomIndex = this.prizeIndices.RandomElement(false);
			List<WheelPrize> prizeToGive = new List<WheelPrize>();
			prizeToGive.Add(this.configService.general.spin_prizes[randomIndex]);
			if (this.videoAdservice.FreeSpinAvailable)
			{
				this.gameStateService.SpinJackpotProgress++;
				this.trackingService.TrackAdWheel("spin", null);
				base.StartCoroutine(this.UpdateJackpotBar(true));
				yield return this.AnimateSpinWheel(randomIndex);
				yield return new WaitForSeconds(0.25f);
				this.prizeViews[randomIndex].active.gameObject.SetActive(true);
				yield return new WaitForSeconds(0.5f);
				Coroutine showReward = base.StartCoroutine(this.ShowRewardPopup(prizeToGive, false));
				yield return new WaitForSeconds(1f);
				this.ResetPrizeViews();
				this.gameStateService.Resources.AddMaterial("spin_wheel", 1, false);
				yield return showReward;
				this.videoAdservice.TrackClaim();
				this.externalGamesService.ShowWheelSpinAchievement();
				int nextAdAvailableInSeconds = this.configService.general.wheel_settings.spin_cooldown;
				this.gameStateService.NextSpinAvailable = DateTime.Now.AddSeconds((double)nextAdAvailableInSeconds).ToUnixTimeStamp();
				this.videoAdservice.FreeSpinAvailable = false;
				base.StartCoroutine(this.UpdateTimerText());
				if (this.gameStateService.SpinJackpotProgress >= 5)
				{
					yield return new WaitForSeconds(0.5f);
					this.HandleJackpotReward();
					this.gameStateService.SpinJackpotProgress = 0;
					yield return this.UpdateJackpotBar(false);
					// buried point: 领取转盘头奖
					DataStatistics.Instance.TriggerGetWheelBounty();
				}
				this.closingDisabled = false;
				this.gameStateService.Save(false);
			}
			yield break;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x00101AB0 File Offset: 0x000FFEB0
		private IEnumerator AnimateSpinWheel(int stopIndex)
		{
			this.wheelRotationAnimation["WheelRotation"].wrapMode = WrapMode.Loop;
			this.wheelRotationAnimation["WheelRotation"].speed = 0f;
			this.wheelRotationAnimation["WheelRotation"].enabled = true;
			this.wheelRotationAnimation["WheelRotation"].weight = 1f;
			float elapsedTime = 0f;
			float ratio = 0f;
			float startTime = this.wheelRotationAnimation["WheelRotation"].time;
			float endTime = (float)this.numberOfTurnsBeforePrize * 4f + WheelRoot.WHEEL_TIME_POSITIONS[stopIndex];
			float closeCallExtraSpin = global::UnityEngine.Random.Range(-0.05f, 0.3f);
			endTime += closeCallExtraSpin;
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / 7f;
				float curveEvalutation = this.spinCurve.Evaluate(ratio);
				this.wheelRotationAnimation["WheelRotation"].time = Mathf.Lerp(startTime, endTime, curveEvalutation);
				this.spinnerSparkleMaterial.color = new Color(1f, 1f, 1f, 1f - curveEvalutation);
				yield return null;
			}
			this.spinnerSparkleMaterial.color = Color.clear;
			this.wheelRotationAnimation["WheelRotation"].time = endTime - (float)this.numberOfTurnsBeforePrize * 4f;
			yield break;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x00101AD4 File Offset: 0x000FFED4
		private void HandleJackpotReward()
		{
			int index = this.jackpotPrizeIndices.RandomElement(false);
			List<WheelPrize> prizes = this.configService.general.jackpot_prizes[index].prizes;
			base.StartCoroutine(this.ShowRewardPopup(prizes, true));
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x00101B1C File Offset: 0x000FFF1C
		private IEnumerator ShowRewardPopup(List<WheelPrize> prizes, bool isJackpot)
		{
			Wooroutine<PopupWheelRewardRoot> rewardPopup = SceneManager.Instance.LoadSceneWithParams<PopupWheelRewardRoot, List<WheelPrize>>(prizes, null);
			yield return rewardPopup;
			if (isJackpot)
			{
				this.audioService.PlaySFX(AudioId.AdWheelJackpot, false, false, false);
			}
			else
			{
				this.audioService.PlaySFX(AudioId.AdWheelYouWon, false, false, false);
			}
			rewardPopup.ReturnValue.titleLable.text = this.localizationService.GetText((!isJackpot) ? "spinwheel.reward.title" : "spinwheel.reward.jackpot.title", new LocaParam[0]);
			this.trackingService.TrackAdWheelReward(prizes, isJackpot);
			yield return rewardPopup.ReturnValue.onClose;
			yield return new WaitForSeconds(1f);
			yield break;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00101B48 File Offset: 0x000FFF48
		private IEnumerator UpdateJackpotBar(bool animate = true)
		{
			float targetFill = (float)this.gameStateService.SpinJackpotProgress * 0.2f;
			if (animate)
			{
				float startFill = this.jackpotFill.fillAmount;
				float elapsedTime = 0f;
				float ratio = 0f;
				this.audioService.PlaySFX(AudioId.AdWheelJackpotCharge, false, false, false);
				while (ratio < 1f)
				{
					elapsedTime += Time.deltaTime;
					ratio = elapsedTime / 1f;
					this.jackpotFill.fillAmount = Mathf.Lerp(startFill, targetFill, ratio);
					yield return null;
				}
			}
			this.jackpotFill.fillAmount = targetFill;
			yield break;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x00101B6C File Offset: 0x000FFF6C
		public void Close()
		{
			this.dialog.Hide();
			BackButtonManager.Instance.RemoveAction(new Action(this.Close));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			this.onClose.Dispatch();
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x00101BBC File Offset: 0x000FFFBC
		public void Handle(AdSpinOperation evt)
		{
			switch (evt)
			{
			case AdSpinOperation.Close:
				if (!this.closingDisabled)
				{
					this.Close();
				}
				break;
			case AdSpinOperation.Spin:
				base.StartCoroutine(this.OnSpinButtonPressed());
				this.spinButton.interactable = false;
				this.spinButtonAnimation.enabled = false;
				break;
			case AdSpinOperation.JackpotDetails:
				SceneManager.Instance.LoadScene<PopupJackpotPreviewRoot>(null);
				break;
			case AdSpinOperation.WatchAd:
				this.watchAdButton.interactable = false;
				base.StartCoroutine(this.ShowAd());
				break;
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00101C6C File Offset: 0x0010006C
		protected override void OnDestroy()
		{
			this.spinnerSparkleMaterial.color = Color.white;
			base.OnDestroy();
		}

		// Token: 0x04005D8B RID: 23947
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005D8C RID: 23948
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005D8D RID: 23949
		[WaitForService(true, true)]
		private IVideoAdService videoAdservice;

		// Token: 0x04005D8E RID: 23950
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x04005D8F RID: 23951
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005D90 RID: 23952
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005D91 RID: 23953
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005D92 RID: 23954
		[WaitForService(true, true)]
		private ExternalGamesService externalGamesService;

		// Token: 0x04005D93 RID: 23955
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x04005D94 RID: 23956
		public AnimatedUi dialog;

		// Token: 0x04005D95 RID: 23957
		private List<int> prizeIndices;

		// Token: 0x04005D96 RID: 23958
		private List<int> jackpotPrizeIndices;

		// Token: 0x04005D97 RID: 23959
		private const int NUMBER_OF_SPINS_JACKPOT = 5;

		// Token: 0x04005D98 RID: 23960
		private const float JACKPOT_BAR_ANIMATION_TIME = 1f;

		// Token: 0x04005D99 RID: 23961
		private const float WHEEL_SLOWDOWN_ANIMATION_TIME = 7f;

		// Token: 0x04005D9A RID: 23962
		private const float WHEEL_ANIMATION_CLIP_LENGTH = 4f;

		// Token: 0x04005D9B RID: 23963
		private const string WHEEL_ANIMATION_CLIP_NAME = "WheelRotation";

		// Token: 0x04005D9C RID: 23964
		private const string PRIZE_BG_YELLOW = "ui_wheel_stone_yellow";

		// Token: 0x04005D9D RID: 23965
		private const string PRIZE_BG_PINK = "ui_wheel_stone_pink";

		// Token: 0x04005D9E RID: 23966
		public static readonly float[] WHEEL_TIME_POSITIONS = new float[]
		{
			0f,
			0.5f,
			1f,
			1.5f,
			2f,
			2.5f,
			3f,
			3.5f
		};

		// Token: 0x04005D9F RID: 23967
		public Image jackpotFill;

		// Token: 0x04005DA0 RID: 23968
		public SpriteManager wheelBackgrounds;

		// Token: 0x04005DA1 RID: 23969
		public List<WheelPrizeView> prizeViews;

		// Token: 0x04005DA2 RID: 23970
		public GameObject spinCoolDownWindow;

		// Token: 0x04005DA3 RID: 23971
		public GameObject watchAdContainer;

		// Token: 0x04005DA4 RID: 23972
		public Button watchAdButton;

		// Token: 0x04005DA5 RID: 23973
		public TextMeshProUGUI coolDownTimerLabel;

		// Token: 0x04005DA6 RID: 23974
		public List<TextMeshProUGUI> dialogLabels;

		// Token: 0x04005DA7 RID: 23975
		public GameObject dialogBubble;

		// Token: 0x04005DA8 RID: 23976
		public GameObject videosUnavailableBlocker;

		// Token: 0x04005DA9 RID: 23977
		public Button spinButton;

		// Token: 0x04005DAA RID: 23978
		public Animation spinButtonAnimation;

		// Token: 0x04005DAB RID: 23979
		public Animation wheelRotationAnimation;

		// Token: 0x04005DAC RID: 23980
		public int numberOfTurnsBeforePrize = 4;

		// Token: 0x04005DAD RID: 23981
		public AnimationCurve spinCurve;

		// Token: 0x04005DAE RID: 23982
		public Material spinnerSparkleMaterial;

		// Token: 0x04005DAF RID: 23983
		private bool closingDisabled;
	}
}
