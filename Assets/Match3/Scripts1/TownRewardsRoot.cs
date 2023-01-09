using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200090D RID: 2317
namespace Match3.Scripts1
{
	public class TownRewardsRoot : APtSceneRoot<List<TownReward>>
	{
		// Token: 0x06003886 RID: 14470 RVA: 0x00115488 File Offset: 0x00113888
		public static IEnumerator ShowRoutine(List<TownReward> rewards)
		{
			Wooroutine<TownRewardsRoot> scene = SceneManager.Instance.LoadSceneWithParams<TownRewardsRoot, List<TownReward>>(rewards, null);
			yield return scene;
			yield return scene.ReturnValue.onDestroyed;
			yield break;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x001154A4 File Offset: 0x001138A4
		public static IEnumerator ShowPendingRoutine()
		{
			yield return TownRewardsRoot.ShowRoutine(null);
			yield break;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x001154B8 File Offset: 0x001138B8
		protected override void Go()
		{
			BackButtonManager.Instance.AddAction(new Action(this.OnClaimButtonClicked));
			this.claimButton.onClick.AddListener(new UnityAction(this.OnClaimButtonClicked));
			this.skipButton.onClick.AddListener(new UnityAction(this.OnSkipButtonClicked));
			foreach (MaterialAmountView materialAmountView in this.bluePrintViews)
			{
				materialAmountView.gameObject.SetActive(false);
			}
			foreach (MaterialAmountView materialAmountView2 in this.materialRewardViews)
			{
				materialAmountView2.gameObject.SetActive(false);
			}
			if (base.registeredFirst)
			{
				this.parameters = new List<TownReward>
				{
					new TownReward("diamonds", this.materialRewardViews[0].image.sprite, 15, false),
					new TownReward("coins", this.materialRewardViews[1].image.sprite, 100, false),
					new TownReward("coins", this.materialRewardViews[2].image.sprite, 100, false),
					new TownReward("coins", this.materialRewardViews[3].image.sprite, 100, false),
					new TownReward("coins", this.materialRewardViews[4].image.sprite, 100, false)
				};
			}
			base.StartCoroutine(this.AnimateRewardsRoutine());
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x001156B0 File Offset: 0x00113AB0
		private IEnumerator AnimateRewardsRoutine()
		{
			this.audioService.PlaySFX(AudioId.AdWheelYouWon, false, false, false);
			yield return this.PlayRewardAnimationRoutine("TownRewardsOpen");
			if (this.parameters != null)
			{
				int bluePrintIndex = 0;
				int otherRewardsIndex = 0;
				foreach (TownReward townReward in this.parameters)
				{
					this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
					if (townReward.blueprint)
					{
						if (bluePrintIndex < this.bluePrintViews.Count)
						{
							MaterialAmountView bluePrintView = this.bluePrintViews[bluePrintIndex];
							bluePrintView.gameObject.SetActive(true);
							bluePrintView.image.sprite = townReward.sprite;
							bluePrintView.label.text = townReward.amount.ToString();
							yield return this.PlayRewardAnimationRoutine(string.Format("Blueprint{0}Opening", bluePrintIndex + 1));
							townReward.viewIndex = bluePrintIndex;
							bluePrintIndex++;
						}
					}
					else if (otherRewardsIndex < this.materialRewardViews.Count)
					{
						MaterialAmountView materialRewardView = this.materialRewardViews[otherRewardsIndex];
						materialRewardView.gameObject.SetActive(true);
						if (townReward.type == null)
						{
							materialRewardView.Show(townReward.materialAmount);
						}
						else
						{
							materialRewardView.image.sprite = townReward.sprite;
							materialRewardView.label.text = townReward.amount.ToString();
						}
						yield return this.PlayRewardAnimationRoutine(string.Format("Reward{0}Opening", otherRewardsIndex + 1));
						townReward.viewIndex = otherRewardsIndex;
						otherRewardsIndex++;
					}
				}
			}
			else
			{
				yield return this.ShowPendingRewardsRoutine();
			}
			this.audioService.PlaySFX(AudioId.LevelWonClaimButtonAppears, false, false, false);
			yield return this.PlayRewardAnimationRoutine("TownRewardsButtonOpen");
			yield break;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x001156CC File Offset: 0x00113ACC
		private IEnumerator ShowPendingRewardsRoutine()
		{
			this.pendingRewards = this.gameStateService.Resources.GetPendingRewards();
			if (this.pendingRewards != null)
			{
				this.parameters = this.pendingRewards.AsTownRewards();
				for (int i = 0; i < this.pendingRewards.materials.Count; i++)
				{
					this.parameters[i].viewIndex = i;
					this.materialRewardViews[i].gameObject.SetActive(true);
					this.materialRewardViews[i].Show(this.pendingRewards.materials[i]);
					this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
					yield return this.PlayRewardAnimationRoutine(string.Format("Reward{0}Opening", i + 1));
				}
			}
			yield break;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x001156E8 File Offset: 0x00113AE8
		private IEnumerator PlayRewardAnimationRoutine(string name)
		{
			this.animations.Play(name);
			while (this.animations.isPlaying && !this.isSkipping)
			{
				yield return null;
			}
			if (this.isSkipping)
			{
				this.animations.wrapMode = WrapMode.Once;
				this.animations[name].normalizedTime = 1f;
				this.animations.Sample();
			}
			yield break;
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0011570C File Offset: 0x00113B0C
		private void OnClaimButtonClicked()
		{
			this.claimButton.interactable = false;
			BackButtonManager.Instance.RemoveAction(new Action(this.OnClaimButtonClicked));
			this.audioService.PlaySFX(AudioId.PopupHideDefault, false, false, false);
			base.StartCoroutine(this.AnimateOutRoutine());
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0011575C File Offset: 0x00113B5C
		private void OnSkipButtonClicked()
		{
			this.isSkipping = true;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x00115768 File Offset: 0x00113B68
		private IEnumerator AnimateOutRoutine()
		{
			MaterialAmountDisplayLabel[] displays = global::UnityEngine.Object.FindObjectsOfType<MaterialAmountDisplayLabel>();
			float waitTime = 1f;
			if (this.parameters != null)
			{
				Materials materials = new Materials();
				using (List<TownReward>.Enumerator enumerator = this.parameters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TownReward reward = enumerator.Current;
						if (!reward.blueprint)
						{
							if (reward.type == "lives_unlimited")
							{
								this.livesService.StartUnlimitedLives(reward.amount);
							}
							else if (reward.type.StartsWith("iso_"))
							{
								this.gameStateService.Buildings.StoreBuilding(this.configService.buildingConfigList.GetConfig(reward.type), reward.amount);
							}
							else
							{
								MaterialAmount materialAmount = new MaterialAmount(reward.type, reward.amount, MaterialAmountUsage.Undefined, 0);
								materials.Add(materialAmount);
								this.gameStateService.Resources.AddMaterial(materialAmount, true, "每日奖励");
							}
						}
						MaterialAmountDisplayLabel materialAmountDisplayLabel = Array.Find<MaterialAmountDisplayLabel>(displays, (MaterialAmountDisplayLabel d) => d.material == reward.type);
						if (materialAmountDisplayLabel != null)
						{
							Transform start = (!reward.blueprint) ? this.materialRewardViews[reward.viewIndex].image.transform : this.bluePrintViews[reward.viewIndex].image.transform;
							float num = this.doobers.SpawnDoobers(new MaterialAmount(reward.type, reward.amount, MaterialAmountUsage.Undefined, 0), start, materialAmountDisplayLabel.icon.transform, null);
							waitTime = ((num <= waitTime) ? waitTime : num);
						}
					}
				}
				if (this.pendingRewards == null)
				{
					this.trackingService.TrackChallengeResources("gained", "claim_box", materials);
				}
				else
				{
					this.gameStateService.Resources.RemovePendingRewards(this.pendingRewards);
					if (this.pendingRewards.trackingCall != null && this.pendingRewards.trackingCall.parametersJson != null)
					{
						this.trackingService.Track(this.pendingRewards.trackingCall);
					}
				}
			}
			yield return new WaitForSeconds(waitTime);
			this.animations.Play("TownRewardsClose");
			while (this.animations.isPlaying)
			{
				yield return null;
			}
			this.onClose.Dispatch();
			global::UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x040060C1 RID: 24769
		private const string OPEN_ANIMATION_NAME = "TownRewardsOpen";

		// Token: 0x040060C2 RID: 24770
		private const string BUTTON_OPEN_ANIMATION_NAME = "TownRewardsButtonOpen";

		// Token: 0x040060C3 RID: 24771
		private const string CLOSE_ANIMATION = "TownRewardsClose";

		// Token: 0x040060C4 RID: 24772
		public readonly AwaitSignal onClose = new AwaitSignal();

		// Token: 0x040060C5 RID: 24773
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040060C6 RID: 24774
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040060C7 RID: 24775
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x040060C8 RID: 24776
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040060C9 RID: 24777
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040060CA RID: 24778
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x040060CB RID: 24779
		[SerializeField]
		private Animation animations;

		// Token: 0x040060CC RID: 24780
		[SerializeField]
		private List<MaterialAmountView> bluePrintViews;

		// Token: 0x040060CD RID: 24781
		[SerializeField]
		private List<MaterialAmountView> materialRewardViews;

		// Token: 0x040060CE RID: 24782
		[SerializeField]
		private Button claimButton;

		// Token: 0x040060CF RID: 24783
		[SerializeField]
		private Button skipButton;

		// Token: 0x040060D0 RID: 24784
		private bool isSkipping;

		// Token: 0x040060D1 RID: 24785
		private PendingRewards pendingRewards;

		// Token: 0x0200090E RID: 2318
		public class Trigger : PopupManager.Trigger
		{
			// Token: 0x0600388F RID: 14479 RVA: 0x00115783 File Offset: 0x00113B83
			public Trigger(GameStateService gameState)
			{
				this.gameState = gameState;
			}

			// Token: 0x06003890 RID: 14480 RVA: 0x00115792 File Offset: 0x00113B92
			public override bool ShouldTrigger()
			{
				return this.gameState.Resources.HasPendingRewards();
			}

			// Token: 0x06003891 RID: 14481 RVA: 0x001157A4 File Offset: 0x00113BA4
			public override IEnumerator Run()
			{
				while (this.gameState.Resources.HasPendingRewards())
				{
					yield return TownRewardsRoot.ShowPendingRoutine();
				}
				yield break;
			}

			// Token: 0x040060D2 RID: 24786
			private GameStateService gameState;

			// Token: 0x040060D3 RID: 24787
			private TrackingService tracking;
		}
	}
}
