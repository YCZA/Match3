using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Shared.M3Engine;
using Wooga.Coroutines;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000558 RID: 1368
	public class CascadeBalancer : MonoBehaviour
	{
		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x0009F235 File Offset: 0x0009D635
		public float MeanMoves
		{
			get
			{
				return this.meanMoves;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x0009F23D File Offset: 0x0009D63D
		public float MeanCascadesPerMove
		{
			get
			{
				return this.meanCascadesPerMove;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x0009F245 File Offset: 0x0009D645
		public float MeanReshufflesPerMove
		{
			get
			{
				return this.meanReshufflesPerMove;
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0009F24D File Offset: 0x0009D64D
		public void RunLevel(string levelName, LevelConfig config, int times, int movesLimit)
		{
			this.levelName = levelName;
			this.levelConfig = config;
			this.SetUpMovesLimit(movesLimit);
			WooroutineRunner.StartCoroutine(this.RunLevelRoutineTimes(times), null);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0009F274 File Offset: 0x0009D674
		public IEnumerator RunLevelRoutine(string levelName, LevelConfig config, int times, int movesLimit)
		{
			this.levelName = levelName;
			this.levelConfig = config;
			this.SetUpMovesLimit(movesLimit);
			yield return this.RunLevelRoutineTimes(times);
			yield break;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0009F2AC File Offset: 0x0009D6AC
		public void BalanceLevel(LevelConfig config, string filePath, int times, int movesLimit, int stepSize)
		{
			this.levelConfig = config;
			this.SetUpMovesLimit(movesLimit);
			WooroutineRunner.StartCoroutine(this.BalanceLevelRoutine(filePath, times, stepSize), null);
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0009F2D0 File Offset: 0x0009D6D0
		private IEnumerator RunLevelRoutineTimes(int times)
		{
			this.sumMoves = 0;
			this.meanMoves = 0f;
			this.sumCascadesPerMoves = 0f;
			this.sumReshufflesPerMove = 0f;
			this.meanCascadesPerMove = 0f;
			this.meanReshufflesPerMove = 0f;
			for (int i = 0; i < times; i++)
			{
				yield return this.RunLevelRoutine();
			}
			this.meanMoves = (float)this.sumMoves / (float)times;
			this.meanCascadesPerMove = this.sumCascadesPerMoves / (float)times;
			this.meanReshufflesPerMove = this.sumReshufflesPerMove / (float)times;
			WoogaDebug.LogWarningFormatted("Level {0} Finished {1} times. Cascades/Moves: {2}, Reshuffles/Moves: {3}, Mean Moves Needed: {4}", new object[]
			{
				this.levelName,
				times,
				this.meanCascadesPerMove,
				this.meanReshufflesPerMove,
				this.meanMoves
			});
			this.LogColorDistribution();
			yield break;
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0009F2F4 File Offset: 0x0009D6F4
		private IEnumerator BalanceLevelRoutine(string filePath, int times, int stepSize)
		{
			do
			{
				yield return this.RunLevelRoutineTimes(times);
				this.AutoAdjustColors(stepSize);
			}
			while (this.canBalance && (this.meanCascadesPerMove < 1.2f || this.meanCascadesPerMove > 1.4f));
			if (this.canBalance)
			{
				WoogaDebug.Log(new object[]
				{
					"Finished balancing"
				});
				this.LogColorDistribution();
				FieldSerializer.SaveToDisk(filePath, this.levelConfig);
			}
			yield break;
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0009F324 File Offset: 0x0009D724
		private void LogColorDistribution()
		{
			string text = string.Empty;
			foreach (MaterialAmount materialAmount in this.levelConfig.data.gems)
			{
				text = text + materialAmount.amount + "/";
			}
			WoogaDebug.Log(new object[]
			{
				text
			});
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0009F390 File Offset: 0x0009D790
		private void AutoAdjustColors(int stepSize)
		{
			if (this.meanCascadesPerMove < 1.2f)
			{
				this.IncreaseCascades(stepSize);
			}
			else if (this.meanCascadesPerMove > 1.4f)
			{
				this.DecreaseCascades(stepSize);
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0009F3C8 File Offset: 0x0009D7C8
		private void IncreaseCascades(int stepSize)
		{
			WoogaDebug.Log(new object[]
			{
				"Increase Cascades",
				this.meanCascadesPerMove,
				1.2f,
				1.4f
			});
			int num = this.levelConfig.data.gems.Length - 1;
			int amount = this.levelConfig.data.gems[num].amount;
			int num2 = amount - stepSize;
			if (num2 > 0)
			{
				this.levelConfig.data.gems[num].amount = num2;
			}
			else if (this.levelConfig.data.gems.Length > 4)
			{
				int num3 = num;
				MaterialAmount[] array = new MaterialAmount[num3];
				for (int i = 0; i < num3; i++)
				{
					array[i] = this.levelConfig.data.gems[i];
				}
				this.levelConfig.data.gems = array;
			}
			else
			{
				WoogaDebug.LogError(new object[]
				{
					"Can't reduce colors anymore - discard auto balancing"
				});
				this.canBalance = false;
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0009F500 File Offset: 0x0009D900
		private void DecreaseCascades(int stepSize)
		{
			WoogaDebug.Log(new object[]
			{
				"Decrease Cascades",
				this.meanCascadesPerMove,
				1.2f,
				1.4f
			});
			int num = this.levelConfig.data.gems.Length - 1;
			int amount = this.levelConfig.data.gems[num].amount;
			int num2 = amount + stepSize;
			if (num2 <= 25)
			{
				this.levelConfig.data.gems[num].amount = num2;
			}
			else
			{
				string availableColor = this.GetAvailableColor();
				if (this.levelConfig.data.gems.Length < CascadeBalancer.allColors.Count && availableColor != null)
				{
					this.LogColorDistribution();
					MaterialAmount[] array = new MaterialAmount[num + 2];
					for (int i = 0; i <= num; i++)
					{
						array[i] = this.levelConfig.data.gems[i];
					}
					array[num + 1] = new MaterialAmount(availableColor, stepSize, MaterialAmountUsage.Undefined, 0);
					this.levelConfig.data.gems = array;
					this.LogColorDistribution();
				}
				else
				{
					WoogaDebug.LogError(new object[]
					{
						"Can't add anymore colors - discard auto balancing"
					});
					this.canBalance = false;
				}
			}
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0009F674 File Offset: 0x0009DA74
		private string GetAvailableColor()
		{
			List<string> list = CascadeBalancer.allColors.Clone<string>();
			foreach (MaterialAmount materialAmount in this.levelConfig.data.gems)
			{
				if (list.Contains(materialAmount.type))
				{
					list.Remove(materialAmount.type);
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return list.RandomElement(false);
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0009F6F4 File Offset: 0x0009DAF4
		private IEnumerator RunLevelRoutine()
		{
			this.movesCounter = 0;
			this.SetupMatchEngine();
			this.startNextAutoPlayMove = true;
			AwaitSignal<Match3Score> score = null;
			do
			{
				score = this.scoringController.onGameOver.Await<Match3Score>();
				yield return score;
			}
			while (!score.Dispatched.success);
			this.startNextAutoPlayMove = false;
			int movesNeeded = score.Dispatched.Config.data.moves - score.Dispatched.movesLeft;
			int cascades = this.matchEngine.sumLevelCascades;
			float cascadesPerMoves = (float)cascades / (float)movesNeeded;
			int reshuffles = score.Dispatched.reshuffles;
			float reshufflesPerMoves = (float)reshuffles / (float)movesNeeded;
			WoogaDebug.LogFormatted("Level {0} Finished. Cascades/Moves: {1}, Reshuffles/Moves: {2}, Moves Needed: {3}", new object[]
			{
				this.levelName,
				cascadesPerMoves,
				reshufflesPerMoves,
				movesNeeded
			});
			this.sumMoves += movesNeeded;
			this.sumCascadesPerMoves += cascadesPerMoves;
			this.sumReshufflesPerMove += reshufflesPerMoves;
			yield break;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0009F710 File Offset: 0x0009DB10
		private void SetupMatchEngine()
		{
			this.boardFactory = new PTBoardFactory();
			this.moveProcessor = new PTMoveProcessor();
			this.levelConfig.UpdateObjectives();
			this.boardFactory.Setup(this.levelConfig);
			this.matchEngine = new PTMatchEngine(this.boardFactory, this.moveProcessor);
			this.scoringController = new CascadeBalancingScoringController(this.levelConfig, this.matchEngine);
			LevelLoader.SetupHiddenItems(this.boardFactory.Fields, this.levelConfig.layout);
			LevelLoader.SetupColorWheels(this.boardFactory.Fields, this.levelConfig);
			LevelLoader.SetupChameleons(this.boardFactory.Fields, this.levelConfig);
			MatchEngineHelper.AddProcessors(this.matchEngine, this.boardFactory, this.levelConfig, this.scoringController);
			this.matchEngine.onStepCompleted.AddListener(new Action<List<List<IMatchResult>>>(this.HandleStepCompleted));
			this.matchEngine.Start();
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x0009F808 File Offset: 0x0009DC08
		private void SetUpMovesLimit(int movesLimit)
		{
			this.hasMovesLimit = (movesLimit > 0);
			this.movesLimit = movesLimit;
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0009F81B File Offset: 0x0009DC1B
		private void HandleStepCompleted(List<List<IMatchResult>> results)
		{
			this.scoringController.UpdateScoreStatusAndDispatchGameOverIfNeeded();
			this.matchEngine.IsResolvingMoves = false;
			this.startNextAutoPlayMove = true;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0009F83C File Offset: 0x0009DC3C
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				Match3Score match3Score = new Match3Score(this.levelConfig, LevelPlayMode.Regular);
				match3Score.success = false;
				this.startNextAutoPlayMove = false;
				this.scoringController.onGameOver.Dispatch(match3Score);
			}
			if (global::UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				WoogaDebug.Log(new object[]
				{
					"Stopped Balancing"
				});
				this.canBalance = false;
			}
			if (this.startNextAutoPlayMove)
			{
				this.startNextAutoPlayMove = false;
				if (!this.matchEngine.IsResolvingMoves)
				{
					if (this.hasMovesLimit && this.movesCounter >= this.movesLimit)
					{
						WoogaDebug.LogWarning(new object[]
						{
							"Reached moves limit: ",
							this.movesCounter,
							this.movesLimit
						});
						this.scoringController.ForceGameOver();
						return;
					}
					Move nextBestMove = this.matchEngine.GetNextBestMove();
					this.matchEngine.ProcessMove(nextBestMove);
					this.movesCounter++;
				}
			}
		}

		// Token: 0x04004F66 RID: 20326
		private const float MIN_CASCADES = 1.2f;

		// Token: 0x04004F67 RID: 20327
		private const float MAX_CASCADES = 1.4f;

		// Token: 0x04004F68 RID: 20328
		private const string SINGLE_RUN_STATS = "Level {0} Finished. Cascades/Moves: {1}, Reshuffles/Moves: {2}, Moves Needed: {3}";

		// Token: 0x04004F69 RID: 20329
		private const string COMBINED_STATS = "Level {0} Finished {1} times. Cascades/Moves: {2}, Reshuffles/Moves: {3}, Mean Moves Needed: {4}";

		// Token: 0x04004F6A RID: 20330
		private static readonly List<string> allColors = new List<string>
		{
			GemColor.Blue.ToString().ToLower(),
			GemColor.Green.ToString().ToLower(),
			GemColor.Orange.ToString().ToLower(),
			GemColor.Purple.ToString().ToLower(),
			GemColor.Red.ToString().ToLower(),
			GemColor.Yellow.ToString().ToLower()
		};

		// Token: 0x04004F6B RID: 20331
		private LevelConfig levelConfig;

		// Token: 0x04004F6C RID: 20332
		private string levelName;

		// Token: 0x04004F6D RID: 20333
		private PTBoardFactory boardFactory;

		// Token: 0x04004F6E RID: 20334
		private PTMoveProcessor moveProcessor;

		// Token: 0x04004F6F RID: 20335
		private CascadeBalancingScoringController scoringController;

		// Token: 0x04004F70 RID: 20336
		private PTMatchEngine matchEngine;

		// Token: 0x04004F71 RID: 20337
		private int movesCounter;

		// Token: 0x04004F72 RID: 20338
		private int movesLimit;

		// Token: 0x04004F73 RID: 20339
		private bool hasMovesLimit;

		// Token: 0x04004F74 RID: 20340
		private bool canBalance = true;

		// Token: 0x04004F75 RID: 20341
		private int sumMoves;

		// Token: 0x04004F76 RID: 20342
		private float meanMoves;

		// Token: 0x04004F77 RID: 20343
		private float meanReshufflesPerMove;

		// Token: 0x04004F78 RID: 20344
		private float sumCascadesPerMoves;

		// Token: 0x04004F79 RID: 20345
		private float sumReshufflesPerMove;

		// Token: 0x04004F7A RID: 20346
		private float meanCascadesPerMove;

		// Token: 0x04004F7B RID: 20347
		private bool startNextAutoPlayMove;
	}
}
