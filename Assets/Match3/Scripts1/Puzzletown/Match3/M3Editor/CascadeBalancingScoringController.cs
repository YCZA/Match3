using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000559 RID: 1369
	public class CascadeBalancingScoringController : IScoringController
	{
		// Token: 0x06002400 RID: 9216 RVA: 0x0009FFD0 File Offset: 0x0009E3D0
		public CascadeBalancingScoringController(LevelConfig config, PTMatchEngine matchEngine)
		{
			this.score = new Match3Score(config, LevelPlayMode.Regular);
			this.objectives = new Materials(config.data.objectives);
			this.processors.Add(new GroupProcessor(this.score, TournamentType.Undefined));
			this.processors.Add(new FieldModifierProcessor(this.score));
			this.processors.Add(new ScoreCollectedProcessor(this.score));
			this.processors.Add(new HiddenItemScoreProcessor(this.score));
			matchEngine.onStepCompleted.AddListener(new Action<List<List<IMatchResult>>>(this.HandleStepCompleted));
			matchEngine.onShuffled.AddListener(delegate
			{
				this.score.reshuffles++;
			});
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002401 RID: 9217 RVA: 0x000A00AE File Offset: 0x0009E4AE
		public TournamentType CurrentOngoingTournament
		{
			get
			{
				return TournamentType.Undefined;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x000A00B4 File Offset: 0x0009E4B4
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

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x000A0150 File Offset: 0x0009E550
		public int MovesLeft
		{
			get
			{
				return this.score.movesLeft;
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000A0160 File Offset: 0x0009E560
		public TournamentScore GetTournamentScore()
		{
			return default(TournamentScore);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000A0176 File Offset: 0x0009E576
		public void HandleTournamentScoreIncreased(TournamentType eventType)
		{
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000A0178 File Offset: 0x0009E578
		public void UpdateScoreStatusAndDispatchGameOverIfNeeded()
		{
			this.score.success = this.IsObjectiveReached();
			if (this.score.success)
			{
				this.onGameOver.Dispatch(this.score);
			}
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000A01AC File Offset: 0x0009E5AC
		public void ForceGameOver()
		{
			this.score.success = true;
			this.onGameOver.Dispatch(this.score);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000A01CC File Offset: 0x0009E5CC
		public void HandleStepCompleted(List<List<IMatchResult>> results)
		{
			foreach (List<IMatchResult> list in results)
			{
				foreach (IMatchResult input in list)
				{
					foreach (IScoreProcessor scoreProcessor in this.processors)
					{
						scoreProcessor.Process(input);
					}
				}
			}
			if (!this.score.success)
			{
				this.score.movesLeft--;
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000A02CC File Offset: 0x0009E6CC
		private bool IsObjectiveReached()
		{
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

		// Token: 0x04004F7C RID: 20348
		public readonly Signal<Match3Score> onGameOver = new Signal<Match3Score>();

		// Token: 0x04004F7D RID: 20349
		private readonly Materials _objectivesLeft = new Materials();

		// Token: 0x04004F7E RID: 20350
		private readonly Match3Score score;

		// Token: 0x04004F7F RID: 20351
		private readonly Materials objectives;

		// Token: 0x04004F80 RID: 20352
		private readonly List<IScoreProcessor> processors = new List<IScoreProcessor>();
	}
}
