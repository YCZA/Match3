using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using Match3.Scripts2.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000907 RID: 2311
namespace Match3.Scripts1
{
	public class ChallengeProgressViewV2 : ChallengeProgressView
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06003864 RID: 14436 RVA: 0x00113EB1 File Offset: 0x001122B1
		protected override string CollectAnimationName
		{
			get
			{
				return "ChallengeV2ViewTaskToCollect";
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x00113EB8 File Offset: 0x001122B8
		protected override string CompleteAnimationName
		{
			get
			{
				return "ChallengeV2ViewCollectToCompleted";
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x00113EBF File Offset: 0x001122BF
		protected override string ReturnToCollectAnimationName
		{
			get
			{
				return "ChallengeV2ViewCompletedToCollectAgain";
			}
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x00113EC8 File Offset: 0x001122C8
		public override void SetChallengeData(int challengeIndex, GameStateService gameStateService, ILocalizationService localizationService, ChallengeService challengeService, SBSService sbsService, IVideoAdService videoAdService)
		{
			this.gameStateService = gameStateService;
			this.localizationService = localizationService;
			this.challengeService = challengeService;
			this.challengeIndex = challengeIndex;
			this.videoAdService = videoAdService;
			this.sbsService = sbsService;
			this.watchAdButton.onClick.AddListener(new UnityAction(this.HandleWatchAd));
			this.skipWithDiamondsButton.onClick.AddListener(new UnityAction(this.HandleSkip));
			this.collectButton.onClick.RemoveAllListeners();
			this.collectButton.onClick.AddListener(new UnityAction(this.OnCollectButton));
			this.exchangeButton.onClick.RemoveAllListeners();
			this.exchangeButton.onClick.AddListener(new UnityAction(this.HandleExhangeButton));
			this.exchangeDiamondButton.onClick.RemoveAllListeners();
			this.exchangeDiamondButton.onClick.AddListener(new UnityAction(this.HandlePurchaseExchangeButton));
			this.cancelButton.onClick.RemoveAllListeners();
			this.cancelButton.onClick.AddListener(new UnityAction(this.HandleCancelButton));
			this.collectButton.interactable = true;
			this.exchangePanel.SetActive(false);
			this.AddSlowUpdate(new SlowUpdate(this.RefreshAdStatus), 1);
			this.isSetup = true;
			this.RefreshAdStatus();
			this.Refresh();
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x00114028 File Offset: 0x00112428
		private void Refresh()
		{
			base.challenge = this.gameStateService.Challenges.CurrentChallenges[this.challengeIndex];
			this.pawReward = this.challengeService.GetPawReward(base.challenge);
			this.difficultyBackground.sprite = this.spriteManager.GetSimilar(base.challenge.difficulty.ToString());
			this.itemImage.sprite = this.spriteManager.GetSimilar(base.challenge.type);
			this.difficultyLabel.text = this.localizationService.GetText("ui.challenges.difficulty." + base.challenge.difficulty, new LocaParam[0]);
			this.pawLabel.text = this.pawReward.ToString();
			this.taskLabel.text = string.Format(this.localizationService.GetText("ui.challenges.task." + base.challenge.type, new LocaParam[0]), base.challenge.goal);
			this.additionalRewardView.Show(this.GetAdditionalReward());
			this.diamondSkipPriceLabel.text = this.challengeService.Balancing.diamond_skip_price.ToString();
			this.SetupViewState(this.gameStateService.Resources.GetCollectedTotal(base.challenge.type), !this.challengeService.IsChallengeRunning);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x001141B8 File Offset: 0x001125B8
		private void RefreshAdStatus()
		{
			bool flag = !this.videoAdService.IsAllowedToWatchAd(AdPlacement.Challenges_V2);
			if (!this.videoAdService.IsVideoAvailable(true) || flag)
			{
				if ((DateTime.UtcNow - this.videoAdService.LastAdTime).TotalSeconds > 60.0 || flag)
				{
					this.SetAdButtonState(ChallengeProgressViewV2.AdButtonState.PayWithdiamonds);
				}
				else
				{
					this.SetAdButtonState(ChallengeProgressViewV2.AdButtonState.AdLoading);
				}
			}
			else if (this.currentAdButtonState != ChallengeProgressViewV2.AdButtonState.PayWithdiamonds)
			{
				this.SetAdButtonState(ChallengeProgressViewV2.AdButtonState.AdAvailable);
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0011424C File Offset: 0x0011264C
		private void SetAdButtonState(ChallengeProgressViewV2.AdButtonState adButtonState)
		{
			if (adButtonState != ChallengeProgressViewV2.AdButtonState.AdAvailable)
			{
				if (adButtonState != ChallengeProgressViewV2.AdButtonState.AdLoading)
				{
					if (adButtonState == ChallengeProgressViewV2.AdButtonState.PayWithdiamonds)
					{
						this.watchAdButton.gameObject.SetActive(false);
						this.skipWithDiamondsButton.gameObject.SetActive(true);
					}
				}
				else
				{
					this.watchAdButton.gameObject.SetActive(true);
					this.watchAdButton.interactable = false;
					this.skipWithDiamondsButton.gameObject.SetActive(false);
				}
			}
			else
			{
				this.watchAdButton.gameObject.SetActive(true);
				this.watchAdButton.interactable = true;
				this.skipWithDiamondsButton.gameObject.SetActive(false);
			}
			this.currentAdButtonState = adButtonState;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x00114308 File Offset: 0x00112708
		protected override void SetupViewState(int amountInGameState, bool locked)
		{
			int amountCollectedAndViewed = base.challenge.AmountCollectedAndViewed;
			int num = amountInGameState - base.challenge.start;
			int num2 = base.challenge.TargetAmount - base.challenge.start;
			float val = (float)amountCollectedAndViewed / (float)num2;
			this.progressSlider.fillAmount = Math.Min(val, 1f);
			if (base.challenge.collected)
			{
				this.exchangeButton.gameObject.SetActive(false);
				this.stateAnimation[this.CompleteAnimationName].time = 1f;
				this.stateAnimation.Play(this.CompleteAnimationName);
				this.nextChallengeTimer.SetTargetTime(base.challenge.CollectedTime.AddMinutes((double)this.challengeService.Balancing.cool_down_timer_minutes), true, new Action(this.HandleTimeReached));
			}
			else if (amountCollectedAndViewed < num2)
			{
				this.exchangeButton.gameObject.SetActive(true);
				int num3 = amountInGameState - base.challenge.start;
				this.progressLabel.text = num3 + "/" + base.challenge.goal;
				base.StartCoroutine(this.AnimateProgressRoutine(amountInGameState));
				if (num >= num2)
				{
					this.exchangeButton.gameObject.SetActive(false);
				}
			}
			else
			{
				this.exchangeButton.gameObject.SetActive(false);
				this.stateAnimation[this.CollectAnimationName].time = 1f;
				this.stateAnimation.Play(this.CollectAnimationName);
			}
			this.exchangeLabel.text = this.GetSkipPrice(base.challenge.difficulty).ToString();
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x001144DC File Offset: 0x001128DC
		private MaterialAmount GetAdditionalReward()
		{
			MaterialAmount result = default(MaterialAmount);
			ChallengeGoal.ChallengeDifficulty difficulty = base.challenge.difficulty;
			if (difficulty != ChallengeGoal.ChallengeDifficulty.easy)
			{
				if (difficulty != ChallengeGoal.ChallengeDifficulty.medium)
				{
					if (difficulty == ChallengeGoal.ChallengeDifficulty.hard)
					{
						result.type = this.challengeService.Balancing.hard_additional_reward;
						result.amount = this.challengeService.Balancing.hard_additional_amount;
					}
				}
				else
				{
					result.type = this.challengeService.Balancing.medium_additional_reward;
					result.amount = this.challengeService.Balancing.medium_additional_amount;
				}
			}
			else
			{
				result.type = this.challengeService.Balancing.easy_additional_reward;
				result.amount = this.challengeService.Balancing.easy_additional_amount;
			}
			return result;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x001145B0 File Offset: 0x001129B0
		private void HandleTimeReached()
		{
			this.ResetChallenge(true);
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x001145BC File Offset: 0x001129BC
		private void ResetChallenge(bool playAnimation = true)
		{
			int difficulty = (int)base.challenge.difficulty;
			this.challengeService.AssignNextChallenges(difficulty);
			this.exchangePanel.SetActive(false);
			this.Refresh();
			if (playAnimation)
			{
				this.stateAnimation.Play(this.ReturnToCollectAnimationName);
			}
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0011460C File Offset: 0x00112A0C
		private int GetSkipPrice(ChallengeGoal.ChallengeDifficulty difficulty)
		{
			int result = 0;
			ChallengeGoal.ChallengeDifficulty difficulty2 = base.challenge.difficulty;
			if (difficulty2 != ChallengeGoal.ChallengeDifficulty.easy)
			{
				if (difficulty2 != ChallengeGoal.ChallengeDifficulty.medium)
				{
					if (difficulty2 == ChallengeGoal.ChallengeDifficulty.hard)
					{
						result = this.sbsService.SbsConfig.challenges.balancing_v2.hard_diamond_skip_price;
					}
				}
				else
				{
					result = this.sbsService.SbsConfig.challenges.balancing_v2.medium_diamond_skip_price;
				}
			}
			else
			{
				result = this.sbsService.SbsConfig.challenges.balancing_v2.easy_diamond_skip_price;
			}
			return result;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x001146A1 File Offset: 0x00112AA1
		private void OnCollectButton()
		{
			this.collectButton.interactable = false;
			base.StartCoroutine(this.CollectPawsRoutine());
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x001146BC File Offset: 0x00112ABC
		private void HandleExhangeButton()
		{
			this.exchangePanel.gameObject.SetActive(true);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x001146CF File Offset: 0x00112ACF
		private void HandleCancelButton()
		{
			this.exchangePanel.gameObject.SetActive(false);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x001146E2 File Offset: 0x00112AE2
		private void HandlePurchaseExchangeButton()
		{
			base.StartCoroutine(this.HandleSkipRoutine(this.GetSkipPrice(base.challenge.difficulty), "exchange_challenge_task", base.challenge.id, true));
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x00114714 File Offset: 0x00112B14
		protected override IEnumerator CollectPawsRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			Materials rewards = new Materials
			{
				new MaterialAmount("paws", this.pawReward, MaterialAmountUsage.Undefined, 0),
				this.GetAdditionalReward()
			};
			this.trackingService.TrackChallengeResources("gained", base.challenge.difficulty.ToString(), rewards);
			this.gameStateService.Resources.AddPendingRewards(rewards, this.trackingService.TrackChallengeEvent("collect_paws", false, base.challenge, this.pawReward, 0));
			yield return TownRewardsRoot.ShowPendingRoutine();
			if (this.stateAnimation != null)
			{
				this.stateAnimation.Play(this.CompleteAnimationName);
				yield return new WaitForSeconds(this.stateAnimation[this.CompleteAnimationName].length);
			}
			this.HandleOnParent(ChallengeOperation.AddPaws);
			base.challenge.collected = true;
			base.challenge.CollectedTime = DateTime.UtcNow;
			this.Refresh();
			this.collectButton.interactable = true;
			yield break;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0011472F File Offset: 0x00112B2F
		private void HandleWatchAd()
		{
			this.watchAdButton.interactable = false;
			base.StartCoroutine(this.HandleWatchAdRoutine());
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0011474C File Offset: 0x00112B4C
		private IEnumerator HandleWatchAdRoutine()
		{
			yield return this.videoAdService.ShowAd(AdPlacement.Challenges_V2);
			this.RefreshAdStatus();
			yield return ServiceLocator.Instance.Inject(this);
			this.videoAdService.TrackClaim();
			this.HandleTimeReached();
			yield break;
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00114767 File Offset: 0x00112B67
		private void HandleSkip()
		{
			base.StartCoroutine(this.HandleSkipRoutine(this.challengeService.Balancing.diamond_skip_price, "challenges,", "get_now", false));
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00114794 File Offset: 0x00112B94
		private IEnumerator HandleSkipRoutine(int price, string detail1, string detail2, bool exchange = false)
		{
			PaymentFlow flow = new PaymentFlow(new TrackingService.PurchaseFlowContext
			{
				det1 = detail1,
				det2 = detail2,
				det3 = base.challenge.difficulty.ToString()
			}, new TownDiamondsPanelRoot.TownDiamondsPanelRootParameters
			{
				source1 = detail1,
				source2 = detail2
			});
			Wooroutine<bool> payment = flow.Start(new Materials(new List<MaterialAmount>
			{
				new MaterialAmount("diamonds", price, MaterialAmountUsage.Undefined, 0)
			}));
			yield return payment;
			if (payment.ReturnValue)
			{
				if (exchange)
				{
					Wooroutine<TrackingService> trackingServiceLoader = ServiceLocator.Instance.Await<TrackingService>(true);
					yield return trackingServiceLoader;
					this.trackingService = trackingServiceLoader.ReturnValue;
					if (this.trackingService != null)
					{
						this.trackingService.TrackChallengeEvent("exchange_task", true, base.challenge, this.pawReward, 0);
					}
				}
				this.ResetChallenge(!exchange);
			}
			yield break;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x001147CC File Offset: 0x00112BCC
		private void OnEnable()
		{
			if (this.isSetup)
			{
				this.RefreshAdStatus();
				this.Refresh();
			}
		}

		// Token: 0x0400607C RID: 24700
		private const float AD_WAIT_TIME_SECONDS = 60f;

		// Token: 0x0400607D RID: 24701
		[SerializeField]
		protected GameObject progressView;

		// Token: 0x0400607E RID: 24702
		[SerializeField]
		protected GameObject claimView;

		// Token: 0x0400607F RID: 24703
		[SerializeField]
		protected GameObject completedView;

		// Token: 0x04006080 RID: 24704
		[SerializeField]
		protected Button watchAdButton;

		// Token: 0x04006081 RID: 24705
		[SerializeField]
		protected Button skipWithDiamondsButton;

		// Token: 0x04006082 RID: 24706
		[SerializeField]
		protected Button exchangeDiamondButton;

		// Token: 0x04006083 RID: 24707
		[SerializeField]
		protected Button exchangeButton;

		// Token: 0x04006084 RID: 24708
		[SerializeField]
		protected Button cancelButton;

		// Token: 0x04006085 RID: 24709
		[SerializeField]
		protected TextMeshProUGUI exchangeLabel;

		// Token: 0x04006086 RID: 24710
		[SerializeField]
		protected GameObject exchangePanel;

		// Token: 0x04006087 RID: 24711
		[SerializeField]
		protected CountdownTimer nextChallengeTimer;

		// Token: 0x04006088 RID: 24712
		[SerializeField]
		protected MaterialAmountView additionalRewardView;

		// Token: 0x04006089 RID: 24713
		[SerializeField]
		protected TextMeshProUGUI diamondSkipPriceLabel;

		// Token: 0x0400608A RID: 24714
		private int challengeIndex;

		// Token: 0x0400608B RID: 24715
		private ChallengeProgressViewV2.AdButtonState currentAdButtonState;

		// Token: 0x0400608C RID: 24716
		private bool isSetup;

		// Token: 0x02000908 RID: 2312
		private enum AdButtonState
		{
			// Token: 0x0400608E RID: 24718
			None,
			// Token: 0x0400608F RID: 24719
			AdAvailable,
			// Token: 0x04006090 RID: 24720
			AdLoading,
			// Token: 0x04006091 RID: 24721
			PayWithdiamonds
		}
	}
}
