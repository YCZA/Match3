using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Shared.Pooling;
using Match3.Scripts1.Wooga.Services.ErrorAnalytics;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000AF3 RID: 2803
	public abstract class AMatchEngine : IMatchEngine
	{
		// Token: 0x06004248 RID: 16968 RVA: 0x000B2A98 File Offset: 0x000B0E98
		public AMatchEngine(IBoardFactory boardFactory, IMoveProcessor moveProcessor)
		{
			this.onStepCompleted = new Signal<List<List<IMatchResult>>>();
			this.onSetupCompleted = new Signal<Fields>();
			this.onStepBegin = new Signal<Fields, Move>();
			this.onOutOfMoves = new Signal();
			this.onHighlightNextMove = new Signal<MatchCandidate>();
			this.boardFactory = boardFactory;
			this.moveProcessor = moveProcessor;
			this.fieldMappings = new FieldMappings(boardFactory.Fields);
			this.spawnerMappings = new SpawnerMappings(this.fieldMappings);
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06004249 RID: 16969 RVA: 0x000B2BAC File Offset: 0x000B0FAC
		// (set) Token: 0x0600424A RID: 16970 RVA: 0x000B2BB4 File Offset: 0x000B0FB4
		public Signal<List<List<IMatchResult>>> onStepCompleted { get; private set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x0600424B RID: 16971 RVA: 0x000B2BBD File Offset: 0x000B0FBD
		// (set) Token: 0x0600424C RID: 16972 RVA: 0x000B2BC5 File Offset: 0x000B0FC5
		public Signal<Fields> onSetupCompleted { get; private set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x000B2BCE File Offset: 0x000B0FCE
		// (set) Token: 0x0600424E RID: 16974 RVA: 0x000B2BD6 File Offset: 0x000B0FD6
		public Signal<Fields, Move> onStepBegin { get; private set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x000B2BDF File Offset: 0x000B0FDF
		// (set) Token: 0x06004250 RID: 16976 RVA: 0x000B2BE7 File Offset: 0x000B0FE7
		public Signal<MatchCandidate> onHighlightNextMove { get; private set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x000B2BF0 File Offset: 0x000B0FF0
		// (set) Token: 0x06004252 RID: 16978 RVA: 0x000B2BF8 File Offset: 0x000B0FF8
		public Signal onOutOfMoves { get; private set; }

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06004253 RID: 16979 RVA: 0x000B2C01 File Offset: 0x000B1001
		// (set) Token: 0x06004254 RID: 16980 RVA: 0x000B2C09 File Offset: 0x000B1009
		public bool IsResolvingMoves
		{
			get
			{
				return this._isResolvingMoves;
			}
			set
			{
				this._isResolvingMoves = value;
				if (!value && this.outOfMoves)
				{
					this.onOutOfMoves.Dispatch();
				}
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x000B2C2E File Offset: 0x000B102E
		// (set) Token: 0x06004256 RID: 16982 RVA: 0x000B2C36 File Offset: 0x000B1036
		public bool AllowFreeSwapping { get; set; }

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x000B2C3F File Offset: 0x000B103F
		// (set) Token: 0x06004258 RID: 16984 RVA: 0x000B2C47 File Offset: 0x000B1047
		public bool NoShuffle { get; set; }

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x06004259 RID: 16985 RVA: 0x000B2C50 File Offset: 0x000B1050
		public FieldMappings FieldMappings
		{
			get
			{
				return this.fieldMappings;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600425A RID: 16986 RVA: 0x000B2C58 File Offset: 0x000B1058
		public SpawnerMappings SpawnerMappings
		{
			get
			{
				return this.spawnerMappings;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600425B RID: 16987 RVA: 0x000B2C60 File Offset: 0x000B1060
		public int SumStepCascades
		{
			get
			{
				return this.sumStepCascades;
			}
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x000B2C68 File Offset: 0x000B1068
		public Move GetNextBestMove()
		{
			MatchCandidate matchCandidate = this.candidateMatcher.FindPossibleMove(this.boardFactory.Fields, true);
			return new Move(matchCandidate.start, matchCandidate.target, true, false, true);
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x000B2CA4 File Offset: 0x000B10A4
		public void Start()
		{
			while (!this.candidateMatcher.HasPossibleMove(this.boardFactory.Fields))
			{
				this.boardFactory.CreateBoard();
			}
			this.spawnerMappings.Initialize(this.boardFactory.Fields);
			this.onSetupCompleted.Dispatch(this.boardFactory.Fields);
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x000B2D08 File Offset: 0x000B1108
		public virtual void ProcessMove(Move move)
		{
			if (this.IsResolvingMoves)
			{
				return;
			}
			this.onStepBegin.Dispatch(this.boardFactory.Fields, move);
			List<IMatchResult> list = this.moveProcessor.Process(move, this.boardFactory.Fields);
			if (list.IsNullOrEmptyCollection())
			{
				return;
			}
			this.ProcessBoard(list, move, false);
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x000B2D68 File Offset: 0x000B1168
		public void ProcessBoard(List<IMatchResult> initialResults, Move move, bool forceTrickling)
		{
			this.sumStepCascades = -1;
			this.IsResolvingMoves = true;
			this.currentMove = move;
			this.results.Clear();
			this.results.Add(initialResults);
			bool flag = false;
			for (;;)
			{
				this.MatchAndTrickle(forceTrickling);
				if (this.NoShuffle)
				{
					break;
				}
				bool flag2 = this.TryShuffle();
				flag = (flag || flag2);
				if (!flag2)
				{
					goto IL_69;
				}
			}
			WoogaDebug.Log(new object[]
			{
				"shuffling aborted"
			});
			IL_69:
			if (flag)
			{
				this.onShuffled.Dispatch();
			}
			this.onStepCompleted.Dispatch(this.results);
			this.AfterTrickling();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x000B2E08 File Offset: 0x000B1208
		public void ApplyBoosts(List<IBoost> boosts)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			foreach (IBoost boost in boosts)
			{
				if (boost.IsValid())
				{
					list.AddRange(boost.Apply());
				}
			}
			this.ApplyBoostResult(list);
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x000B2E7C File Offset: 0x000B127C
		public void ApplyBoost(IBoost boost)
		{
			if (!boost.IsValid())
			{
				return;
			}
			List<IMatchResult> list = boost.Apply();
			this.ApplyBoostResult(list);
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x000B2EA4 File Offset: 0x000B12A4
		private void ApplyBoostResult(List<IMatchResult> results)
		{
			this.onStepBegin.Dispatch(this.boardFactory.Fields, default(Move));
			results.AddRange(this.RemoveSupergems(results));
			this.ProcessResults(results);
			this.PostProcessResults(results);
			this.ProcessBoard(results, default(Move), true);
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x000B2EFC File Offset: 0x000B12FC
		private void MatchAndTrickle(bool forceTrickling)
		{
			bool flag = !this.currentMove.Equals(default(Move));
			bool flag2 = !flag;
			for (;;)
			{
				List<IMatchResult> list = ListPool<IMatchResult>.Create(10);
				if (flag)
				{
					foreach (ISwapProcessor swapProcessor in this.swapProcessors)
					{
						swapProcessor.ProcessSwap(this.boardFactory.Fields, this.currentMove);
					}
					foreach (ASwapMatcher aswapMatcher in this.swapMatchers)
					{
						aswapMatcher.InitMatcherWithMove(this.currentMove);
						aswapMatcher.MarkMatches(this.currentMove);
					}
				}
				foreach (AInstantMatcher ainstantMatcher in this.instantMatchers)
				{
					list.AddRange(ainstantMatcher.RemoveMatches(this.boardFactory.Fields, this.currentMove, null));
					this.ProcessResults(list);
					list.AddRange(this.RemoveSupergems(list));
				}
				List<Group> groups = this.floodFill.FindMatches(this.boardFactory.Fields);
				list.AddRange(this.RemovePatternMatches(groups));
				if (flag)
				{
					foreach (ASwapMatcher aswapMatcher2 in this.swapMatchers)
					{
						List<IMatchResult> list2 = ListPool<IMatchResult>.Create(10);
						list2.AddRange(aswapMatcher2.RemoveMatches(this.boardFactory.Fields, this.currentMove, null));
						this.ProcessResults(list2);
						list.AddRange(list2);
						list.AddRange(this.RemoveSupergems(list2.ToList<IMatchResult>()));
					}
				}
				if (!list.IsNullOrEmptyCollection() || forceTrickling)
				{
					forceTrickling = false;
					if (!list.IsNullOrEmptyCollection())
					{
						this.PostProcessResults(list);
						this.results.Add(list);
					}
					this.results.Add(this.TrickleDown());
					flag = false;
					this.currentMove = default(Move);
					this.sumStepCascades++;
					this.boardFactory.Fields.ResetExplosions();
				}
				else
				{
					bool flag3 = false;
					bool flag4 = false;
					if (this.results.Count == 1 && !this.AllowFreeSwapping)
					{
						flag4 = (this.results[0][0] is BlockedSwap);
						flag3 = !flag4;
						if (flag3)
						{
							this.results.Add(this.moveProcessor.Process(this.currentMove, this.boardFactory.Fields));
						}
					}
					else
					{
						int count = this.results.Count;
						this.PostTricklingProcessResults(this.results);
						if (count != this.results.Count)
						{
							this.boardFactory.Fields.ResetExplosions();
							continue;
						}
					}
					this.boardFactory.Fields.ResetExplosions();
					if (flag3 || flag4 || flag2)
					{
						break;
					}
					int count2 = this.results.Count;
					foreach (ITrigger trigger in this.triggers)
					{
						trigger.ExecuteTrigger(this.boardFactory.Fields, this.results);
					}
					if (count2 == this.results.Count)
					{
						break;
					}
				}
			}
			this.boardFactory.Fields.ResetProcessedGems();
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x000B3318 File Offset: 0x000B1718
		public void AddMatcher(IMatcher matcher)
		{
			if (matcher is ASwapMatcher)
			{
				this.swapMatchers.Add((ASwapMatcher)matcher);
				this.candidateMatcher.AddSwapMatcher((ASwapMatcher)matcher);
			}
			else if (matcher is AInstantMatcher)
			{
				this.instantMatchers.Add((AInstantMatcher)matcher);
			}
			else
			{
				this.matchers.Add(matcher);
				this.candidateMatcher.AddPatternMatcher((APatternMatcher)matcher);
			}
			if (matcher is ISuperGemRemover)
			{
				this.superGemRemovers.Add((ISuperGemRemover)matcher);
			}
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x000B33B1 File Offset: 0x000B17B1
		public void AddTrickler(ITrickler trickler)
		{
			this.tricklers.Add(trickler);
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x000B33BF File Offset: 0x000B17BF
		public void AddTrigger(ITrigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x000B33CD File Offset: 0x000B17CD
		public void AddSwapProcessor(ISwapProcessor processor)
		{
			this.swapProcessors.Add(processor);
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x000B33DB File Offset: 0x000B17DB
		public void AddProcessor(IMatchProcessor processor)
		{
			this.processors.Add(processor);
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x000B33E9 File Offset: 0x000B17E9
		public void AddPostProcessor(IMatchProcessor processor)
		{
			this.postProcessors.Add(processor);
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x000B33F7 File Offset: 0x000B17F7
		public void AddPostTricklingProcessor(IMatchProcessor processor)
		{
			this.postTricklingProcessors.Add(processor);
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x000B3408 File Offset: 0x000B1808
		protected List<IMatchResult> RemovePatternMatches(List<Group> groups)
		{
			List<IMatchResult> list = ListPool<IMatchResult>.Create(10);
			foreach (IMatcher matcher in this.matchers)
			{
				List<IMatchResult> list2 = ListPool<IMatchResult>.Create(10);
				list2.AddRange(matcher.RemoveMatches(this.boardFactory.Fields, this.currentMove, groups));
				this.ProcessResults(list2);
				list.AddRange(list2);
				list.AddRange(this.RemoveSupergems(list2));
			}
			return list;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x000B34A8 File Offset: 0x000B18A8
		protected List<IMatchResult> RemoveSupergems(List<IMatchResult> results)
		{
			List<IMatchResult> list = ListPool<IMatchResult>.Create(10);
			foreach (ISuperGemRemover superGemRemover in this.superGemRemovers)
			{
				List<IMatchResult> collection = superGemRemover.RemoveSuperGems(this.boardFactory.Fields, results);
				this.ProcessResults(collection);
				list.AddRange(collection);
			}
			if (list.Count > 0)
			{
				list.AddRange(this.RemoveSupergems(list));
			}
			return list;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x000B3540 File Offset: 0x000B1940
		protected void ProcessResults(List<IMatchResult> results)
		{
			foreach (IMatchProcessor matchProcessor in this.processors)
			{
				results.AddRange(matchProcessor.Process(this.boardFactory.Fields, results));
			}
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x000B35B0 File Offset: 0x000B19B0
		protected void PostProcessResults(List<IMatchResult> results)
		{
			foreach (IMatchProcessor matchProcessor in this.postProcessors)
			{
				results.AddRange(matchProcessor.Process(this.boardFactory.Fields, results));
			}
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x000B3620 File Offset: 0x000B1A20
		protected void PostTricklingProcessResults(List<List<IMatchResult>> resultsList)
		{
			List<List<IMatchResult>> list = new List<List<IMatchResult>>();
			bool flag = false;
			foreach (IMatchProcessor matchProcessor in this.postTricklingProcessors)
			{
				for (int i = 0; i < resultsList.Count; i++)
				{
					List<IMatchResult> list2 = resultsList[i];
					List<IMatchResult> list3 = matchProcessor.Process(this.boardFactory.Fields, list2).ToList<IMatchResult>();
					if (!list3.IsNullOrEmptyEnumerable())
					{
						if (!(list3.First<IMatchResult>() is IMultipleStepResult))
						{
							List<IMatchResult> collection = this.RemoveSupergems(list3);
							if (!collection.IsNullOrEmptyCollection())
							{
								list3.AddRange(collection);
							}
						}
						list.Add(list3);
						flag = true;
					}
				}
			}
			foreach (List<IMatchResult> item in list)
			{
				this.ProcessResults(item);
				this.PostProcessResults(item);
				resultsList.Add(item);
			}
			if (flag)
			{
				resultsList.Add(this.TrickleDown());
			}
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x000B3770 File Offset: 0x000B1B70
		protected List<IMatchResult> TrickleDown()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			foreach (ITrickler trickler in this.tricklers)
			{
				list.AddRange(trickler.Trickle(this.boardFactory.Fields));
			}
			return list;
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x000B37E4 File Offset: 0x000B1BE4
		protected virtual bool TryShuffle()
		{
			ShuffleResult shuffleResult = default(ShuffleResult);
			Fields fields = this.boardFactory.Fields;
			if (!this.candidateMatcher.HasPossibleMove(fields))
			{
				if (!this.candidateMatcher.CanCreateMove(fields))
				{
					Log.Warning("M3_Shuffle", "No moves and it seems impossible to fix it", null);
					this.outOfMoves = true;
					return false;
				}
				shuffleResult = this.candidateMatcher.Shuffle(fields);
				if (shuffleResult.results.IsNullOrEmptyCollection())
				{
					Log.Warning("M3_Shuffle", "Shuffling failed.", null);
					this.outOfMoves = true;
					return false;
				}
				this.results.Add(new List<IMatchResult>
				{
					shuffleResult
				});
			}
			return !shuffleResult.results.IsNullOrEmptyCollection();
		}

		// Token: 0x06004272 RID: 17010
		protected abstract void AfterTrickling();

		// Token: 0x04006B56 RID: 27478
		public readonly Signal onShuffled = new Signal();

		// Token: 0x04006B59 RID: 27481
		private bool _isResolvingMoves;

		// Token: 0x04006B5C RID: 27484
		protected FieldMappings fieldMappings;

		// Token: 0x04006B5D RID: 27485
		protected SpawnerMappings spawnerMappings;

		// Token: 0x04006B5E RID: 27486
		protected List<ITrigger> triggers = new List<ITrigger>();

		// Token: 0x04006B5F RID: 27487
		protected List<IMatcher> matchers = new List<IMatcher>();

		// Token: 0x04006B60 RID: 27488
		protected List<AInstantMatcher> instantMatchers = new List<AInstantMatcher>();

		// Token: 0x04006B61 RID: 27489
		protected List<ASwapMatcher> swapMatchers = new List<ASwapMatcher>();

		// Token: 0x04006B62 RID: 27490
		protected List<ISuperGemRemover> superGemRemovers = new List<ISuperGemRemover>();

		// Token: 0x04006B63 RID: 27491
		protected List<ITrickler> tricklers = new List<ITrickler>();

		// Token: 0x04006B64 RID: 27492
		protected List<IMatchProcessor> processors = new List<IMatchProcessor>();

		// Token: 0x04006B65 RID: 27493
		protected List<IMatchProcessor> postProcessors = new List<IMatchProcessor>();

		// Token: 0x04006B66 RID: 27494
		protected List<IMatchProcessor> postTricklingProcessors = new List<IMatchProcessor>();

		// Token: 0x04006B67 RID: 27495
		protected List<ISwapProcessor> swapProcessors = new List<ISwapProcessor>();

		// Token: 0x04006B68 RID: 27496
		protected IMoveProcessor moveProcessor;

		// Token: 0x04006B69 RID: 27497
		protected IBoardFactory boardFactory;

		// Token: 0x04006B6A RID: 27498
		protected FloodFillMatcher floodFill = new FloodFillMatcher();

		// Token: 0x04006B6B RID: 27499
		protected CandidateMatcher candidateMatcher = new CandidateMatcher();

		// Token: 0x04006B6C RID: 27500
		protected Move currentMove;

		// Token: 0x04006B6D RID: 27501
		private bool outOfMoves;

		// Token: 0x04006B6E RID: 27502
		protected int sumStepCascades;

		// Token: 0x04006B6F RID: 27503
		protected List<List<IMatchResult>> results = new List<List<IMatchResult>>();
	}
}
