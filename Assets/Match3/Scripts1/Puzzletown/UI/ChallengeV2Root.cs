using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts1.Puzzletown.UI.Challenge;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000859 RID: 2137
	[LoadOptions(true, true, false)]
	public class ChallengeV2Root : APtSceneRoot, IDisposableDialog, IHandler<ChallengeOperation>
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x000FCB72 File Offset: 0x000FAF72
		// (set) Token: 0x060034CB RID: 13515 RVA: 0x000FCB7A File Offset: 0x000FAF7A
		public bool IsAnimating { get; protected set; }

		// Token: 0x060034CC RID: 13516 RVA: 0x000FCB84 File Offset: 0x000FAF84
		protected void SetupTest()
		{
			ChallengeDataService challenges = this.gameStateService.Challenges;
			foreach (ChallengeGoal challengeGoal in this.testData)
			{
				int collectedTotal = this.gameStateService.Resources.GetCollectedTotal(challengeGoal.type);
				challengeGoal.start = collectedTotal - challengeGoal.start;
				challengeGoal.lastViewed = challengeGoal.start + challengeGoal.lastViewed;
			}
			challenges.CurrentChallenges = this.testData;
			challenges.ChallengeExpireTime = DateTime.Now.AddHours(-2.0);
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000FCC48 File Offset: 0x000FB048
		protected void SetupChallenges()
		{
			this.challengeTab.Setup(this.gameStateService, this.challengeService, this.localizationService, this.sbsService, this.videoAdService, this.audioService);
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000FCC7C File Offset: 0x000FB07C
		protected override void Go()
		{
			this.dialog.Show();
			if (base.registeredFirst && this.useTestData)
			{
				this.SetupTest();
			}
			this.audioService.PlaySFX(AudioId.PopupShowDefault, false, false, false);
			if (this.gameStateService.Challenges.CurrentChallenges == null || this.gameStateService.Challenges.CurrentChallenges.Count < 3)
			{
				this.challengeService.AssignNextChallenges(-1);
				this.trackingService.TrackChallengeV2Unlocked(0);
				this.trackingService.TrackChallengeV2Unlocked(1);
				this.trackingService.TrackChallengeV2Unlocked(2);
			}
			if (this.gameStateService.Challenges.DecoSetExpireTime < DateTime.UtcNow)
			{
				this.challengeService.AssignNextDecoSet(false);
			}
			this.SetupChallenges();
			this.Refresh();
			WooroutineRunner.StartCoroutine(this.SetupRoutine(), null);
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000FCD68 File Offset: 0x000FB168
		private IEnumerator SetupRoutine()
		{
			this.tutorialMarker.enabled = true;
			yield return new WaitForEndOfFrame();
			this.SetPawsLabel(0, true);
			while (this.townMain.IsTutorialRunning)
			{
				yield return null;
			}
			int mysteryBoxPrice = this.challengeService.MysteryBoxPrice;
			int amountOfPawsInGamestate = this.gameStateService.Resources.GetAmount("paws");
			if (this.challengeService.IsMysteryBoxAvailable)
			{
				this.gameStateService.Resources.AddMaterial("paws", mysteryBoxPrice - amountOfPawsInGamestate, true);
				this.pawChange = new MaterialChange("paws", mysteryBoxPrice, mysteryBoxPrice);
				yield return this.AnimatePawsBarRoutine(this.pawChange);
			}
			else
			{
				this.SetPawsLabel(amountOfPawsInGamestate, true);
				this.AddSlowUpdate(new SlowUpdate(this.Refresh), 1);
				BackButtonManager.Instance.AddAction(new Action(this.CloseViaBackButton));
				this.gameStateService.Resources.onChanged.AddListener(new Action<MaterialChange>(this.HandleResourceChanged));
			}
			yield break;
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000FCD84 File Offset: 0x000FB184
		public void Handle(ChallengeOperation evt)
		{
			switch (evt)
			{
			case ChallengeOperation.None:
				break;
			case ChallengeOperation.Close:
				this.Close(true);
				break;
			case ChallengeOperation.WatchAd:
				base.StartCoroutine(this.ShowAd());
				break;
			default:
				if (evt != ChallengeOperation.Help)
				{
					if (evt != ChallengeOperation.AddPaws)
					{
						WoogaDebug.Log(new object[]
						{
							"Challenge operation not found " + evt
						});
					}
					else if (!string.IsNullOrEmpty(this.pawChange.name) && this.pawChange.name == "paws" && this.pawChange.after > this.pawChange.before)
					{
						this.RefreshInteraction();
						this.SetPawsLabel(this.pawChange.before, true);
						base.StartCoroutine(this.AnimatePawsBarRoutine(this.pawChange));
					}
				}
				else
				{
					SceneManager.Instance.LoadSceneWithParams<ChallengeV2InfoRoot, List<BuildingConfig>>(this.challengeDecoSet, null);
				}
				break;
			case ChallengeOperation.Play:
				this.CloseWithOptions(false, false);
				new CoreGameFlow().Start(default(CoreGameFlow.Input));
				break;
			case ChallengeOperation.OpenMysteryBox:
				base.StartCoroutine(this.OpenMysteryBox());
				break;
			}
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000FCEE0 File Offset: 0x000FB2E0
		private IEnumerator ShowAd()
		{
			yield return this.challengeTab.ShowAd();
			this.Refresh();
			yield break;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000FCEFB File Offset: 0x000FB2FB
		private void HandleResourceChanged(MaterialChange change)
		{
			if (change.name == "paws")
			{
				this.pawChange = change;
				this.IsAnimating = true;
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000FCF21 File Offset: 0x000FB321
		private void RefreshInteraction()
		{
			this.canvasGroup.interactable = !this.challengeService.IsMysteryBoxAvailable;
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000FCF3C File Offset: 0x000FB33C
		private IEnumerator AnimatePawsBarRoutine(MaterialChange change)
		{
			yield return new WaitForSeconds(0.5f);
			this.SetPawsLabel(Math.Min(change.after, this.challengeService.MysteryBoxPrice), false);
			float targetFill = (float)change.after / (float)this.challengeService.MysteryBoxPrice;
			targetFill = ((targetFill >= 1f) ? 1f : targetFill);
			float startFill = this.pawSlider.fillAmount;
			float elapsedTime = 0f;
			float ratio = 0f;
			while (ratio < 1f)
			{
				elapsedTime += Time.deltaTime;
				ratio = elapsedTime / 0.5f;
				this.pawSlider.fillAmount = Mathf.Lerp(startFill, targetFill, ratio);
				yield return null;
			}
			if (targetFill >= 1f)
			{
				yield return this.OpenMysteryBox();
			}
			this.pawChange = default(MaterialChange);
			yield return null;
			this.IsAnimating = false;
			yield break;
		}

		// 打开挑战盒子
		private IEnumerator OpenMysteryBox()
		{
			Wooroutine<ChallengeV2WheelRoot> scene = SceneManager.Instance.LoadSceneWithParams<ChallengeV2WheelRoot, List<BuildingConfig>>(this.challengeDecoSet, null);
			yield return scene;
			yield return scene.ReturnValue.onCompleted;
			BuildingConfig building = scene.ReturnValue.onCompleted.Dispatched;
			this.gameStateService.Resources.AddPendingRewards(new Materials(new List<MaterialAmount>
			{
				new MaterialAmount(building.name, 1, MaterialAmountUsage.Undefined, 0)
			}), this.trackingService.TrackChallengeEvent("open_paws", false, null, 0, 0));
			this.gameStateService.Resources.AddMaterial("paws", -this.challengeService.MysteryBoxPrice, true);
			yield return TownRewardsRoot.ShowPendingRoutine();
			this.SetPawsLabel(this.gameStateService.Resources.GetAmount("paws"), true);
			yield return WooroutineRunner.StartCoroutine(this.TryPlaceBuildingRoutine(building), null);
			yield break;
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000FCF7C File Offset: 0x000FB37C
		private void SetPawsLabel(int amount, bool fillSliderToo = true)
		{
			this.pawLabel.text = amount + "/" + this.challengeService.MysteryBoxPrice;
			if (fillSliderToo)
			{
				this.pawSlider.fillAmount = (float)amount / (float)this.challengeService.MysteryBoxPrice;
			}
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000FCFD4 File Offset: 0x000FB3D4
		public void Refresh()
		{
			this.RefreshInteraction();
			this.CheckAndUpdateDecoSets();
			this.challengeDecoSet.Clear();
			foreach (BuildingConfig buildingConfig in this.configService.buildingConfigList.buildings)
			{
				if (buildingConfig.challenge_set == this.gameStateService.Challenges.CurrentDecoSet)
				{
					this.challengeDecoSet.Add(buildingConfig);
				}
			}
			int num = 0;
			while (num < this.challengeDecoSet.Count && num < this.buildingImages.Count)
			{
				this.buildingImages[num].sprite = this.resourceService.GetWrappedSpriteOrPlaceholder(this.challengeDecoSet[num]).asset;
				this.buildingImages[num].gameObject.SetActive(true);
				num++;
			}
			this.challengeTab.Refresh(this.challengeService.IsChallengeRunning);
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000FD0CF File Offset: 0x000FB4CF
		private void CheckAndUpdateDecoSets()
		{
			if (this.gameStateService.Challenges.DecoSetExpireTime < DateTime.Now)
			{
				this.challengeService.AssignNextDecoSet(false);
			}
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000FD0FC File Offset: 0x000FB4FC
		private IEnumerator TryPlaceBuildingRoutine(BuildingConfig buildingConfig)
		{
			this.IsAnimating = false;
			this.Close(false);
			yield return new WaitForSeconds(this.dialog.close.length);
			yield return new ForceUserPlaceDecoFlow(buildingConfig).Start();
			yield break;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000FD11E File Offset: 0x000FB51E
		private void CloseViaBackButton()
		{
			this.CloseWithOptions(true, true);
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000FD128 File Offset: 0x000FB528
		private void Close(bool zoomOutAfter)
		{
			this.CloseWithOptions(true, zoomOutAfter);
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000FD134 File Offset: 0x000FB534
		private void CloseWithOptions(bool showAnimation, bool zoomOutAfter)
		{
			if (this.IsAnimating)
			{
				return;
			}
			this.IsAnimating = true;
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			BackButtonManager.Instance.RemoveAction(new Action(this.CloseViaBackButton));
			if (showAnimation)
			{
				this.dialog.Hide();
			}
			else
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			if (zoomOutAfter && BuildingLocation.Selected == null)
			{
				BlockerManager.global.Append(PanCameraFlow.CreateZoomOutFlow());
			}
		}

		// Token: 0x04005CC2 RID: 23746
		private const float PROGRESS_ANIMATION_TIME = 0.5f;

		// Token: 0x04005CC3 RID: 23747
		private const float INITIAL_DELAY = 0.5f;

		// Token: 0x04005CC4 RID: 23748
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x04005CC5 RID: 23749
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x04005CC6 RID: 23750
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005CC7 RID: 23751
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005CC8 RID: 23752
		[WaitForService(true, true)]
		private IVideoAdService videoAdService;

		// Token: 0x04005CC9 RID: 23753
		[WaitForService(true, true)]
		private ChallengeService challengeService;

		// Token: 0x04005CCA RID: 23754
		[WaitForService(true, true)]
		private ILocalizationService localizationService;

		// Token: 0x04005CCB RID: 23755
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005CCC RID: 23756
		[WaitForRoot(true, false)]
		private BuildingResourceServiceRoot resourceService;

		// Token: 0x04005CCD RID: 23757
		[WaitForRoot(false, false)]
		private TownMainRoot townMain;

		// Token: 0x04005CCE RID: 23758
		[SerializeField]
		private AnimatedUi dialog;

		// Token: 0x04005CCF RID: 23759
		[SerializeField]
		private ChallengesTab challengeTab;

		// Token: 0x04005CD0 RID: 23760
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04005CD1 RID: 23761
		[SerializeField]
		private TutorialMarker tutorialMarker;

		// Token: 0x04005CD2 RID: 23762
		[Header("V2 PawBar")]
		[SerializeField]
		private Image pawSlider;

		// Token: 0x04005CD3 RID: 23763
		[SerializeField]
		private TextMeshProUGUI pawLabel;

		// Token: 0x04005CD4 RID: 23764
		[SerializeField]
		private List<Image> buildingImages;

		// Token: 0x04005CD5 RID: 23765
		[Header("Testing")]
		[SerializeField]
		private bool useTestData;

		// Token: 0x04005CD6 RID: 23766
		[SerializeField]
		private List<ChallengeGoal> testData;

		// Token: 0x04005CD7 RID: 23767
		private MaterialChange pawChange;

		// Token: 0x04005CD8 RID: 23768
		private List<BuildingConfig> challengeDecoSet = new List<BuildingConfig>();
	}
}
