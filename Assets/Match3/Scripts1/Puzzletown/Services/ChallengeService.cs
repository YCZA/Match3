using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200075F RID: 1887
	public class ChallengeService : AService
	{
		// Token: 0x06002EBC RID: 11964 RVA: 0x000DA250 File Offset: 0x000D8650
		public ChallengeService()
		{
			WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000DA268 File Offset: 0x000D8668
		public int NumCompletedChallenges
		{
			get
			{
				if (this.gameStateService.Challenges == null || this.gameStateService.Challenges.CurrentChallenges == null)
				{
					return 0;
				}
				return this.gameStateService.Challenges.CurrentChallenges.Count(new Func<ChallengeGoal, bool>(this.IsChallengeReadyToCollect));
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x000DA2C0 File Offset: 0x000D86C0
		public int NumAvailableDecos
		{
			get
			{
				if (this.gameStateService.Challenges == null || this.gameStateService.Challenges.BuildingProgress == null)
				{
					return 0;
				}
				return this.gameStateService.Challenges.BuildingProgress.Count((ChallengeBuildingProgress bp) => bp.IsCompleted);
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x000DA326 File Offset: 0x000D8726
		public bool IsChallengeRunning
		{
			get
			{
				return this.gameStateService.Challenges != null;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x000DA33B File Offset: 0x000D873B
		public int MysteryBoxPrice
		{
			get
			{
				return this.Balancing.mystery_box_paw_price;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x000DA348 File Offset: 0x000D8748
		public bool IsMysteryBoxAvailable
		{
			get
			{
				return this.gameStateService.Resources.GetAmount("paws") >= this.MysteryBoxPrice;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x000DA36A File Offset: 0x000D876A
		public ChallengesConfig.Balancing Balancing
		{
			get
			{
				return this.sbsService.SbsConfig.challenges.balancing_v2;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x000DA381 File Offset: 0x000D8781
		private List<ChallengeConfig> Definitions
		{
			get
			{
				return this.sbsService.SbsConfig.challenges.challenge_definitions_v2;
			}
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000DA398 File Offset: 0x000D8798
		private IEnumerator InitRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			base.OnInitialized.Dispatch();
			bool decoSetOutOfRange = this.gameStateService.Challenges.CurrentDecoSet < 5;
			if (this.gameStateService.Challenges.DecoSetExpireTime < DateTime.Now || decoSetOutOfRange)
			{
				this.AssignNextDecoSet(false);
			}
			WooroutineRunner.StartCoroutine(this.CheckForBundleRoutine(), null);
			yield break;
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000DA3B4 File Offset: 0x000D87B4
		private IEnumerator CheckForBundleRoutine()
		{
			while (!this.isAssetBundleAvailable)
			{
				Wooroutine<bool> isAvailable = this.assetBundleService.IsBundleAvailable("buildings_challenges_2018");
				yield return isAvailable;
				this.isAssetBundleAvailable = isAvailable.ReturnValue;
			}
			yield break;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000DA3D0 File Offset: 0x000D87D0
		private bool IsTimeOver()
		{
			return (this.gameStateService.Challenges.ChallengeExpireTime - DateTime.UtcNow).TotalSeconds < 1.0;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000DA40C File Offset: 0x000D880C
		public void ExtendTime()
		{
			this.gameStateService.Challenges.ChallengeExpireTime = DateTime.UtcNow.AddMinutes((double)this.Balancing.challenge_extend_minutes);
			this.gameStateService.Challenges.NumberOfBoughtExtentions++;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000DA45A File Offset: 0x000D885A
		public bool ShouldShowChallengeIconBadge()
		{
			return this.NumCompletedChallenges > 0 && this.isAssetBundleAvailable;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000DA474 File Offset: 0x000D8874
		public int GetPawReward(ChallengeGoal challenge)
		{
			int num = this.Balancing.paw_reward_easy;
			ChallengeGoal.ChallengeDifficulty difficulty = challenge.difficulty;
			if (difficulty != ChallengeGoal.ChallengeDifficulty.easy)
			{
				if (difficulty != ChallengeGoal.ChallengeDifficulty.medium)
				{
					if (difficulty == ChallengeGoal.ChallengeDifficulty.hard)
					{
						num = this.Balancing.paw_reward_hard;
					}
				}
				else
				{
					num = this.Balancing.paw_reward_medium;
				}
			}
			else
			{
				num = this.Balancing.paw_reward_easy;
			}
			return num + (int)((float)(num * this.gameStateService.Challenges.CurrentAdBonus) * 0.1f);
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000DA500 File Offset: 0x000D8900
		public void AssignNextChallenges(int index = -1)
		{
			List<ChallengeConfig> list = new List<ChallengeConfig>(from challenge in this.Definitions
			where challenge.unlock_level <= this.progressionDataService.UnlockedLevel
			select challenge);
			List<ChallengeConfig> list2 = new List<ChallengeConfig>();
			if (this.gameStateService.Challenges.CurrentChallenges != null && this.gameStateService.Challenges.CurrentChallenges.Count > 0)
			{
				List<ChallengeConfig> list3 = new List<ChallengeConfig>(list);
				list3.RemoveAll((ChallengeConfig challenge) => this.gameStateService.Challenges.CurrentChallenges.Exists((ChallengeGoal goal) => goal.id == challenge.id));
				int i;
				for (i = 0; i < 3; i++)
				{
					if (index < 0)
					{
						ChallengeConfig item = (from c in list3
						where this.IsValid(c, i)
						select c).ToList<ChallengeConfig>().RandomElement(false);
						list2.Add(item);
						list3.Remove(item);
					}
					else if (i == index)
					{
						ChallengeConfig item = (from c in list3
						where this.IsValid(c, i)
						select c).ToList<ChallengeConfig>().RandomElement(false);
						list2.Add(item);
						list3.Remove(item);
					}
					else
					{
						ChallengeConfig item = list.FirstOrDefault((ChallengeConfig c) => c.id == this.gameStateService.Challenges.CurrentChallenges[i].id);
						list2.Add(item);
					}
				}
				if (index >= 0)
				{
					this.trackingService.TrackChallengeV2Unlocked(index);
				}
			}
			else
			{
				list2.Add(this.Definitions.Find((ChallengeConfig challenge) => challenge.predefined_order == 1));
				list2.Add(this.Definitions.Find((ChallengeConfig challenge) => challenge.predefined_order == 2));
				list2.Add(this.Definitions.Find((ChallengeConfig challenge) => challenge.predefined_order == 3));
			}
			List<ChallengeGoal> list4 = new List<ChallengeGoal>();
			switch (index)
			{
			case 0:
				list4.Add(this.CreateChallengeGoal(list2[0], list2[0].easy, ChallengeGoal.ChallengeDifficulty.easy));
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[1]);
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[2]);
				break;
			case 1:
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[0]);
				list4.Add(this.CreateChallengeGoal(list2[1], list2[1].medium, ChallengeGoal.ChallengeDifficulty.medium));
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[2]);
				break;
			case 2:
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[0]);
				list4.Add(this.gameStateService.Challenges.CurrentChallenges[1]);
				list4.Add(this.CreateChallengeGoal(list2[2], list2[2].hard, ChallengeGoal.ChallengeDifficulty.hard));
				break;
			default:
				list4.Add(this.CreateChallengeGoal(list2[0], list2[0].easy, ChallengeGoal.ChallengeDifficulty.easy));
				list4.Add(this.CreateChallengeGoal(list2[1], list2[1].medium, ChallengeGoal.ChallengeDifficulty.medium));
				list4.Add(this.CreateChallengeGoal(list2[2], list2[2].hard, ChallengeGoal.ChallengeDifficulty.hard));
				break;
			}
			this.gameStateService.Challenges.CurrentChallenges = list4;
			if (index < 0)
			{
				this.gameStateService.Challenges.ChallengeExpireTime = DateTime.Now.AddHours(24.0);
				this.gameStateService.Challenges.ChallengeStartTime = DateTime.Now;
				this.gameStateService.Challenges.CurrentAdBonus = 0;
				this.gameStateService.Challenges.NumberOfBoughtExtentions = 0;
			}
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000DA908 File Offset: 0x000D8D08
		private bool IsValid(ChallengeConfig config, int difficulty)
		{
			switch (difficulty)
			{
			case 0:
				return config.easy > -1;
			case 1:
				return config.medium > -1;
			case 2:
				return config.hard > -1;
			default:
				return true;
			}
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000DA940 File Offset: 0x000D8D40
		public bool IsChallengeCompleted(ChallengeGoal challenge)
		{
			return this.gameStateService.Resources.GetCollectedTotal(challenge.type) >= challenge.TargetAmount;
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000DA963 File Offset: 0x000D8D63
		public bool IsChallengeReadyToCollect(ChallengeGoal challenge)
		{
			return this.IsChallengeCompleted(challenge) && !challenge.collected;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000DA980 File Offset: 0x000D8D80
		private ChallengeGoal CreateChallengeGoal(ChallengeConfig challenge, int goal, ChallengeGoal.ChallengeDifficulty difficulty)
		{
			int collectedTotal = this.gameStateService.Resources.GetCollectedTotal(challenge.item);
			return new ChallengeGoal(challenge.id, challenge.item, collectedTotal, goal, false, DateTime.MinValue, difficulty);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000DA9BE File Offset: 0x000D8DBE
		public bool IsChallengeBundleAvailable()
		{
			return this.isAssetBundleAvailable;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000DA9C8 File Offset: 0x000D8DC8
		public void AssignNextDecoSet(bool force = false)
		{
			this.SelectNewDecoSet(force);
			ChallengeDataService challenges = this.gameStateService.Challenges;
			List<ChallengeBuildingProgress> list = new List<ChallengeBuildingProgress>();
			foreach (BuildingConfig buildingConfig in this.configService.buildingConfigList.buildings)
			{
				if (buildingConfig.challenge_set == challenges.CurrentDecoSet)
				{
					list.Add(new ChallengeBuildingProgress(buildingConfig.name, 0, 0, buildingConfig.blueprints));
				}
			}
			list.Sort((ChallengeBuildingProgress x, ChallengeBuildingProgress y) => y.targetAmount.CompareTo(x.targetAmount));
			challenges.BuildingProgress = list;
		}

		// 挑战奖励
		private void SelectNewDecoSet(bool force)
		{
			ChallengeDataService challenges = this.gameStateService.Challenges;
			int num = 5;
			int num2 = 5;
			foreach (BuildingConfig buildingConfig in this.configService.buildingConfigList.buildings)
			{
				if (buildingConfig.challenge_set >= num2)
				{
					num2 = buildingConfig.challenge_set;
				}
			}
			if (!force)
			{
				challenges.DecoSetExpireTime = DateTime.UtcNow.GetNextWeekDay(this.Balancing.reset_time_day).Date.AddMinutes((double)this.Balancing.reset_time_minutes);
				int num3 = (challenges.DecoSetExpireTime - Scripts1.DateTimeExtensions.FromUnixTimeStamp(0, DateTimeKind.Utc)).Days / 7;
				int num4 = num2 - num + 1;
				challenges.CurrentDecoSet = num + num3 % num4;
			}
			else
			{
				challenges.CurrentDecoSet++;
				challenges.DecoSetExpireTime = DateTime.Now.AddHours(168.0);
			}
			if (challenges.CurrentDecoSet > num2 || challenges.CurrentDecoSet < num)
			{
				challenges.CurrentDecoSet = num;
			}
		}

		// Token: 0x040057F8 RID: 22520
		public const string ASSET_BUNDLE_NAME = "buildings_challenges_2018";

		// Token: 0x040057F9 RID: 22521
		private const float AD_BONUS_PERCENTAGE = 0.1f;

		// Token: 0x040057FA RID: 22522
		private const int NUMBER_OF_CHALLENGES = 3;

		// Token: 0x040057FB RID: 22523
		private const int DECO_SET_EXPIRE_TIME_HOURS = 168;

		// Token: 0x040057FC RID: 22524
		private const int CHALLENGE_EXPIRE_TIME_HOURS = 24;

		// Token: 0x040057FD RID: 22525
		public const int FIRST_V2_DECO_SET = 5;

		// Token: 0x040057FE RID: 22526
		[WaitForService(true, true)]
		private GameStateService gameStateService;

		// Token: 0x040057FF RID: 22527
		[WaitForService(true, true)]
		private ConfigService configService;

		// Token: 0x04005800 RID: 22528
		[WaitForService(true, true)]
		private SBSService sbsService;

		// Token: 0x04005801 RID: 22529
		[WaitForService(true, true)]
		private ProgressionDataService.Service progressionDataService;

		// Token: 0x04005802 RID: 22530
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x04005803 RID: 22531
		[WaitForService(true, true)]
		private AssetBundleService assetBundleService;

		// Token: 0x04005804 RID: 22532
		private bool isAssetBundleAvailable;
	}
}
