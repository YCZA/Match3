using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

// Token: 0x020005B2 RID: 1458
namespace Match3.Scripts1
{
	public class CandidateMatcher
	{
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x000AA27E File Offset: 0x000A867E
		// (set) Token: 0x0600261B RID: 9755 RVA: 0x000AA286 File Offset: 0x000A8686
		public MatchCandidate CurrentMatchCandidate { get; private set; }

		// Token: 0x0600261C RID: 9756 RVA: 0x000AA28F File Offset: 0x000A868F
		public void AddPatternMatcher(APatternMatcher matcher)
		{
			this.patternMatchers.Add(matcher);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000AA29D File Offset: 0x000A869D
		public void AddSwapMatcher(ASwapMatcher matcher)
		{
			this.swapMatchers.Add(matcher);
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000AA2AB File Offset: 0x000A86AB
		public MatchCandidate FindMatchCandidate(Fields fields)
		{
			this.CurrentMatchCandidate = this.FindPossibleMove(fields, true);
			return this.CurrentMatchCandidate;
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000AA2C4 File Offset: 0x000A86C4
		public ShuffleResult Shuffle(Fields fields)
		{
			this.replacedGems.Clear();
			this.EnsureColors(fields);
			int num = 0;
			for (;;)
			{
				this.shuffleResults = new List<IMatchResult>();
				num++;
				if (num > 60)
				{
					break;
				}
				this.gemsToShuffle.Clear();
				this.gemsToShuffle.AddRange(this.originalGems);
				foreach (Gem gem in this.originalGems)
				{
					Gem gem2 = this.gemsToShuffle.RandomElement(true);
					this.shuffleResults.Add(new Move(gem2.position, gem.position, false, false, false));
					fields[gem.position].AssignGem(gem2);
				}
				if (this.HasPossibleMove(fields))
				{
					goto Block_3;
				}
			}
			WoogaDebug.Log(new object[]
			{
				"Exceeds shuffling limit of 60"
			});
			return new ShuffleResult(this.shuffleResults);
			Block_3:
			if (!this.replacedGems.IsNullOrEmptyCollection())
			{
				this.shuffleResults.AddRange(this.replacedGems);
			}
			return new ShuffleResult(this.shuffleResults);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000AA400 File Offset: 0x000A8800
		public bool CanCreateMove(Fields fields)
		{
			this.CollectColors(fields);
			return this.colors.Sum((KeyValuePair<GemColor, int> kvp) => kvp.Value) > 2 && this.originalGems.Count > 1;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000AA454 File Offset: 0x000A8854
		public bool HasPossibleMove(Fields fields)
		{
			this.CurrentMatchCandidate = null;
			MatchCandidate matchCandidate = this.FindPossibleMove(fields, false);
			if (matchCandidate != null && matchCandidate.isBestCandidate)
			{
				this.CurrentMatchCandidate = matchCandidate;
			}
			return matchCandidate != null;
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000AA490 File Offset: 0x000A8890
		public MatchCandidate FindPossibleMove(Fields fields, bool findAll)
		{
			IntVector2 zero = IntVector2.Zero;
			IntVector2[] array = new IntVector2[]
			{
				IntVector2.Right,
				IntVector2.Up
			};
			this.candidates.Clear();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					zero.x = i;
					zero.y = j;
					foreach (IntVector2 b in array)
					{
						Move move = new Move(zero, zero + b, false, false, true);
						if (fields.IsSwapPossible(move.from, move.to))
						{
							foreach (ASwapMatcher aswapMatcher in this.swapMatchers)
							{
								aswapMatcher.InitMatcherWithMove(move);
								if (aswapMatcher.HasMatch(move))
								{
									Group group = new Group
									{
										fields[move.from].gem,
										fields[move.to].gem
									};
									return new MatchCandidate(group, move.from, move.to, true);
								}
							}
							fields.SwapGems(move.from, move.to);
							List<Group> list = this.floodFill.FindMatch(move, fields);
							fields.SwapGems(move.from, move.to);
							foreach (Group group2 in list)
							{
								for (int l = 0; l < this.patternMatchers.Count; l++)
								{
									APatternMatcher apatternMatcher = this.patternMatchers[l];
									List<Group> list2 = apatternMatcher.FindMatches(move, group2);
									if (list2 != null)
									{
										bool flag = false;
										foreach (Group group3 in list2)
										{
											if (group3.Count >= 3)
											{
												IntVector2 start = move.from;
												IntVector2 intVector = move.to;
												if (!group3.Contains(intVector))
												{
													start = move.to;
													intVector = move.from;
												}
												MatchCandidate matchCandidate = new MatchCandidate(group3, start, intVector, findAll);
												if (!findAll)
												{
													return matchCandidate;
												}
												this.candidates.Add(new CandidateMatcher.Candidate(l, matchCandidate));
												flag = true;
											}
										}
										if (flag)
										{
											break;
										}
									}
								}
							}
							list.ReturnToPool<Group>();
						}
					}
				}
			}
			if (this.candidates.Count > 0)
			{
				this.candidates.Sort();
				return this.candidates[0].match;
			}
			return null;
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000AA81C File Offset: 0x000A8C1C
		protected void EnsureColors(Fields fields)
		{
			this.CollectColors(fields);
			if (!this.HasEnoughGems())
			{
				GemColor gemColor = GemColor.Undefined;
				int num = 0;
				while (gemColor == GemColor.Undefined && num < 2)
				{
					foreach (KeyValuePair<GemColor, int> keyValuePair in this.colors)
					{
						if (keyValuePair.Value == 2 - num)
						{
							gemColor = keyValuePair.Key;
							break;
						}
					}
					num++;
				}
				for (int i = 0; i < num; i++)
				{
					Gem randomGemOfDifferentColor = fields.GetRandomGemOfDifferentColor(gemColor);
					fields[randomGemOfDifferentColor.position].gem.color = gemColor;
					this.replacedGems.Add(new ReplaceGem(fields[randomGemOfDifferentColor.position].gem));
				}
				this.CollectColors(fields);
			}
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000AA920 File Offset: 0x000A8D20
		protected void CollectColors(Fields fields)
		{
			this.colors.Clear();
			this.originalGems.Clear();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.isOn && field.HasGem && field.gem.IsMatchable && field.CanMove && field.gem.CanShuffle)
					{
						if (field.CanMove && field.gem.CanShuffle)
						{
							this.originalGems.Add(field.gem);
						}
						if (!this.colors.ContainsKey(field.gem.color))
						{
							this.colors[field.gem.color] = 0;
						}
						Dictionary<GemColor, int> dictionary;
						GemColor color;
						(dictionary = this.colors)[color = field.gem.color] = dictionary[color] + 1;
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

		// Token: 0x06002625 RID: 9765 RVA: 0x000AAA54 File Offset: 0x000A8E54
		protected bool HasEnoughGems()
		{
			foreach (KeyValuePair<GemColor, int> keyValuePair in this.colors)
			{
				if (keyValuePair.Value >= 3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400510A RID: 20746
		protected List<IMatchResult> shuffleResults = new List<IMatchResult>();

		// Token: 0x0400510B RID: 20747
		protected List<Gem> gemsToShuffle = new List<Gem>();

		// Token: 0x0400510C RID: 20748
		protected List<Gem> originalGems = new List<Gem>();

		// Token: 0x0400510D RID: 20749
		protected Dictionary<GemColor, int> colors = new Dictionary<GemColor, int>();

		// Token: 0x0400510E RID: 20750
		protected List<IMatchResult> replacedGems = new List<IMatchResult>();

		// Token: 0x0400510F RID: 20751
		private readonly FloodFillMatcher floodFill = new FloodFillMatcher();

		// Token: 0x04005110 RID: 20752
		private readonly List<APatternMatcher> patternMatchers = new List<APatternMatcher>();

		// Token: 0x04005111 RID: 20753
		private readonly List<ASwapMatcher> swapMatchers = new List<ASwapMatcher>();

		// Token: 0x04005112 RID: 20754
		private readonly List<CandidateMatcher.Candidate> candidates = new List<CandidateMatcher.Candidate>();

		// Token: 0x020005B3 RID: 1459
		private struct Candidate : IComparable<CandidateMatcher.Candidate>
		{
			// Token: 0x06002627 RID: 9767 RVA: 0x000AAAC9 File Offset: 0x000A8EC9
			public Candidate(int order, MatchCandidate match)
			{
				this.order = order;
				this.match = match;
			}

			// Token: 0x06002628 RID: 9768 RVA: 0x000AAAD9 File Offset: 0x000A8ED9
			public int CompareTo(CandidateMatcher.Candidate other)
			{
				return this.order.CompareTo(other.order);
			}

			// Token: 0x04005115 RID: 20757
			public int order;

			// Token: 0x04005116 RID: 20758
			public MatchCandidate match;
		}
	}
}
