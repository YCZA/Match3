using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005B6 RID: 1462
	public class ClimberMatcher : AInstantMatcher
	{
		// Token: 0x0600262E RID: 9774 RVA: 0x000AAB2F File Offset: 0x000A8F2F
		public ClimberMatcher(Fields fields, FieldMappings fieldMappings) : base(fields)
		{
			this.mappings = fieldMappings;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000AAB4C File Offset: 0x000A8F4C
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			this.matchResults.Clear();
			for (int i = 0; i < this.fields.size; i++)
			{
				for (int j = 0; j < this.fields.size; j++)
				{
					Field field = this.fields[i, j];
					if (field.isClimberExit && field.CanSwap && field.gem.color == GemColor.Climber && ((move.to != field.gridPosition && move.from != field.gridPosition) || move == default(Move)))
					{
						this.matchResults.Add(new ClimberCollected(field.gem));
					}
				}
			}
			return this.matchResults;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000AAC38 File Offset: 0x000A9038
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			List<IMatchResult> list = this.FindMatches(move, groups).ToList<IMatchResult>();
			foreach (IMatchResult matchResult in list)
			{
				ClimberCollected climberCollected = (ClimberCollected)matchResult;
				this.fields[climberCollected.Position].HasGem = false;
			}
			this.mappings.UpdateFieldMappings(this.fields);
			return list;
		}

		// Token: 0x0400511C RID: 20764
		private List<IMatchResult> matchResults = new List<IMatchResult>();

		// Token: 0x0400511D RID: 20765
		private FieldMappings mappings;
	}
}
