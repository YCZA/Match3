using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Match3.Scoring
{
	// Token: 0x0200063B RID: 1595
	public class ScoringController : IScoringController, ITrackableScoring
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x000B529C File Offset: 0x000B369C
		public ScoringController(PTMatchEngine matchEngine, LevelConfig config, TournamentType currentTournament, LevelPlayMode levelPlayMode)
		{
			this.currentOngoingTournament = currentTournament;
			this.score = new Match3Score(config, levelPlayMode);
			this.objectives = new Materials(config.data.objectives);
			this.processors.Add(new GroupProcessor(this.score, this.currentOngoingTournament));
			this.processors.Add(new FieldModifierProcessor(this.score));
			this.processors.Add(new ScoreCollectedProcessor(this.score));
			this.processors.Add(new HiddenItemScoreProcessor(this.score));
			this.processors.Add(new TrackingBombProcessor(this.score));
			this.processors.Add(new TournamentScoreProcessor(this.score));
			this.processors.Add(new SuperRainbowProcessor(this.score));
			this.processors.Add(new LinegemBombProcessor(this.score));
			this.processors.Add(new StarProcessor(this.score));
			this.processors.Add(new RainbowSuperGemProcessor(this.score));
			this.processors.Add(new SuperBombProcessor(this.score));
			matchEngine.onStepCompleted.AddListener(new Action<List<List<IMatchResult>>>(this.HandleStepCompleted));
			matchEngine.onOutOfMoves.AddListener(new Action(this.HandleOutOfPossibleMoves));
			matchEngine.onShuffled.AddListener(delegate
			{
				this.score.reshuffles++;
			});
			this.matchEngine = matchEngine;
			this.isEditMode = config.IsEditMode;
			this.SetupTournamentPointsContainer(config.tournamentMultiplier);
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x000B5470 File Offset: 0x000B3870
		public Materials ObjectivesLeft
		{
			get
			{
				this._objectivesLeft.Clear();
				foreach (MaterialAmount materialAmount in this.objectives)
				{
					this._objectivesLeft.Add(new MaterialAmount(materialAmount.type, materialAmount.amount - this.score.amounts[materialAmount.type], MaterialAmountUsage.Undefined, 0));
				}
				return this._objectivesLeft;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000B550C File Offset: 0x000B390C
		public int MovesLeft
		{
			get
			{
				return this.score.movesLeft;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000B5519 File Offset: 0x000B3919
		public MaterialAmount CoinsCollected
		{
			get
			{
				return this.score.CoinsCollected;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000B5526 File Offset: 0x000B3926
		public bool OutOfMoves
		{
			get
			{
				return this.score.movesLeft == 0;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600288D RID: 10381 RVA: 0x000B5536 File Offset: 0x000B3936
		public bool LevelWon
		{
			get
			{
				return this.score.success;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x000B5543 File Offset: 0x000B3943
		public TournamentType CurrentOngoingTournament
		{
			get
			{
				return this.currentOngoingTournament;
			}
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000B554B File Offset: 0x000B394B
		private void SetupTournamentPointsContainer(int tournamentMultiplier)
		{
			this.score.tournamentScore = new TournamentScore(this.currentOngoingTournament, 0, tournamentMultiplier);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000B5568 File Offset: 0x000B3968
		public bool IsObjectiveReached()
		{
			if (this.score.cheated)
			{
				return this.score.success;
			}
			Materials objectivesLeft = this.ObjectivesLeft;
			foreach (MaterialAmount materialAmount in objectivesLeft)
			{
				if (materialAmount.amount > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000B55F0 File Offset: 0x000B39F0
		public bool AreCoinsCollectable()
		{
			return this.score.AreCoinsCollectable();
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000B55FD File Offset: 0x000B39FD
		public TournamentScore GetTournamentScore()
		{
			return this.score.tournamentScore;
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000B560A File Offset: 0x000B3A0A
		public string GetCollectable()
		{
			return this.score.Config.collectable;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000B561C File Offset: 0x000B3A1C
		public LevelPlayMode GetLevelPlayMode()
		{
			return this.score.levelPlayMode;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000B562C File Offset: 0x000B3A2C
		public int GetLevel()
		{
			return this.score.Config.Level.level;
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000B5651 File Offset: 0x000B3A51
		public void CheatIncreaseTournamentScore()
		{
			if (!this.score.success)
			{
				this.score.tournamentScore.Increment(1);
				this.onScoreChanged.Dispatch();
			}
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000B567F File Offset: 0x000B3A7F
		public void HandleTournamentScoreIncreased(TournamentType eventType)
		{
			if (!this.score.success && eventType == this.CurrentOngoingTournament)
			{
				this.score.tournamentScore.Increment(1);
			}
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000B56AE File Offset: 0x000B3AAE
		public void HandleLineGemExplosionCreated()
		{
			if (!this.score.success)
			{
				this.score.lineGemsActivated++;
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000B56D3 File Offset: 0x000B3AD3
		public void HandleFishExplosionCreated()
		{
			if (!this.score.success)
			{
				this.score.fishActivated++;
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000B56F8 File Offset: 0x000B3AF8
		public void HandleRainbowExplosionCreated()
		{
			if (!this.score.success)
			{
				this.score.rainbowsActivated++;
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000B571D File Offset: 0x000B3B1D
		public void HandleLineGemCreated()
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("linegem", 1, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000B5747 File Offset: 0x000B3B47
		public void HandleBombCreated()
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("bomb", 1, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000B5771 File Offset: 0x000B3B71
		public void HandleRainbowCreated()
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("rainbow", 1, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000B579B File Offset: 0x000B3B9B
		public void HandleFishCreated()
		{
			if (!this.score.success)
			{
				this.score.AddAmount(new MaterialAmount("fish", 1, MaterialAmountUsage.Undefined, 0));
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000B57C5 File Offset: 0x000B3BC5
		public void MoreMovesBought()
		{
			this.score.postMoves++;
		}

		// 道具使用次数统计
		public void HandleBoostUsed(Boosts type)
		{
			switch (type)
			{
			case Boosts.boost_hammer:
				this.score.ingameHammerUsed++;
				break;
			case Boosts.boost_star:
				this.score.ingameStarUsed++;
				break;
			case Boosts.boost_rainbow:
				this.score.ingameRainbowUsed++;
				break;
			case Boosts.boost_pre_rainbow:
				this.score.preGameRainbowsUsed++;
				break;
			case Boosts.boost_pre_bomb_linegem:
				this.score.preGameBombLinegemUsed++;
				break;
			case Boosts.boost_pre_double_fish:
				this.score.preGameDoubleFishUsed++;
				break;
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000B58AE File Offset: 0x000B3CAE
		public bool IsObjective(string type)
		{
			return this.ObjectivesLeft.Contains(type);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000B58BC File Offset: 0x000B3CBC
		public void HandleStepCompleted(List<List<IMatchResult>> results)
		{
			this.resultIndex = 0;
			this.results = results;
			if (!BoardView.IsInvalidMove(results) && !this.matchEngine.AllowFreeSwapping)
			{
				if (!this.score.success || (this.score.movesLeft > 0 && this.score.success))
				{
					this.score.movesLeft--;
				}
				this.onMovesChanged.Dispatch(this.score.movesLeft);
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000B594C File Offset: 0x000B3D4C
		public void HandleStepFinished()
		{
			if (this.resultIndex >= this.results.Count)
			{
				Log.Warning("HandleStepFinished", string.Format("Index is {0} but results count is {1}", this.resultIndex, this.results.Count), null);
				return;
			}
			List<IMatchResult> list = this.results[this.resultIndex];
			foreach (IMatchResult input in list)
			{
				foreach (IScoreProcessor scoreProcessor in this.processors)
				{
					scoreProcessor.Process(input);
				}
			}
			this.onScoreChanged.Dispatch();
			this.resultIndex++;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000B5A5C File Offset: 0x000B3E5C
		public void AddMoves(int moveCount)
		{
			this.score.movesLeft += moveCount;
			this.onMovesChanged.Dispatch(this.score.movesLeft);
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000B5A88 File Offset: 0x000B3E88
		public void UpdateScoreStatusAndDispatchGameOverIfNeeded()
		{
			this.score.success = this.IsObjectiveReached();
			bool flag = (this.OutOfMoves && !this.isEditMode) || this.score.success;
			if (flag)
			{
				this.onGameOver.Dispatch(this.score);
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000B5AE2 File Offset: 0x000B3EE2
		public void CancelLevel()
		{
			this.score.cancelled = true;
			this.onGameOver.Dispatch(this.score);
		}

		// eli key point 直接完成关卡
		// Token: 0x060028A7 RID: 10407 RVA: 0x000B5B01 File Offset: 0x000B3F01
		public void FinishLevel(bool success, int movesTaken)
		{
			this.score.success = success;
			this.score.movesLeft -= movesTaken;
			this.score.cheated = true;
			this.onGameOver.Dispatch(this.score);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000B5B3F File Offset: 0x000B3F3F
		private void HandleOutOfPossibleMoves()
		{
			if (!this.score.success)
			{
				this.onGameOver.Dispatch(this.score);
			}
		}

		// Token: 0x04005285 RID: 21125
		public readonly Signal<Match3Score> onGameOver = new Signal<Match3Score>();

		// Token: 0x04005286 RID: 21126
		public readonly Signal<int> onMovesChanged = new Signal<int>();

		// Token: 0x04005287 RID: 21127
		public readonly Signal onScoreChanged = new Signal();

		// Token: 0x04005288 RID: 21128
		private Materials _objectivesLeft = new Materials();

		// Token: 0x04005289 RID: 21129
		private Match3Score score;

		// Token: 0x0400528A RID: 21130
		private bool isEditMode;

		// Token: 0x0400528B RID: 21131
		private List<IScoreProcessor> processors = new List<IScoreProcessor>();

		// Token: 0x0400528C RID: 21132
		private Materials objectives;

		// Token: 0x0400528D RID: 21133
		private PTMatchEngine matchEngine;

		// Token: 0x0400528E RID: 21134
		private int resultIndex;

		// Token: 0x0400528F RID: 21135
		private List<List<IMatchResult>> results;

		// Token: 0x04005290 RID: 21136
		public readonly TournamentType currentOngoingTournament;
	}
}
