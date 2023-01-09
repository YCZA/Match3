using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BF RID: 1471
	public class DroppableMatcher : AInstantMatcher
	{
		// Token: 0x06002648 RID: 9800 RVA: 0x000AB4E8 File Offset: 0x000A98E8
		public DroppableMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000AB4FC File Offset: 0x000A98FC
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			this.matchResults.Clear();
			for (int i = 0; i < this.fields.size; i++)
			{
				for (int j = 0; j < this.fields.size; j++)
				{
					Field field = this.fields[i, j];
					if (this.CheckMove(move, field.gridPosition) && this.CheckField(field))
					{
						this.matchResults.Add(new DroppableCollected(field.gem));
					}
				}
			}
			return this.matchResults;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000AB59C File Offset: 0x000A999C
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			List<IMatchResult> list = this.FindMatches(move, groups).ToList<IMatchResult>();
			foreach (IMatchResult matchResult in list)
			{
				DroppableCollected droppableCollected = (DroppableCollected)matchResult;
				this.fields[droppableCollected.Position].HasGem = false;
			}
			return list;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000AB618 File Offset: 0x000A9A18
		private bool CheckMove(Move move, IntVector2 pos)
		{
			return (move.to != pos && move.from != pos) || move == default(Move);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000AB65B File Offset: 0x000A9A5B
		private bool CheckField(Field field)
		{
			return field.IsExitDropItems && field.CanSwap && field.gem.IsDroppable;
		}

		// Token: 0x04005121 RID: 20769
		private List<IMatchResult> matchResults = new List<IMatchResult>();
	}
}
