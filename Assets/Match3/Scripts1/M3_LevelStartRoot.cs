using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.UI;
using Match3.Scripts1.UnityEngine;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200072B RID: 1835
namespace Match3.Scripts1
{
	public class M3_LevelStartRoot : M3_ALevelStartRoot
	{
		// Token: 0x06002D7B RID: 11643 RVA: 0x000D38CC File Offset: 0x000D1CCC
		protected override void Go()
		{
			base.Go();
			this.saleNotificationView.Hide();
			if (base.registeredFirst)
			{
				new CoreGameFlow().Start(default(CoreGameFlow.Input));
				base.gameObject.SetActive(false);
			}
			this.ExecuteOnChildren(delegate(CreateUiGrabCamera grabber)
			{
				grabber.RefreshBakedImage();
			}, false);
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000D393C File Offset: 0x000D1D3C
		protected override IEnumerator GoRoutine()
		{
			yield return base.GoRoutine();
			Wooroutine<SpriteManager> spriteManagerRoutine = new SeasonSpriteManagerFlow().Start<SpriteManager>();
			yield return spriteManagerRoutine;
			if (spriteManagerRoutine.ReturnValue != null)
			{
				this.seasonSprite = spriteManagerRoutine.ReturnValue.GetSimilar("season_currency");
			}
			yield break;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000D3957 File Offset: 0x000D1D57
		protected override void OnEnable()
		{
			base.OnEnable();
			base.UpdateSalesBanner(this.saleNotificationView);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000D396C File Offset: 0x000D1D6C
		protected override void Show(LevelConfig level)
		{
			base.Show(level);
			this.masteredLabel.SetActive(this.progressionService.IsCompleted(this.parameters.LevelCollectionConfig.level));
			this.landscapeMasteredLabel.SetActive(this.progressionService.IsCompleted(this.parameters.LevelCollectionConfig.level));
			IEnumerable<M3LevelSelectionItemTier> tiers = this.SetupTiers(level);
			this.SetTierDataSource(this.tierDataSource, tiers);
			this.SetTierDataSource(this.landscapeTierDataSource, tiers);
			base.UpdateSalesBanner(this.saleNotificationView);
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000D39FA File Offset: 0x000D1DFA
		private void SetTierDataSource(M3_TierSelectionDataSource dataSource, IEnumerable<M3LevelSelectionItemTier> tiers)
		{
			dataSource.progressionService = this.progressionService;
			dataSource.selectedLevel = this.parameters.LevelCollectionConfig.level;
			dataSource.selectedTier = (int)this.parameters.SelectedTier;
			dataSource.Show(tiers);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000D3A38 File Offset: 0x000D1E38
		private IEnumerable<M3LevelSelectionItemTier> SetupTiers(LevelConfig level)
		{
			foreach (TierConfig tier in this.parameters.LevelCollectionConfig.tiers)
			{
				int index = (int)tier.tier;
				if (!this.AreTiersUnlocked() && index > 0)
				{
					yield break;
				}
				// QuestData questData = this.questService.questManager.CurrentQuestData;
				QuestData questData = null;
				string tierName = this.parameters.LevelCollectionConfig.tiers[index].name;
				string levelName = this.parameters.LevelCollectionConfig.level + tierName;
				MaterialAmount collectable = default(MaterialAmount);
				if (questData != null)
				{
					for (int j = 0; j < questData.Tasks.Count; j++)
					{
						QuestTaskData questTaskData = questData.Tasks[j];
						if (questTaskData.levels.Contains(levelName))
						{
							collectable = new MaterialAmount(questTaskData.item, 1, MaterialAmountUsage.Undefined, 0);
							break;
						}
					}
				}
				List<MaterialAmount> rewards = new List<MaterialAmount>
				{
					collectable
				};
				// int seasonalAmount = this.seasonService.GetEarnedMaterialForTier(index).amount;
				int seasonalAmount = 0;
				yield return new M3LevelSelectionItemTier
				{
					name = this.m3ConfigService.GetTierName(index),
					rewards = rewards,
					level = level,
					seasonSprite = this.seasonSprite,
					showSeasonRewards = (seasonalAmount > 0),
					seasonalCurrencyRewardAmount = seasonalAmount,
					diamonds = tier.diamonds,
					multiplier = this.configService.general.tier_factor[index].coin_multiplier,
					tier = index,
					// tournamentType = this.tournamentService.GetApparentOngoingTournamentType(),
					// tournamentPointMultiplier = this.tournamentService.GetCurrentScoreMultiplierForTier(index)
				};
			}
			yield break;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000D3A62 File Offset: 0x000D1E62
		private bool AreTiersUnlocked()
		{
			return this.progressionService.UnlockedLevel >= this.configService.general.tier_unlocked.unlock_tier_c_at_level;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000D3A8C File Offset: 0x000D1E8C
		protected override void SetupTournamentInfo()
		{
			// TournamentType apparentOngoingTournamentType = this.tournamentService.GetApparentOngoingTournamentType();
			TournamentType apparentOngoingTournamentType = TournamentType.Undefined;
			if (apparentOngoingTournamentType == TournamentType.Undefined)
			{
				this.tournamentInfoUI.gameObject.SetActive(false);
				this.landscapeTournamentInfoUI.gameObject.SetActive(false);
			}
			else
			{
				this.tournamentInfoUI.gameObject.SetActive(true);
				// this.tournamentInfoUI.Setup(apparentOngoingTournamentType, this.locaService, this.helpshiftService);
				this.landscapeTournamentInfoUI.gameObject.SetActive(true);
				// this.landscapeTournamentInfoUI.Setup(apparentOngoingTournamentType, this.locaService, this.helpshiftService);
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000D3B24 File Offset: 0x000D1F24
		protected override void SetupObjective(ALevelCollectionConfig config)
		{
			base.SetupObjective(config);
			this.landscapeImageObjective.sprite = this.goalSprites.GetSimilar(config.objective);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000D3B49 File Offset: 0x000D1F49
		protected override void SetTitle(string title)
		{
			base.SetTitle(title);
			this.landscapeLabelTitle.text = title;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000D3B5E File Offset: 0x000D1F5E
		protected override void ShowBoostData(BoostViewData[] boostsData)
		{
			base.ShowBoostData(boostsData);
			this.landscapeBoostsDataSource.Show(boostsData);
		}

		// Token: 0x04005715 RID: 22293
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionService;

		// Token: 0x04005716 RID: 22294
		// [WaitForService(true, true)]
		// private QuestService questService;

		// Token: 0x04005717 RID: 22295
		// [WaitForService(true, true)]
		// private SeasonService seasonService;

		// Token: 0x04005718 RID: 22296
		[SerializeField]
		private M3_TierSelectionDataSource tierDataSource;

		// Token: 0x04005719 RID: 22297
		[SerializeField]
		private GameObject masteredLabel;

		// Token: 0x0400571A RID: 22298
		[SerializeField]
		private SaleNotificationView saleNotificationView;

		// Token: 0x0400571B RID: 22299
		[Header("Landscape")]
		[SerializeField]
		private TextMeshProUGUI landscapeLabelTitle;

		// Token: 0x0400571C RID: 22300
		[SerializeField]
		private Image landscapeImageObjective;

		// Token: 0x0400571D RID: 22301
		[SerializeField]
		private BoostsDataSource landscapeBoostsDataSource;

		// Token: 0x0400571E RID: 22302
		[SerializeField]
		private TournamentInfoUI landscapeTournamentInfoUI;

		// Token: 0x0400571F RID: 22303
		[SerializeField]
		private M3_TierSelectionDataSource landscapeTierDataSource;

		// Token: 0x04005720 RID: 22304
		[SerializeField]
		private GameObject landscapeMasteredLabel;

		// Token: 0x04005721 RID: 22305
		private Sprite seasonSprite;
	}
}
