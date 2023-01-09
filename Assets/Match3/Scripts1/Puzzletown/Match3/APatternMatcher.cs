using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AB RID: 1451
	public abstract class APatternMatcher : AMatcher
	{
		// Token: 0x060025EF RID: 9711 RVA: 0x000A9415 File Offset: 0x000A7815
		protected APatternMatcher(Fields fields) : base(fields)
		{
			this.InitPatterns();
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000A9424 File Offset: 0x000A7824
		protected APatternMatcher() : base(null)
		{
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000A942D File Offset: 0x000A782D
		public List<Group> FindMatchesFromGroups(List<Group> groups)
		{
			return this.CheckPatterns(groups);
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000A9436 File Offset: 0x000A7836
		public List<Group> FindMatches(Move move, Group group)
		{
			return this.CheckPatterns(group);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000A9440 File Offset: 0x000A7840
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			List<Group> groups2 = this.FindMatchesFromGroups(groups);
			this.CreateResultsFromGroups(groups2, list, move);
			return list;
		}

		// Token: 0x060025F4 RID: 9716
		protected abstract void InitPatterns();

		// Token: 0x060025F5 RID: 9717
		protected abstract void CreateResultsFromGroups(List<Group> groups, List<IMatchResult> results, Move move);

		// Token: 0x060025F6 RID: 9718 RVA: 0x000A9468 File Offset: 0x000A7868
		protected List<Group> CheckPatterns(List<Group> groups)
		{
			List<Group> list = ListPool<Group>.Create(10);
			for (int i = 0; i < groups.Count; i++)
			{
				List<Group> list2 = this.CheckPatterns(groups[i]);
				if (list2 != null)
				{
					list.AddRange(list2);
				}
			}
			return list;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000A94B0 File Offset: 0x000A78B0
		protected List<Group> CheckPatterns(Group group)
		{
			List<Group> list = ListPool<Group>.Create(10);
			if (group.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < this.patterns.Length; i++)
			{
				list.AddIfNotNull(this.CheckPattern(group, this.patterns[i]));
				if (group.Count < 3)
				{
					break;
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			list.ReleasePooled<Group>();
			return null;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000A9528 File Offset: 0x000A7928
		protected Group CheckPattern(Group group, Pattern pattern)
		{
			for (int i = 0; i <= group.Width - pattern.width; i++)
			{
				for (int j = 0; j <= group.Height - pattern.height; j++)
				{
					Group group2 = this.CheckPattern(group, pattern, new IntVector2(i, j));
					if (group2 != null)
					{
						return group2;
					}
				}
			}
			return null;
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000A958C File Offset: 0x000A798C
		protected Group CheckPattern(Group group, Pattern pattern, IntVector2 patternOffset)
		{
			Group group2 = Pool<Group>.Get();
			if (group.Width < pattern.width || group.Height < pattern.height)
			{
				group2.ReturnToPool<Group>();
				return null;
			}
			IntVector2 b = new IntVector2(group.LeftGem.position.x, group.DownGem.position.y) + patternOffset;
			for (int i = 0; i < group.Count; i++)
			{
				IntVector2 intVector = group[i].position - b;
				intVector.y = pattern.height - intVector.y - 1;
				if (intVector.x >= 0 && intVector.x < pattern.width)
				{
					if (intVector.y >= 0 && intVector.y < pattern.height)
					{
						if (pattern.positions[intVector.y, intVector.x] > 0)
						{
							group2.Add(group[i]);
						}
					}
				}
			}
			if (group2.Count != pattern.count)
			{
				group2.ReturnToPool<Group>();
				return null;
			}
			for (int j = 0; j < group2.Count; j++)
			{
				group.RemovePosition(group2[j].position);
			}
			group2.Color = group.Color;
			if (group.Count == 0)
			{
				return group2;
			}
			this.ExtendMatchingGroup(group, group2);
			for (int k = 0; k < group.Count; k++)
			{
				IntVector2 position = group[k].position;
				foreach (IntVector2 intVector2 in APatternMatcher.directions)
				{
					if (group2.Contains(position + intVector2) && group2.Contains(position + 2 * intVector2))
					{
						group2.Add(group[k]);
						group.RemoveAt(k--);
						break;
					}
				}
			}
			return group2;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000A97D4 File Offset: 0x000A7BD4
		protected virtual void ExtendMatchingGroup(Group group, Group matchingGroup)
		{
		}

		// Token: 0x04005101 RID: 20737
		protected static readonly IntVector2[] directions = new IntVector2[]
		{
			IntVector2.Up,
			IntVector2.Right,
			IntVector2.Down,
			IntVector2.Left
		};

		// Token: 0x04005102 RID: 20738
		protected Pattern[] patterns;
	}
}
