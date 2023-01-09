using System;
using System.Collections.Generic;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A2 RID: 1442
	public class GroupRemover
	{
		// Token: 0x060025BF RID: 9663 RVA: 0x000A8244 File Offset: 0x000A6644
		public void RemoveGroups(Fields fields, GemFactory gemFactory)
		{
			for (;;)
			{
				List<Group> groups = this.floodFill.FindMatches(fields);
				List<Group> list = ListPool<Group>.Create(10);
				List<Group> list2 = this.match3.FindMatchesFromGroups(groups);
				list.AddRange(list2);
				list2.ReturnToPool<Group>();
				list2 = this.fishMatcher.FindMatchesFromGroups(groups);
				list.AddRange(list2);
				list2.ReturnToPool<Group>();
				if (list.Count == 0)
				{
					break;
				}
				for (int i = 0; i < list.Count; i++)
				{
					this.RemoveGroup(list[i], gemFactory, fields);
				}
				ListPool<Group>.Release(list);
			}
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x000A82DC File Offset: 0x000A66DC
		private void RemoveGroup(Group match, GemFactory gemFactory, Fields fields)
		{
			GemColor color = gemFactory.GetRandomDifferentGem(match.Color).color;
			IntVector2 intVector = Fields.invalidPos;
			int num = 0;
			bool flag = false;
			while (!flag && num < match.Count)
			{
				intVector = match[num].position;
				flag = !fields.prePositionedGems.Contains(intVector);
				num++;
			}
			if (num == match.Count && !flag)
			{
				throw new ArgumentException("There is a preplaced group in the level layout");
			}
			fields[intVector].gem.color = color;
		}

		// Token: 0x040050F1 RID: 20721
		private FloodFillMatcher floodFill = new FloodFillMatcher();

		// Token: 0x040050F2 RID: 20722
		private Count3Matcher match3 = new Count3Matcher(null);

		// Token: 0x040050F3 RID: 20723
		private FishMatcher fishMatcher = new FishMatcher(null, null, null, false, string.Empty);
	}
}
