using System;
using System.Collections;
using System.Linq;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x020007E2 RID: 2018
	public class ProgressionDataService : ADataService
	{
		// Token: 0x060031D6 RID: 12758 RVA: 0x000EB601 File Offset: 0x000E9A01
		public ProgressionDataService(Func<GameState> getState) : base(getState)
		{
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000EB60A File Offset: 0x000E9A0A
		// (set) Token: 0x060031D8 RID: 12760 RVA: 0x000EB623 File Offset: 0x000E9A23
		public int UnlockedLevel
		{
			get
			{
				return base.state.progression.tiers.Count + 1;
			}
			protected set
			{
				this.UnlockLevel(value, 0);
			}
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000EB630 File Offset: 0x000E9A30
		protected void UnlockLevel(int level, int tierToComplete)
		{
			int unlockedLevel = this.UnlockedLevel;
			if (level > unlockedLevel)
			{
				int num = level - this.UnlockedLevel;
				for (int i = 0; i < num; i++)
				{
					base.state.progression.tiers.Add(tierToComplete);
				}
			}
			else if (level < this.UnlockedLevel)
			{
				base.state.progression.tiers = base.state.progression.tiers.Take(level - 1).ToList<int>();
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x000EB6BA File Offset: 0x000E9ABA
		// (set) Token: 0x060031DB RID: 12763 RVA: 0x000EB6CC File Offset: 0x000E9ACC
		public string CurrentTutorial
		{
			get
			{
				return base.state.progression.currentTutorial;
			}
			set
			{
				base.state.progression.currentTutorial = value;
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000EB6DF File Offset: 0x000E9ADF
		protected int GetTier(int level)
		{
			return (level > base.state.progression.tiers.Count) ? -1 : base.state.progression.tiers[level - 1];
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000EB71A File Offset: 0x000E9B1A
		protected void CompleteTier(int level, int tier)
		{
			base.state.progression.tiers[level - 1] = tier;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000EB738 File Offset: 0x000E9B38
		protected int NumberOfCompletedLevelsAtTier(int tier)
		{
			return base.state.progression.tiers.Count((int t) => t >= tier);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000EB773 File Offset: 0x000E9B73
		protected bool IsLocked(int level)
		{
			return level > this.UnlockedLevel;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000EB77E File Offset: 0x000E9B7E
		protected bool IsCompleted(int level)
		{
			return this.GetTier(level) >= 2;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000EB78D File Offset: 0x000E9B8D
		public bool IsTutorialCompleted(string name)
		{
			return base.state.progression.completedTutorials.Contains(name);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000EB7A5 File Offset: 0x000E9BA5
		public void CompleteTutorial(string name)
		{
			if (!this.IsTutorialCompleted(name))
			{
				base.state.progression.completedTutorials.Add(name);
			}
		}

		public int GetNumberOfTutorialCompleted()
		{
			return base.state.progression.completedTutorials.Count;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000EB7C9 File Offset: 0x000E9BC9
		public void ResetTutorial(string name)
		{
			base.state.progression.completedTutorials.Remove(name);
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x000EB7F5 File Offset: 0x000E9BF5
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x000EB7E2 File Offset: 0x000E9BE2
		public int CleanupIndex
		{
			get
			{
				return base.state.progression.cleanupIndex;
			}
			set
			{
				base.state.progression.cleanupIndex = value;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060031E7 RID: 12775 RVA: 0x000EB81A File Offset: 0x000E9C1A
		// (set) Token: 0x060031E6 RID: 12774 RVA: 0x000EB807 File Offset: 0x000E9C07
		public int LastUnlockedArea
		{
			get
			{
				return base.state.progression.lastUnlockedArea;
			}
			set
			{
				base.state.progression.lastUnlockedArea = value;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x000EB82C File Offset: 0x000E9C2C
		// (set) Token: 0x060031E9 RID: 12777 RVA: 0x000EB83E File Offset: 0x000E9C3E
		public bool HasOpenedTropicam
		{
			get
			{
				return base.state.progression.hasOpenedTropicam;
			}
			set
			{
				base.state.progression.hasOpenedTropicam = value;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x000EB851 File Offset: 0x000E9C51
		// (set) Token: 0x060031EB RID: 12779 RVA: 0x000EB863 File Offset: 0x000E9C63
		public int EndOfContentReachedAtArea
		{
			get
			{
				return base.state.progression.endOfContentReachedAtArea;
			}
			set
			{
				base.state.progression.endOfContentReachedAtArea = value;
			}
		}

		// Token: 0x020007E3 RID: 2019
		public class Service : AService
		{
			// Token: 0x060031EC RID: 12780 RVA: 0x000EB878 File Offset: 0x000E9C78
			public Service()
			{
				WooroutineRunner.StartCoroutine(this.InitRoutine(), null);
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x060031ED RID: 12781 RVA: 0x000EB8DA File Offset: 0x000E9CDA
			public ProgressionDataService Data
			{
				get
				{
					return this.data;
				}
			}

			// Token: 0x060031EE RID: 12782 RVA: 0x000EB8E4 File Offset: 0x000E9CE4
			public void CheatToNextArea()
			{
				int areaForLevel = this.m3config.GetAreaForLevel(this.UnlockedLevel);
				int num = areaForLevel + 1;
				int last_area = this.configService.general.tier_unlocked.last_area;
				if (num > last_area)
				{
					WoogaDebug.Log(new object[]
					{
						"We are already in last available area"
					});
					return;
				}
				this.UnlockedLevel = this.m3config.GetFirstLevelOfArea(num);
				this.onAreaChanged.Dispatch(num);
			}

			// Token: 0x170007D6 RID: 2006
			// (get) Token: 0x060031EF RID: 12783 RVA: 0x000EB956 File Offset: 0x000E9D56
			// (set) Token: 0x060031F0 RID: 12784 RVA: 0x000EB963 File Offset: 0x000E9D63
			public string CurrentTutorial
			{
				get
				{
					return this.data.CurrentTutorial;
				}
				set
				{
					this.data.CurrentTutorial = value;
				}
			}

			// Token: 0x170007D7 RID: 2007
			// (get) Token: 0x060031F1 RID: 12785 RVA: 0x000EB971 File Offset: 0x000E9D71
			// (set) Token: 0x060031F2 RID: 12786 RVA: 0x000EB979 File Offset: 0x000E9D79
			public int CurrentLevel { get; set; }

			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x060031F3 RID: 12787 RVA: 0x000EB982 File Offset: 0x000E9D82
			// (set) Token: 0x060031F4 RID: 12788 RVA: 0x000EB98F File Offset: 0x000E9D8F
			public int UnlockedLevel
			{
				get
				{
					return this.data.UnlockedLevel;
				}
				set
				{
					this.UnlockLevel(value, 0);
				}
			}

			// Token: 0x060031F5 RID: 12789 RVA: 0x000EB999 File Offset: 0x000E9D99
			public int CheatUnlockNextLevel()
			{
				this.UnlockLevel(this.UnlockedLevel + 1, 2);
				return this.UnlockedLevel;
			}

			// Token: 0x060031F6 RID: 12790 RVA: 0x000EB9B0 File Offset: 0x000E9DB0
			protected void UnlockLevel(int value, int tierToComplete = 0)
			{
				int unlockedLevel = this.UnlockedLevel;
				this.data.UnlockLevel(value, tierToComplete);
				if (this.UnlockedLevel > unlockedLevel)
				{
					int areaForLevel = this.m3config.GetAreaForLevel(unlockedLevel);
					int areaForLevel2 = this.m3config.GetAreaForLevel(this.UnlockedLevel);
					if (areaForLevel2 > areaForLevel)
					{
						this.onAreaUnlocked.Dispatch(areaForLevel2);
						this.onAreaChanged.Dispatch(areaForLevel2);
					}
					this.onLevelUnlocked.Dispatch(this.UnlockedLevel);
				}
				this.CurrentLevel = this.UnlockedLevel;
			}

			// Token: 0x060031F7 RID: 12791 RVA: 0x000EBA38 File Offset: 0x000E9E38
			public int NumberOfCompletedLevelsAtTier(int tier)
			{
				return this.data.NumberOfCompletedLevelsAtTier(tier);
			}

			// Token: 0x060031F8 RID: 12792 RVA: 0x000EBA48 File Offset: 0x000E9E48
			public void CompleteTier(int level, int tier)
			{
				bool flag = level >= this.UnlockedLevel;
				bool flag2 = flag || tier > this.data.GetTier(level);
				if (flag)
				{
					this.UnlockedLevel++;
				}
				if (flag2)
				{
					this.data.CompleteTier(level, tier);
					this.onTierCompleted.Dispatch(new Level(level, tier));
				}
				if (flag)
				{
					this.CurrentLevel = this.UnlockedLevel;
					this.onLevelUnlocked.Dispatch(this.UnlockedLevel);
				}
			}

			// Token: 0x060031F9 RID: 12793 RVA: 0x000EBAD7 File Offset: 0x000E9ED7
			public bool IsLocked(int level)
			{
				return this.data.IsLocked(level);
			}

			// Token: 0x060031FA RID: 12794 RVA: 0x000EBAE5 File Offset: 0x000E9EE5
			public bool IsCompleted(int level)
			{
				return this.data.IsCompleted(level);
			}

			// Token: 0x060031FB RID: 12795 RVA: 0x000EBAF3 File Offset: 0x000E9EF3
			public int GetTier(int level)
			{
				return this.data.GetTier(level);
			}

			// Token: 0x060031FC RID: 12796 RVA: 0x000EBB01 File Offset: 0x000E9F01
			public bool IsTutorialCompleted(string name)
			{
				return this.data.IsTutorialCompleted(name);
			}

			// Token: 0x060031FD RID: 12797 RVA: 0x000EBB0F File Offset: 0x000E9F0F
			public void CompleteTutorial(string name)
			{
				this.data.CompleteTutorial(name);
				this.CurrentTutorial = null;
			}

			public int GetNumberOfTutorialCompleted()
			{
				return data.GetNumberOfTutorialCompleted();
			}

			// Token: 0x060031FE RID: 12798 RVA: 0x000EBB24 File Offset: 0x000E9F24
			public void ResetTutorial(string name)
			{
				this.data.ResetTutorial(name);
			}

			// Token: 0x060031FF RID: 12799 RVA: 0x000EBB34 File Offset: 0x000E9F34
			public bool HasCompletedAllTiers()
			{
				int numLevels = this.m3config.NumLevels;
				for (int i = 1; i <= numLevels; i++)
				{
					if (this.GetTier(i) < 2)
					{
						return false;
					}
				}
				return true;
			}

			// eli key point清理果皮
			public int LastRubbleAreaCleared
			{
				get
				{
					return this.data.CleanupIndex;
				}
				set
				{
					// Debug.LogError("清除果皮");
					if (this.data.CleanupIndex != value)
					{
						this.onCleanupChanged.Dispatch(value);
						this.data.CleanupIndex = value;
					}
				}
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x06003202 RID: 12802 RVA: 0x000EBBA7 File Offset: 0x000E9FA7
			// (set) Token: 0x06003203 RID: 12803 RVA: 0x000EBBB4 File Offset: 0x000E9FB4
			public int LastUnlockedArea
			{
				get
				{
					return this.data.LastUnlockedArea;
				}
				set
				{
					if (this.data.LastUnlockedArea != value)
					{
						this.onAreaUnlocked.Dispatch(value);
						this.data.LastUnlockedArea = value;
					}
				}
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x06003204 RID: 12804 RVA: 0x000EBBDF File Offset: 0x000E9FDF
			public int LastAreaIgnoringQuestAndEocLock
			{
				get
				{
					return this.m3config.GetAreaForLevel(this.UnlockedLevel);
				}
			}

			// Token: 0x06003205 RID: 12805 RVA: 0x000EBBF4 File Offset: 0x000E9FF4
			private IEnumerator InitRoutine()
			{
				yield return ServiceLocator.Instance.Inject(this);
				this.data = this.gameStateService.Progression;
				this.CurrentLevel = this.UnlockedLevel;
				base.OnInitialized.Dispatch();
				yield break;
			}

			// Token: 0x04005A67 RID: 23143
			public readonly Signal<Level> onTierCompleted = new Signal<Level>();

			// Token: 0x04005A68 RID: 23144
			public readonly Signal<int> onAreaChanged = new Signal<int>();

			// Token: 0x04005A69 RID: 23145
			public readonly Signal<int> onAreaUnlocked = new Signal<int>();

			// Token: 0x04005A6A RID: 23146
			public readonly Signal<int> onCleanupChanged = new Signal<int>();

			// Token: 0x04005A6B RID: 23147
			public readonly Signal<int> onLevelUnlocked = new Signal<int>();

			// Token: 0x04005A6C RID: 23148
			public readonly Signal onGameStarted = new Signal();

			// Token: 0x04005A6D RID: 23149
			[WaitForService(true, true)]
			private GameStateService gameStateService;

			// Token: 0x04005A6E RID: 23150
			[WaitForService(true, true)]
			private M3ConfigService m3config;

			// Token: 0x04005A6F RID: 23151
			[WaitForService(true, true)]
			private ConfigService configService;

			// Token: 0x04005A70 RID: 23152
			private ProgressionDataService data;
		}
	}
}
