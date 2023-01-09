using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005C6 RID: 1478
	public class RainbowMatcher : APatternMatcher
	{
		// Token: 0x0600267D RID: 9853 RVA: 0x000AC9AF File Offset: 0x000AADAF
		public RainbowMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000AC9C4 File Offset: 0x000AADC4
		protected override void InitPatterns()
		{
			this.patterns = new Pattern[]
			{
				new Pattern(new int[,]
				{
					{
						1,
						1,
						1,
						1,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						1
					},
					{
						1
					},
					{
						1
					},
					{
						1
					},
					{
						1
					}
				})
			};
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000ACA14 File Offset: 0x000AAE14
		protected override void CreateResultsFromGroups(List<Group> groups, List<IMatchResult> matchResults, Move move)
		{
			foreach (Group group in groups)
			{
				if (group.IsCompletelyCovered())
				{
					matchResults.Add(new Match(group, true));
				}
				else
				{
					IntVector2 intVector = base.FindIdealSuperGemPosition(group, move);
					if (intVector == Fields.invalidPos)
					{
						matchResults.Add(new Match(group, true));
					}
					else
					{
						this.onRainbowGemCreated.Dispatch();
						Gem gem = this.fields[intVector].gem;
						gem.color = GemColor.Rainbow;
						gem.type = GemType.Undefined;
						RainbowMatch rainbowMatch = new RainbowMatch(gem, group);
						matchResults.Add(rainbowMatch);
					}
				}
			}
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000ACAFC File Offset: 0x000AAEFC
		protected override void ExtendMatchingGroup(Group group, Group matchingGroup)
		{
			IntVector2 a = matchingGroup.LowerLeft + new IntVector2(matchingGroup.Width / 2, matchingGroup.Height / 2);
			foreach (IntVector2 intVector in APatternMatcher.directions)
			{
				if (group.Contains(a + intVector) && group.Contains(a + 2 * intVector))
				{
					matchingGroup.Add(group.GetGemAtPosition(a + intVector));
					matchingGroup.Add(group.GetGemAtPosition(a + 2 * intVector));
					group.RemovePosition(a + intVector);
					group.RemovePosition(a + 2 * intVector);
					break;
				}
			}
		}

		// Token: 0x04005157 RID: 20823
		public readonly Signal onRainbowGemCreated = new Signal();
	}
}
