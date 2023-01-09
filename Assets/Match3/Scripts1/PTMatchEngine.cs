using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;

// Token: 0x0200062D RID: 1581
namespace Match3.Scripts1
{
	public class PTMatchEngine : AMatchEngine
	{
		// Token: 0x06002832 RID: 10290 RVA: 0x000B38A9 File Offset: 0x000B1CA9
		public PTMatchEngine(IBoardFactory boardFactory, IMoveProcessor moveProcessor) : base(boardFactory, moveProcessor)
		{
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000B38BE File Offset: 0x000B1CBE
		public Fields Fields
		{
			get
			{
				return this.boardFactory.Fields;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x000B38CB File Offset: 0x000B1CCB
		public PTBoardFactory BoardFactory
		{
			get
			{
				return (PTBoardFactory)this.boardFactory;
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000B38D8 File Offset: 0x000B1CD8
		public void Reload(Fields fields)
		{
			this.BoardFactory.LoadBoard(fields);
			this.fieldMappings.UpdateFieldMappings(fields);
			this.spawnerMappings.Initialize(fields);
			HiddenItemProcessor processor = this.GetProcessor<HiddenItemProcessor>();
			processor.Reload(fields);
			base.onSetupCompleted.Dispatch(fields);
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000B3923 File Offset: 0x000B1D23
		public T GetProcessor<T>() where T : class, IMatchProcessor
		{
			return this.processors.FirstOrDefault((IMatchProcessor p) => p is T) as T;
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000B3946 File Offset: 0x000B1D46
		public void HandleNeedPossibleMoves()
		{
			if (this.candidateMatcher.CurrentMatchCandidate == null)
			{
				this.candidateMatcher.FindMatchCandidate(this.boardFactory.Fields);
			}
			base.onHighlightNextMove.Dispatch(this.candidateMatcher.CurrentMatchCandidate);
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000B3985 File Offset: 0x000B1D85
		public void HandleBoostUsed(Boosts type)
		{
			this.onBoostUsed.Dispatch(type);
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000B3993 File Offset: 0x000B1D93
		public void SwitchToLastHurray()
		{
			this.lastHurray = true;
			this.boardFactory.GemFactory.AddExtraColorProbability();
			this.boardFactory.GemFactory.StopSpawningMatchablesFromRatios();
			this.DisableTournamentScoring();
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000B39C2 File Offset: 0x000B1DC2
		protected override bool TryShuffle()
		{
			return !this.lastHurray && base.TryShuffle();
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000B39D7 File Offset: 0x000B1DD7
		protected override void AfterTrickling()
		{
			if (!this.lastHurray && this.sumStepCascades > 0)
			{
				this.sumLevelCascades += this.sumStepCascades;
			}
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000B3A04 File Offset: 0x000B1E04
		protected void DisableTournamentScoring()
		{
			foreach (IMatcher matcher in this.matchers)
			{
				ITournamentMatcher tournamentMatcher = matcher as ITournamentMatcher;
				if (tournamentMatcher != null)
				{
					tournamentMatcher.CountScore = false;
				}
			}
		}

		// Token: 0x04005242 RID: 21058
		public readonly Signal<Boosts> onBoostUsed = new Signal<Boosts>();

		// Token: 0x04005243 RID: 21059
		public int sumLevelCascades;

		// Token: 0x04005244 RID: 21060
		private bool lastHurray;
	}
}
