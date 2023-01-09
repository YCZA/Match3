using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3.M3Editor;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C5 RID: 1733
	public class LevelLoader : ALevelLoader
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000C4C80 File Offset: 0x000C3080
		// (set) Token: 0x06002B25 RID: 11045 RVA: 0x000C4C88 File Offset: 0x000C3088
		public ScoringController ScoringController { get; private set; }

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x000C4C91 File Offset: 0x000C3091
		// (set) Token: 0x06002B27 RID: 11047 RVA: 0x000C4C99 File Offset: 0x000C3099
		public PTMatchEngine MatchEngine { get; private set; }

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x000C4CA2 File Offset: 0x000C30A2
		public override Fields Fields
		{
			get
			{
				return this.MatchEngine.Fields;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x000C4CAF File Offset: 0x000C30AF
		public BoardView BoardView
		{
			get
			{
				return this.boardView;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000C4CB7 File Offset: 0x000C30B7
		// (set) Token: 0x06002B2B RID: 11051 RVA: 0x000C4CBF File Offset: 0x000C30BF
		public LevelConfig Config { get; protected set; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x000C4CC8 File Offset: 0x000C30C8
		private M3_LevelRoot Root
		{
			get
			{
				M3_LevelRoot result;
				if ((result = this.levelRoot) == null)
				{
					result = (this.levelRoot = base.GetComponentInParent<M3_LevelRoot>());
				}
				return result;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x000C4CF1 File Offset: 0x000C30F1
		// (set) Token: 0x06002B2E RID: 11054 RVA: 0x000C4CF9 File Offset: 0x000C30F9
		public BoostFactory BoostFactory { get; private set; }

		// Token: 0x06002B2F RID: 11055 RVA: 0x000C4D04 File Offset: 0x000C3104
		public void Setup(LevelConfig config, bool isTest, M3_BannersRoot banners, LevelPlayMode levelPlayMode)
		{
			EAHelper.AddBreadcrumb(string.Concat(new object[]
			{
				"Loading Level: ",
				config.Level.level,
				" Tier: ",
				config.Level.tier
			}));
			this.Config = config;
			this.ModifyLevelColorDistribution();
			this.SetupMatchEngine(levelPlayMode);
			this.boardView.banners = banners;
			this.boardView.Initialize(config, this.Root.boosterUi.BoostOverlayController);
			this.boardView.onGemCollected.AddListener(new Action<Transform, GemColor, GemType>(this.Root.HandleGemCollected));
			this.boardView.onModifierCollected.AddListener(new Action<Transform, string>(this.Root.HandleModifierCollected));
			this.boardView.onHiddenItemFound.AddListener(new Action<HiddenItemView, float>(this.Root.HandleHiddenItemFound));
			this.boardView.onTournamentItemCollected.AddListener(new Action<TournamentItemCollectedView>(this.Root.HandleTournamentItemCollected));
			this.RegisterListeners(config);
			if (isTest && this.testState)
			{
				SerializableFields serializableFields = JsonUtility.FromJson<SerializableFields>(this.testState.text);
				this.LoadBoard(serializableFields.Deserialize());
			}
			else
			{
				this.MatchEngine.Start();
				this.boardView.CreateHiddenItems(HiddenItemProcessor.GetHiddenItems(config.layout), null);
				this.boardView.CreateColorWheels(false);
				this.boardView.InitializeChameleonViews(false);
			}
			this.boardView.GetComponent<BoardAnimationController>().Init(this.ScoringController);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000C4EAC File Offset: 0x000C32AC
		public override void LoadBoard(Fields fields)
		{
			this.MatchEngine.Reload(fields);
			HiddenItemInfoDict hiddenItems = this.MatchEngine.GetProcessor<HiddenItemProcessor>().HiddenItems;
			this.boardView.RemoveHiddenItems();
			this.boardView.CreateHiddenItems(HiddenItemProcessor.GetHiddenItems(this.Config.layout), hiddenItems);
			this.boardView.RemoveColorWheels();
			this.boardView.CreateColorWheels(false);
			this.boardView.InitializeChameleonViews(false);
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x000C4F20 File Offset: 0x000C3320
		private void RegisterListeners(LevelConfig config)
		{
			this.MatchEngine.onSetupCompleted.AddListener(new Action<Fields>(this.boardView.CreateViews));
			this.MatchEngine.onStepCompleted.AddListener(new Action<List<List<IMatchResult>>>(this.boardView.HandleStepCompleted));
			this.MatchEngine.onHighlightNextMove.AddListener(new Action<MatchCandidate>(this.boardView.HandleHighlightNextMove));
			InputDispatcher @object = new InputDispatcher(this.MatchEngine, this.BoostFactory, this.Root.gameStateService, this.Root.audioService, this.Root.boosterUi, this.boardView.onAnimationFinished, this.lastHurray.onStarted, config);
			this.boardView.onSwapped.AddListener(new Action<Move>(@object.HandleSwapped));
			this.boardView.onClicked.AddListener(new Action<IntVector2>(@object.HandleClicked));
			this.Root.boosterUi.onBoostSelected.AddListener(new Action<BoostViewData>(@object.HandleBoostSelected));
			this.Root.boosterUi.onBoostAdded.AddListener(new Action<BoostViewData>(@object.HandleBoostAdded));
			this.boardView.onAnimationFinished.AddListener(new Action(this.HandleAnimationFinished));
			this.boardView.onNeedPossibleMoves.AddListener(new Action(this.MatchEngine.HandleNeedPossibleMoves));
			BoardAnimationController component = this.boardView.GetComponent<BoardAnimationController>();
			component.onStepFinished.AddListener(new Action(this.ScoringController.HandleStepFinished));
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000C50B8 File Offset: 0x000C34B8
		private void HandleAnimationFinished()
		{
			base.StartCoroutine(this.AnimationFinishedRoutine());
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000C50C8 File Offset: 0x000C34C8
		private IEnumerator AnimationFinishedRoutine()
		{
			if (this.ScoringController.OutOfMoves && !this.ScoringController.IsObjectiveReached() && !this.Config.IsEditMode)
			{
				yield return new WaitUntil(() => !this.Root.boosterUi.options.gameObject.activeSelf);
				yield return new BuyMoreMovesFlow(this.ScoringController, this.Config, this.Root.boosterUi, new TrackingService.PurchaseFlowContext
				{
					det1 = "post_moves",
					det2 = this.Config.Name,
					det3 = this.Config.LevelCollectionConfig.level.ToString()
				}, new BuyMoreMovesFlow.TrackingContext
				{
					source1 = "out_of_moves",
					source2 = "post_moves"
				}).Start();
			}
			this.Root.AnimateCharacter(this.MatchEngine.SumStepCascades);
			this.Root.PlayCongratulationsAnimation(this.MatchEngine.SumStepCascades);
			this.ScoringController.UpdateScoreStatusAndDispatchGameOverIfNeeded();
			this.MatchEngine.IsResolvingMoves = false;
			yield return null;
			yield break;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000C50E4 File Offset: 0x000C34E4
		private void ModifyLevelColorDistribution()
		{
			// TournamentType apparentOngoingTournamentType = this.Root.tournamentService.GetApparentOngoingTournamentType();
			TournamentType apparentOngoingTournamentType = TournamentType.Undefined;
			if (!TournamentConfig.IsFruitTournament(apparentOngoingTournamentType))
			{
				return;
			}
			GemColor tournamentColor = TournamentConfig.TournamentTypeToGemColor(apparentOngoingTournamentType);
			GemColor exchangeColor = this.GetExchangeColor(tournamentColor);
			if (exchangeColor == GemColor.Undefined)
			{
				return;
			}
			if (this.HasTournamentColorInLayout(tournamentColor))
			{
				return;
			}
			MaterialAmount[] gems = this.Config.data.gems;
			this.ChangeColorsInMaterials(gems, exchangeColor, tournamentColor);
			MaterialAmount[] objectives = this.Config.data.objectives;
			this.ChangeColorsInMaterials(objectives, exchangeColor, tournamentColor);
			this.ChangeColorsInSpawnRatios(exchangeColor, tournamentColor);
			this.ChangeColorsInLayout(exchangeColor, tournamentColor);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000C5178 File Offset: 0x000C3578
		private GemColor GetExchangeColor(GemColor tournamentColor)
		{
			bool flag = this.Config.data.ChameleonCount > 0;
			bool flag2 = false;
			GemColor result = GemColor.Red;
			List<GemColor> colorsFromAmounts = LevelGenerator.GetColorsFromAmounts(this.Config.data.gems);
			foreach (GemColor gemColor in colorsFromAmounts)
			{
				if (tournamentColor == gemColor)
				{
					result = GemColor.Undefined;
					break;
				}
				if (flag && !flag2 && gemColor != GemColor.Red && gemColor != GemColor.Coins)
				{
					result = gemColor;
					flag2 = true;
				}
			}
			return result;
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000C522C File Offset: 0x000C362C
		private void ChangeColorsInMaterials(MaterialAmount[] materials, GemColor exchangeColor, GemColor tournamentColor)
		{
			int i = 0;
			while (i < materials.Length)
			{
				GemColor gemColor = GemColor.Undefined;
				try
				{
					gemColor = (GemColor)Enum.Parse(typeof(GemColor), materials[i].type, true);
				}
				catch (Exception ex)
				{
					goto IL_5B;
				}
				goto IL_36;
				IL_5B:
				i++;
				continue;
				IL_36:
				if (gemColor == exchangeColor)
				{
					materials[i].type = tournamentColor.ToString().ToLower();
					goto IL_5B;
				}
				goto IL_5B;
			}
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000C52B4 File Offset: 0x000C36B4
		private void ChangeColorsInSpawnRatios(GemColor exchangeColor, GemColor tournamentColor)
		{
			SpawnRatio[] spawnRatios = this.Config.data.spawnRatios;
			foreach (SpawnRatio spawnRatio in spawnRatios)
			{
				GemColor gemColor = spawnRatio.gemColor;
				if (gemColor == exchangeColor)
				{
					spawnRatio.gemColor = tournamentColor;
				}
			}
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000C5304 File Offset: 0x000C3704
		private void ChangeColorsInLayout(GemColor exchangeColor, GemColor tournamentColor)
		{
			int num = this.Config.layout.fields.Length;
			bool flag = !this.Config.layout.crates.IsNullOrEmptyCollection();
			for (int i = 0; i < num; i++)
			{
				if (this.Config.layout.gemColors[i] == (int)exchangeColor)
				{
					this.Config.layout.gemColors[i] = (int)tournamentColor;
				}
				if (flag && this.Config.layout.crates[i] > 0)
				{
					int indexCrate = this.Config.layout.crates[i];
					if (Crate.GetColor(indexCrate) == exchangeColor)
					{
						int hp = Crate.GetHp(indexCrate);
						this.Config.layout.crates[i] = Crate.GetIndex(tournamentColor, hp);
					}
				}
			}
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000C53DC File Offset: 0x000C37DC
		private bool HasTournamentColorInLayout(GemColor tournamentColor)
		{
			int num = this.Config.layout.fields.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.Config.layout.gemColors[i] == (int)tournamentColor)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000C542C File Offset: 0x000C382C
		private void SetupMatchEngine(LevelPlayMode levelPlayMode = LevelPlayMode.Regular)
		{
			PTMoveProcessor moveProcessor = new PTMoveProcessor();
			APTBoardFactory aptboardFactory = this.CreateBoardFactory();
			aptboardFactory.Setup(this.Config);
			this.MatchEngine = new PTMatchEngine(aptboardFactory, moveProcessor);
			// this.ScoringController = new ScoringController(this.MatchEngine, this.Config, this.Root.tournamentService.GetApparentOngoingTournamentType(), levelPlayMode);
			this.ScoringController = new ScoringController(this.MatchEngine, this.Config, TournamentType.Undefined, levelPlayMode);
			this.MatchEngine.onBoostUsed.AddListener(new Action<Boosts>(this.ScoringController.HandleBoostUsed));
			LevelLoader.SetupHiddenItems(this.Fields, this.Config.layout);
			LevelLoader.SetupColorWheels(this.Fields, this.Config);
			LevelLoader.SetupChameleons(aptboardFactory.Fields, this.Config);
			MatchEngineHelper.AddProcessors(this.MatchEngine, aptboardFactory, this.Config, this.ScoringController);
			this.BoostFactory = new BoostFactory(this.MatchEngine.Fields, this.Config.layout.HasWaterAndUnwateredFields);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000C5520 File Offset: 0x000C3920
		public static void SetupHiddenItems(Fields fields, LevelLayout layout)
		{
			HiddenItemRandomizer hiddenItemRandomizer = new HiddenItemRandomizer();
			HiddenItemInfoDict hiddenItemInfoDict = hiddenItemRandomizer.Randomize(layout);
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					IntVector2 vec = new IntVector2(i, j);
					fields[vec].hiddenItemId = 0;
				}
			}
			foreach (KeyValuePair<int, HiddenItemInfo> keyValuePair in hiddenItemInfoDict)
			{
				foreach (IntVector2 vec2 in keyValuePair.Value.positions)
				{
					fields[vec2].hiddenItemId = keyValuePair.Value.id;
				}
			}
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000C562C File Offset: 0x000C3A2C
		public static void SetupColorWheels(Fields fields, LevelConfig levelConfig)
		{
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.IsColorWheelCorner)
					{
						fields.AddColorWheel(field.gridPosition, levelConfig, field.IsColorWheelVariant);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000C56A4 File Offset: 0x000C3AA4
		public static void SetupChameleons(Fields fields, LevelConfig levelConfig)
		{
			fields.SetupChameleonModels(levelConfig);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000C56AD File Offset: 0x000C3AAD
		public override APTBoardFactory CreateBoardFactory()
		{
			return new PTBoardFactory();
		}

		// Token: 0x0400544F RID: 21583
		private M3_LevelRoot levelRoot;
	}
}
