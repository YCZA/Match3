using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AD RID: 1453
	public class BombMatcher : APatternMatcher, ITournamentMatcher, ISuperGemRemover
	{
		// Token: 0x06002606 RID: 9734 RVA: 0x000A9AA4 File Offset: 0x000A7EA4
		public BombMatcher(Fields fields, bool countTournamentMatches = false) : base(fields)
		{
			this.countTournamentMatches = countTournamentMatches;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000A9AC0 File Offset: 0x000A7EC0
		protected override void InitPatterns()
		{
			this.patterns = new Pattern[]
			{
				new Pattern(new int[,]
				{
					{
						1,
						0,
						0
					},
					{
						1,
						0,
						0
					},
					{
						1,
						1,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						1,
						1,
						1
					},
					{
						1,
						0,
						0
					},
					{
						1,
						0,
						0
					}
				}),
				new Pattern(new int[,]
				{
					{
						1,
						1,
						1
					},
					{
						0,
						0,
						1
					},
					{
						0,
						0,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						0,
						0,
						1
					},
					{
						0,
						0,
						1
					},
					{
						1,
						1,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						1,
						0,
						0
					},
					{
						1,
						1,
						1
					},
					{
						1,
						0,
						0
					}
				}),
				new Pattern(new int[,]
				{
					{
						1,
						1,
						1
					},
					{
						0,
						1,
						0
					},
					{
						0,
						1,
						0
					}
				}),
				new Pattern(new int[,]
				{
					{
						0,
						0,
						1
					},
					{
						1,
						1,
						1
					},
					{
						0,
						0,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						0,
						1,
						0
					},
					{
						0,
						1,
						0
					},
					{
						1,
						1,
						1
					}
				}),
				new Pattern(new int[,]
				{
					{
						0,
						1,
						0
					},
					{
						1,
						1,
						1
					},
					{
						0,
						1,
						0
					}
				})
			};
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x000A9BC4 File Offset: 0x000A7FC4
		// (set) Token: 0x06002609 RID: 9737 RVA: 0x000A9BCC File Offset: 0x000A7FCC
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

		// Token: 0x0600260A RID: 9738 RVA: 0x000A9BD8 File Offset: 0x000A7FD8
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
						if (gem.type == GemType.Bomb && (base.CanExplosionHit(gem) || base.RemovesDuplicates(matchGroup, gem)))
						{
							int index = matchGroup.Group.IndexOf(gem);
							Gem gem2 = gem;
							gem2.type = GemType.ActivatedBomb;
							matchGroup.Group[index] = gem2;
							fields[gem.position].AssignGem(gem2);
							BombExplosion bombExplosion = new BombExplosion(fields, gem2, gem2.position, false);
							fields[gem.position].isExploded = true;
							base.HitGems(bombExplosion);
							list.Add(bombExplosion);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000A9CEC File Offset: 0x000A80EC
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
					if (intVector == Fields.invalidPos || this.fields[intVector].gem.type == GemType.Bomb)
					{
						matchResults.Add(new Match(group, true));
					}
					else
					{
						this.onBombGemCreated.Dispatch();
						Gem gem = this.fields[intVector].gem;
						gem.type = GemType.Bomb;
						BombMatch bombMatch = new BombMatch(gem, group);
						matchResults.Add(bombMatch);
						if (this.countTournamentMatches)
						{
							matchResults.Add(new TournamentScoreMatch(TournamentType.Bomb, intVector, (Vector3)intVector));
						}
					}
				}
			}
		}

		// Token: 0x04005105 RID: 20741
		public readonly Signal onBombGemCreated = new Signal();

		// Token: 0x04005106 RID: 20742
		private bool countTournamentMatches;
	}
}
