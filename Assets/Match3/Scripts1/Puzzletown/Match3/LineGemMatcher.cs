using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005C4 RID: 1476
	public class LineGemMatcher : APatternMatcher, ISuperGemRemover, ITournamentMatcher
	{
		// Token: 0x06002674 RID: 9844 RVA: 0x000AC571 File Offset: 0x000AA971
		public LineGemMatcher(Fields fields, bool countTournamentMatches = false) : base(fields)
		{
			this.countTournamentMatches = countTournamentMatches;
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x000AC597 File Offset: 0x000AA997
		// (set) Token: 0x06002676 RID: 9846 RVA: 0x000AC59F File Offset: 0x000AA99F
		public bool CountScore
		{
			get
			{
				return this.countTournamentMatches;
			}
			set
			{
				this.countTournamentMatches = value;
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000AC5A8 File Offset: 0x000AA9A8
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
					}
				})
			};
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000AC5F8 File Offset: 0x000AA9F8
		public List<IMatchResult> RemoveSuperGems(Fields fields, List<IMatchResult> results)
		{
			this.fields = fields;
			List<IMatchResult> list = new List<IMatchResult>();
			for (int i = 0; i < results.Count; i++)
			{
				IMatchResult matchResult = results[i];
				IMatchGroup matchGroup = matchResult as IMatchGroup;
				if (matchGroup != null)
				{
					Group.SuperGemEnumerator superGems = matchGroup.Group.SuperGems;
					while (superGems.MoveNext())
					{
						Gem gem = superGems.Current;
						if (gem.IsLineGem() && (base.CanExplosionHit(gem) || base.RemovesDuplicates(matchGroup, gem)))
						{
							if (matchGroup is ILinegemRotatingExplosion)
							{
								int index = matchGroup.Group.IndexOf(gem);
								ILinegemRotatingExplosion linegemRotatingExplosion = (ILinegemRotatingExplosion)matchGroup;
								bool flag = linegemRotatingExplosion.Center.x == gem.position.x || (matchGroup is LinegemBombExplosion && (linegemRotatingExplosion.Center.x + 1 == gem.position.x || linegemRotatingExplosion.Center.x - 1 == gem.position.x));
								if (flag)
								{
									gem.type = GemType.LineHorizontal;
								}
								else
								{
									gem.type = GemType.LineVertical;
								}
								matchGroup.Group[index] = gem;
							}
							LineGemExplosion lineGemExplosion = new LineGemExplosion(fields, gem);
							this.onLineGemExplosionCreated.Dispatch();
							base.HitGems(lineGemExplosion);
							fields[gem.position].isExploded = true;
							list.Add(lineGemExplosion);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000AC798 File Offset: 0x000AAB98
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
						Gem gem = this.fields[intVector].gem;
						gem.type = GemType.LineVertical;
						if (group.Vertical)
						{
							gem.type = GemType.LineHorizontal;
						}
						LineGemMatch lineGemMatch = new LineGemMatch(gem, group);
						matchResults.Add(lineGemMatch);
						this.onLineGemCreated.Dispatch();
						if (this.countTournamentMatches)
						{
							matchResults.Add(new TournamentScoreMatch(TournamentType.Line, gem.position, (Vector3)gem.position));
						}
					}
				}
			}
		}

		// Token: 0x04005153 RID: 20819
		public readonly Signal onLineGemExplosionCreated = new Signal();

		// Token: 0x04005154 RID: 20820
		public readonly Signal onLineGemCreated = new Signal();

		// Token: 0x04005155 RID: 20821
		private bool countTournamentMatches;
	}
}
