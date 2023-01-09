using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000707 RID: 1799
namespace Match3.Scripts1
{
	public class M3_CompletedWonRoot : APtSceneRoot<Match3Score, bool>, IHandler<PopupOperation>
	{
		// Token: 0x06002C8E RID: 11406 RVA: 0x000CD3A4 File Offset: 0x000CB7A4
		protected override void Go()
		{
			if (base.registeredFirst)
			{
				this.parameters = this.testScore;
				this.rewards = new Materials(new MaterialAmount[]
				{
					new MaterialAmount("diamonds", 10, MaterialAmountUsage.Undefined, 0),
					new MaterialAmount("coins", 180, MaterialAmountUsage.Undefined, 0),
					new MaterialAmount("axe", 1, MaterialAmountUsage.Undefined, 0)
				});
			}
			else
			{
				this.rewards = this.parameters.Rewards;
				TournamentScore tournamentScore = this.parameters.tournamentScore;
				this.tournamentPoints = new MaterialAmount(MaterialName.GetNameFor(tournamentScore.TournamentType), tournamentScore.CollectedPoints, MaterialAmountUsage.Undefined, 0);
				this.bankedDiamonds = new MaterialAmount("bank_diamonds", this.parameters.bankedDiamonds, MaterialAmountUsage.Undefined, 0);
				int level = this.parameters.Config.LevelCollectionConfig.level;
				if (level == 1)
				{
					// this.tracking.TrackFunnelEvent("070_01_level_completed", 70, null);
				}
				else if (level == 2)
				{
					// this.tracking.TrackFunnelEvent("125_02_level_completed", 125, null);
				}
				else if (level == 3)
				{
					// this.tracking.TrackFunnelEvent("190_03_level_completed", 190, null);
				}
				else if (level == 4)
				{
					// this.tracking.TrackFunnelEvent("220_04_level_completed", 220, null);
				}
				else if (level == 5)
				{
					// this.tracking.TrackFunnelEvent("255_05_level_completed", 255, null);
				}
				else if (level == 6)
				{
					// this.tracking.TrackFunnelEvent("325_06_level_completed", 325, null);
				}
				else if (level == 7)
				{
					// this.tracking.TrackFunnelEvent("345_07_level_completed", 345, null);
				}
				else if (level == 8)
				{
					// this.tracking.TrackFunnelEvent("365_08_level_completed", 365, null);
				}
				else if (level == 9)
				{
					// this.tracking.TrackFunnelEvent("390_09_level_completed", 390, null);
				}
				else if (level == 10)
				{
					// this.tracking.TrackFunnelEvent("470_10_level_completed", 470, null);
				}
				else if (level == 11)
				{
					// this.tracking.TrackFunnelEvent("535_11_level_completed", 535, null);
				}
				// this.externalGamesService.ShowLevelAchievement(level);
				int num = (this.parameters.Config.SelectedTier != AreaConfig.Tier.c) ? 0 : 1;
				// this.externalGamesService.ShowLevelMasteryAchievement(this.progressionDataService.NumberOfCompletedLevelsAtTier(2) + num);
			}
			BackButtonManager.Instance.AddAction(new Action(this.HandleButtonClose));
			this.HideViews();
			this.audioService.PlaySFX(AudioId.LevelCompleted, false, false, false);
			this.canvasGroup.interactable = true;
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000CD68C File Offset: 0x000CBA8C
		protected override IEnumerator GoRoutine()
		{
			// Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			// yield return spriteManagerRoutine;
			// this.seasonSpriteManager = spriteManagerRoutine.ReturnValue;
			this.ApplyRewards();
			this.ApplyTournamentPoints();
			this.SetupButtons();
			base.StartCoroutine(this.Animate());
			yield break;
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000CD6A8 File Offset: 0x000CBAA8
		private bool UpdateResources()
		{
			TownResourcePanelRoot townResourcePanelRoot = global::UnityEngine.Object.FindObjectOfType<TownResourcePanelRoot>();
			if (townResourcePanelRoot)
			{
				foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in townResourcePanelRoot.GetComponentsInChildren<MaterialAmountDisplayLabel>())
				{
					foreach (MaterialAmount materialAmount in this.rewards)
					{
						if (materialAmount.type == materialAmountDisplayLabel.material)
						{
							materialAmountDisplayLabel.allowReserving = true;
							materialAmountDisplayLabel.ReserveAmount(materialAmount.amount);
							materialAmountDisplayLabel.allowReserving = false;
						}
					}
				}
			}
			return townResourcePanelRoot != null;
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000CD76C File Offset: 0x000CBB6C
		private void SetupButtons()
		{
			// this.indicatorQuests.gameObject.SetActive(this.questService.HasNotification());
			this.buttonVillage.onClick.AddListener(new UnityAction(this.HandleButtonClose));
			this.buttonNext.onClick.AddListener(new UnityAction(this.HandleButtonNext));
			int unlockedLevel = this.progressionDataService.UnlockedLevel;
			bool active = this.parameters.levelPlayMode == LevelPlayMode.Regular && this.configService.FeatureSwitchesConfig.next_level_button && !this.HasIslandTutorialForLevel(unlockedLevel) && !this.CanCompleteQuest(unlockedLevel);
			this.buttonNext.transform.parent.gameObject.SetActive(active);
			string key = (!this.configService.FeatureSwitchesConfig.next_level_button) ? "ui.level.win.button" : "ui.level.back.button";
			this.labelButtonVillage.text = this.locaService.GetText(key, new LocaParam[0]);
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000CD874 File Offset: 0x000CBC74
		private bool CanCompleteQuest(int currentLevel)
		{
			// bool flag = !this.questService.IsLevelUnlocked(currentLevel);
			// int numberOpenTasks = this.questService.GetNumberOpenTasks();
			// bool flag2 = numberOpenTasks == 0;
			// bool flag3 = numberOpenTasks == 1 && this.questService.HasOpenVRTask();
			// return flag && (flag2 || flag3);
			return true;
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000CD8CC File Offset: 0x000CBCCC
		private bool HasIslandTutorialForLevel(int currentLevel)
		{
			Tutorial tutorial = this.islandTutorialList.tutorials.FirstOrDefault((Tutorial t) => t.level == currentLevel);
			if (tutorial != null)
			{
				if (tutorial.forceReturnToIsland)
				{
					return true;
				}
				this.progressionDataService.CompleteTutorial(tutorial.name);
			}
			return false;
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000CD930 File Offset: 0x000CBD30
		private void HideViews()
		{
			this.difficultyBonus.Hide();
			this.diamondsView.Hide();
			this.coinsView.Hide();
			this.collectablesView.Hide();
			this.extraRewardsView.Hide();
			this.tournamentPointsView.Hide();
			this.tournamentDifficultyBonus.Hide();
			this.seasonalCurrencyView.Hide();
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000CD998 File Offset: 0x000CBD98
		private void ApplyRewards()
		{
			foreach (MaterialAmount materialAmount in this.rewards)
			{
				if (materialAmount.amount > 0)
				{
					string type = materialAmount.type;
					if (type != null)
					{
						if (type == "diamonds")
						{
							this.diamondsView.Show(new MaterialAmount("diamonds", this.rewards["diamonds"], MaterialAmountUsage.Undefined, 0));
							continue;
						}
						if (type == "coins")
						{
							this.coinsView.Show(new MaterialAmount("coins", this.rewards["coins"], MaterialAmountUsage.Undefined, 0));
							continue;
						}
						if (type == "earned_season_currency")
						{
							this.seasonalCurrencyView.manager = this.seasonSpriteManager;
							this.seasonalCurrencyView.Show(new MaterialAmount("season_currency", this.rewards["earned_season_currency"], MaterialAmountUsage.Undefined, 0));
							continue;
						}
						if (type == "UnlimitedLives" || type == "lives_unlimited")
						{
							this.extraReward = new MaterialAmount(materialAmount.type, materialAmount.amount, MaterialAmountUsage.Undefined, 0);
							this.extraRewardsView.Show(new MaterialAmount("lives_unlimited", materialAmount.amount, MaterialAmountUsage.Undefined, 0));
							continue;
						}
					}
					if (materialAmount.type.StartsWith("boost_"))
					{
						this.diamondsView.Show(new MaterialAmount(materialAmount.type, materialAmount.amount, MaterialAmountUsage.Undefined, 0));
						this.animateDiamondRewardView = true;
					}
					else
					{
						this.collectableReward = new MaterialAmount(materialAmount.type, materialAmount.amount, MaterialAmountUsage.Undefined, 0);
						this.collectablesView.Show(this.collectableReward);
					}
				}
			}
			if (this.parameters.bankedDiamonds > 0)
			{
				this.bankDiamondsView.Show();
			}
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000CDBD0 File Offset: 0x000CBFD0
		private void ApplyTournamentPoints()
		{
			if (this.tournamentPoints.amount > 0)
			{
				this.tournamentPointsView.Show(new MaterialAmount(this.tournamentPoints.type, this.tournamentPoints.amount, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000CDC0C File Offset: 0x000CC00C
		private AreaConfig.Tier GetTier()
		{
			if (this.parameters.Config == null || (this.parameters.Config.IsCompleted && this.parameters.levelPlayMode == LevelPlayMode.Regular))
			{
				return AreaConfig.Tier.a;
			}
			return this.parameters.Config.SelectedTier;
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000CDC60 File Offset: 0x000CC060
		private IEnumerator Animate()
		{
			while (!this.UpdateResources())
			{
				yield return null;
			}
			this.diamondsView.canvasGroup.alpha = 0f;
			this.coinsView.canvasGroup.alpha = 0f;
			this.collectablesView.canvasGroup.alpha = 0f;
			this.extraRewardsView.canvasGroup.alpha = 0f;
			this.tournamentPointsView.canvasGroup.alpha = 0f;
			this.seasonalCurrencyView.canvasGroup.alpha = 0f;
			AreaConfig.Tier tier = this.GetTier();
			int tierInt = (int)(tier + 1);
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			yield return this.AnimateDiamonds();
			yield return this.AnimateCoins(tierInt);
			yield return this.AnimateTournamentPoints(tierInt);
			yield return this.AnimateSeasonalView();
			yield return this.AnimateOtherRewardViews();
			yield return this.AnimateBankedDiamondsRewards();
			this.audioService.PlaySFX(AudioId.BannerShowDefault, false, false, false);
			yield return this.PlayAnimation("ButtonCollectOpen");
			yield break;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000CDC7C File Offset: 0x000CC07C
		private IEnumerator AnimateDiamonds()
		{
			if (this.rewards["diamonds"] > 0 || this.animateDiamondRewardView)
			{
				this.diamondsView.Show();
				this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
				yield return this.PlayAnimation("DiamondRewardOpening");
			}
			yield break;
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000CDC98 File Offset: 0x000CC098
		private IEnumerator AnimateOtherRewardViews()
		{
			foreach (MaterialAmount reward in this.rewards)
			{
				if (reward.amount > 0)
				{
					if (reward.type == this.collectableReward.type)
					{
						this.collectablesView.Show();
						this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
						yield return this.PlayAnimation("SpecialItemOpening");
					}
					if (reward.type == this.extraReward.type)
					{
						this.extraRewardsView.Show();
						this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
						yield return this.PlayAnimation("SpecialRewardOpening");
					}
				}
			}
			yield break;
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000CDCB4 File Offset: 0x000CC0B4
		private IEnumerator AnimateSeasonalView()
		{
			foreach (MaterialAmount reward in this.rewards)
			{
				if (reward.amount > 0 && reward.type == "earned_season_currency")
				{
					this.seasonalCurrencyView.Show();
					this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
					yield return this.PlayAnimation("SeasonalRewardOpening");
				}
			}
			yield break;
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000CDCD0 File Offset: 0x000CC0D0
		private IEnumerator AnimateCoins(int tierInt)
		{
			if (this.rewards["coins"] > 0)
			{
				this.audioService.PlaySFX(AudioId.LevelWonRewardItemAppears, false, false, false);
				this.coinsView.Show(new MaterialAmount("coins", this.rewards["coins"] / tierInt, MaterialAmountUsage.Undefined, 0));
				int amount = this.rewards["coins"];
				yield return this.PlayAnimation("CoinRewardOpening");
				if (tierInt > 1)
				{
					string multiplier = string.Format("_x{0}", tierInt);
					this.difficultyBonus.image.sprite = this.difficultyBonus.manager.GetSimilar(multiplier);
					this.audioService.PlaySFX(AudioId.LevelWonBonusBannerAppears, false, false, false);
					yield return this.PlayAnimation("CoinRewardMultiplierOpening");
					this.windowAnimation.Play("CoinRewardMultiplierIncreasing");
					this.windowAnimation.wrapMode = WrapMode.Loop;
					float time = Time.time;
					float cointupTime = (float)(amount - amount / tierInt) / 75f;
					while (time + cointupTime > Time.time)
					{
						if (this.isSkipping)
						{
							break;
						}
						float t = Time.time - time;
						float i = t / cointupTime;
						this.coinsView.label.text = ((int)Mathf.Lerp((float)(amount / tierInt), (float)amount, i)).ToString();
						yield return null;
					}
					this.coinsView.label.text = amount.ToString();
					this.windowAnimation.Stop();
				}
			}
			yield break;
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000CDCF4 File Offset: 0x000CC0F4
		private IEnumerator AnimateTournamentPoints(int tierInt)
		{
			if (this.tournamentPoints.amount > 0)
			{
				// int multiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(tierInt - 1);
				int multiplier = 1;
				int amount = this.tournamentPoints.amount;
				int fullAmount = amount * multiplier;
				this.tournamentPointsView.Show();
				yield return this.PlayAnimation("TournamentRewardOpening");
				if (multiplier > 1)
				{
					string multiplierAsString = string.Format("_x{0}", multiplier);
					this.tournamentDifficultyBonus.image.sprite = this.tournamentDifficultyBonus.manager.GetSimilar(multiplierAsString);
					yield return this.PlayAnimation("TournamentRewardMultiplierOpening");
					this.windowAnimation.Play("TournamentRewardMultiplierIncreasing");
					this.windowAnimation.wrapMode = WrapMode.Loop;
					float time = Time.time;
					while (time + 1f > Time.time)
					{
						if (this.isSkipping)
						{
							break;
						}
						float elapsedTime = Time.time - time;
						float lerpFactor = elapsedTime / 1f;
						this.tournamentPointsView.label.text = ((int)Mathf.Lerp((float)amount, (float)fullAmount, lerpFactor)).ToString();
						yield return null;
					}
					this.tournamentPointsView.label.text = fullAmount.ToString();
					this.windowAnimation.Stop();
				}
			}
			yield break;
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000CDD18 File Offset: 0x000CC118
		private IEnumerator AnimateBankedDiamondsRewards()
		{
			if (this.bankedDiamonds.amount > 0)
			{
				this.bankDiamondsView.Show();
				this.bankDiamondsView.label.text = this.bankedDiamonds.amount.ToString();
				yield return this.PlayAnimation("BankedDiamondsRewardOpening");
			}
			yield break;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000CDD34 File Offset: 0x000CC134
		private IEnumerator PlayAnimation(string name)
		{
			this.currentAnimation = name;
			this.windowAnimation.Play(name);
			this.windowAnimation.wrapMode = WrapMode.Once;
			if (this.isSkipping)
			{
				this.windowAnimation[this.currentAnimation].normalizedTime = 1f;
				this.windowAnimation.Sample();
				yield break;
			}
			while (this.windowAnimation.isPlaying)
			{
				yield return null;
			}
			this.windowAnimation.wrapMode = WrapMode.Default;
			yield break;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000CDD56 File Offset: 0x000CC156
		protected override void OnDisable()
		{
			BackButtonManager.Instance.RemoveAction(new Action(this.HandleButtonClose));
			base.OnDisable();
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000CDD74 File Offset: 0x000CC174
		private void CollectAndClose(bool showLevelMap)
		{
			this.onCompleted.Dispatch(showLevelMap);
			base.Disable();
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000CDD88 File Offset: 0x000CC188
		private void HandleButtonNext()
		{
			this.Close(true);
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000CDD91 File Offset: 0x000CC191
		private void HandleButtonClose()
		{
			this.Close(false);
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000CDD9C File Offset: 0x000CC19C
		private void Close(bool showLevelMap)
		{
			BackButtonManager.Instance.SetEnabled(false);
			if (!base.registeredFirst)
			{
				int level = this.parameters.Config.LevelCollectionConfig.level;
				if (level == 1)
				{
					// this.tracking.TrackFunnelEvent("075_01_claim_reward", 55, null);
				}
				else if (level == 2)
				{
					// this.tracking.TrackFunnelEvent("130_02_claim_reward", 130, null);
				}
				else if (level == 3)
				{
					// this.tracking.TrackFunnelEvent("195_03_claim_reward", 195, null);
				}
				else if (level == 4)
				{
					// this.tracking.TrackFunnelEvent("225_04_claim_reward", 225, null);
				}
				else if (level == 5)
				{
					// this.tracking.TrackFunnelEvent("260_05_claim_reward", 260, null);
				}
				else if (level == 6)
				{
					// this.tracking.TrackFunnelEvent("330_06_claim_reward", 330, null);
				}
				else if (level == 7)
				{
					// this.tracking.TrackFunnelEvent("350_07_claim_reward", 350, null);
				}
				else if (level == 8)
				{
					// this.tracking.TrackFunnelEvent("370_08_claim_reward", 370, null);
				}
				else if (level == 9)
				{
					// this.tracking.TrackFunnelEvent("395_09_claim_reward", 395, null);
				}
				else if (level == 10)
				{
					// this.tracking.TrackFunnelEvent("475_10_claim_reward", 475, null);
				}
				else if (level == 11)
				{
					// this.tracking.TrackFunnelEvent("540_11_claim_reward", 540, null);
				}
			}
			TownResourcePanelRoot townResourcePanelRoot = global::UnityEngine.Object.FindObjectOfType<TownResourcePanelRoot>();
			if (townResourcePanelRoot)
			{
				foreach (MaterialAmountDisplayLabel materialAmountDisplayLabel in townResourcePanelRoot.GetComponentsInChildren<MaterialAmountDisplayLabel>())
				{
					materialAmountDisplayLabel.allowReserving = false;
				}
			}
			MaterialAmountDisplayLabel[] array = global::UnityEngine.Object.FindObjectsOfType<MaterialAmountDisplayLabel>();
			using (IEnumerator<MaterialAmount> enumerator = this.rewards.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MaterialAmount reward = enumerator.Current;
					WoogaDebug.Log(new object[]
					{
						"reward collected:",
						reward.type,
						reward.amount
					});
					if (reward.amount > 0)
					{
						MaterialAmountDisplayLabel materialAmountDisplayLabel2 = Array.Find<MaterialAmountDisplayLabel>(array, (MaterialAmountDisplayLabel d) => d.material == reward.type);
						if (materialAmountDisplayLabel2)
						{
							string type = reward.type;
							if (type != null)
							{
								if (!(type == "diamonds"))
								{
									if (!(type == "coins"))
									{
										if (type == "earned_season_currency")
										{
											this.doobers.SpawnDoobers(new MaterialAmount("season_currency", reward.amount, MaterialAmountUsage.Undefined, 0), this.seasonalCurrencyView.transform, materialAmountDisplayLabel2.icon.transform, null);
										}
									}
									else
									{
										this.doobers.SpawnDoobers(reward, this.coinsView.transform, materialAmountDisplayLabel2.icon.transform, null);
									}
								}
								else
								{
									this.doobers.SpawnDoobers(reward, this.diamondsView.transform, materialAmountDisplayLabel2.icon.transform, null);
								}
							}
						}
					}
				}
			}
			base.StartCoroutine(this.TryToCollectAndClose(showLevelMap));
			this.canvasGroup.interactable = false;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000CE168 File Offset: 0x000CC568
		private IEnumerator TryToCollectAndClose(bool showLevelMap)
		{
			yield return null;
			while (Doober.ActiveDoobers > 0)
			{
				yield return null;
			}
			this.CollectAndClose(showLevelMap);
			yield break;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000CE18C File Offset: 0x000CC58C
		public void Handle(PopupOperation op)
		{
			if (op != PopupOperation.OK)
			{
				if (op == PopupOperation.Skip)
				{
					this.isSkipping = true;
					if (!string.IsNullOrEmpty(this.currentAnimation))
					{
						this.windowAnimation[this.currentAnimation].normalizedTime = 1f;
					}
				}
			}
		}

		// Token: 0x040055E2 RID: 21986
		private const string CoinRewardMultiplierOpening = "CoinRewardMultiplierOpening";

		// Token: 0x040055E3 RID: 21987
		private const string CoinRewardMultiplierIncreasing = "CoinRewardMultiplierIncreasing";

		// Token: 0x040055E4 RID: 21988
		private const string CoinRewardOpening = "CoinRewardOpening";

		// Token: 0x040055E5 RID: 21989
		private const string DiamondRewardOpening = "DiamondRewardOpening";

		// Token: 0x040055E6 RID: 21990
		private const string CollectableRewardOpening = "SpecialItemOpening";

		// Token: 0x040055E7 RID: 21991
		private const string SpecialRewardOpening = "SpecialRewardOpening";

		// Token: 0x040055E8 RID: 21992
		private const string SeasonalRewardOpening = "SeasonalRewardOpening";

		// Token: 0x040055E9 RID: 21993
		private const string TournamentRewardOpening = "TournamentRewardOpening";

		// Token: 0x040055EA RID: 21994
		private const string TournamentMultiplierOpening = "TournamentRewardMultiplierOpening";

		// Token: 0x040055EB RID: 21995
		private const string TournamentMultiplierIncreasing = "TournamentRewardMultiplierIncreasing";

		// Token: 0x040055EC RID: 21996
		private const string ButtonCollectOpen = "ButtonCollectOpen";

		// Token: 0x040055ED RID: 21997
		private const string BankedDiamondsRewardOpening = "BankedDiamondsRewardOpening";

		// Token: 0x040055EE RID: 21998
		private const float COINS_COUNTUP_SPEED = 75f;

		// Token: 0x040055EF RID: 21999
		private const float TOURNAMENT_COUNT_UP_TIME_SECONDS = 1f;

		// Token: 0x040055F0 RID: 22000
		private bool isSkipping;

		// Token: 0x040055F1 RID: 22001
		private string currentAnimation;

		// Token: 0x040055F2 RID: 22002
		private SpriteManager seasonSpriteManager;

		// Token: 0x040055F3 RID: 22003
		private Materials rewards;

		// Token: 0x040055F4 RID: 22004
		private MaterialAmount tournamentPoints;

		// Token: 0x040055F5 RID: 22005
		private MaterialAmount bankedDiamonds;

		// Token: 0x040055F6 RID: 22006
		private MaterialAmount collectableReward;

		// Token: 0x040055F7 RID: 22007
		private MaterialAmount extraReward;

		// Token: 0x040055F8 RID: 22008
		private bool animateDiamondRewardView;

		// Token: 0x040055F9 RID: 22009
		[WaitForRoot(false, false)]
		private DoobersRoot doobers;

		// Token: 0x040055FA RID: 22010
		[WaitForService(true, true)]
		private AudioService audioService;

		// Token: 0x040055FB RID: 22011
		// [WaitForService(true, true)]
		// private TrackingService tracking;

		// Token: 0x040055FC RID: 22012
		[WaitForService(true, true)]
		private ILocalizationService locaService;

		// Token: 0x040055FD RID: 22013
		[WaitForService(true, true)]
		private M3ConfigService m3ConfigService;

		// Token: 0x040055FE RID: 22014
		[WaitForService(true, true)]
		private LivesService livesService;

		// Token: 0x040055FF RID: 22015
		// [WaitForService(true, true)]
		// private TournamentService tournamentService;

		// Token: 0x04005600 RID: 22016
		// [WaitForService(true, true)]
		// private ExternalGamesService externalGamesService;

		// Token: 0x04005601 RID: 22017
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x04005602 RID: 22018
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005603 RID: 22019
		// [WaitForService(true, true)]
		// private QuestService questService;

		// Token: 0x04005604 RID: 22020
		[SerializeField]
		private TutorialList islandTutorialList;

		// Token: 0x04005605 RID: 22021
		[SerializeField]
		private Button buttonNext;

		// Token: 0x04005606 RID: 22022
		[SerializeField]
		private Button buttonVillage;

		// Token: 0x04005607 RID: 22023
		[SerializeField]
		private TextMeshProUGUI labelButtonVillage;

		// Token: 0x04005608 RID: 22024
		[SerializeField]
		private UiIndicator indicatorQuests;

		// Token: 0x04005609 RID: 22025
		[SerializeField]
		private MaterialAmountView diamondsView;

		// Token: 0x0400560A RID: 22026
		[SerializeField]
		private MaterialAmountView coinsView;

		// Token: 0x0400560B RID: 22027
		[SerializeField]
		private MaterialAmountView collectablesView;

		// Token: 0x0400560C RID: 22028
		[SerializeField]
		private MaterialAmountView extraRewardsView;

		// Token: 0x0400560D RID: 22029
		[SerializeField]
		private MaterialAmountView difficultyBonus;

		// Token: 0x0400560E RID: 22030
		[SerializeField]
		private MaterialAmountView seasonalCurrencyView;

		// Token: 0x0400560F RID: 22031
		[SerializeField]
		private MaterialAmountView tournamentPointsView;

		// Token: 0x04005610 RID: 22032
		[SerializeField]
		private MaterialAmountView tournamentDifficultyBonus;

		// Token: 0x04005611 RID: 22033
		[SerializeField]
		private MaterialAmountView bankDiamondsView;

		// Token: 0x04005612 RID: 22034
		[SerializeField]
		private Match3Score testScore;

		// Token: 0x04005613 RID: 22035
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04005614 RID: 22036
		[SerializeField]
		private Animation windowAnimation;

		// Token: 0x04005615 RID: 22037
		private TextMeshProUGUI[] labels;
	}
}
