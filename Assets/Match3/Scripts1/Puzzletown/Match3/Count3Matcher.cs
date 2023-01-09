using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BE RID: 1470
	public class Count3Matcher : APatternMatcher
	{
		// Token: 0x06002645 RID: 9797 RVA: 0x000AB426 File Offset: 0x000A9826
		public Count3Matcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000AB430 File Offset: 0x000A9830
		protected override void InitPatterns()
		{
			this.patterns = new Pattern[]
			{
				new Pattern(new int[,]
				{
					{
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
					}
				})
			};
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000AB480 File Offset: 0x000A9880
		protected override void CreateResultsFromGroups(List<Group> groups, List<IMatchResult> matchResults, Move move)
		{
			foreach (Group group in groups)
			{
				Match match = new Match(group, true);
				matchResults.Add(match);
			}
		}
	}
}
