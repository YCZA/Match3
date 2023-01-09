using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200069C RID: 1692
	public class ClimberHighlighter : IObjectiveHighlighter
	{
		// Token: 0x06002A39 RID: 10809 RVA: 0x000C1568 File Offset: 0x000BF968
		public List<IMatchResult> GetHighlights(Fields fields)
		{
			Group group = new Group();
			IntVector2[] allModifierFieldPositions = fields.GetAllModifierFieldPositions(this.helpsClimber);
			foreach (IntVector2 vec in allModifierFieldPositions)
			{
				group.Add(fields[vec].gem);
			}
			return new List<IMatchResult>
			{
				new ObjectiveHighlights(group)
			};
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000C15DD File Offset: 0x000BF9DD
		public bool IsValid(string objective)
		{
			return objective.Equals("climber");
		}

		// Token: 0x04005398 RID: 21400
		private Predicate<Field> helpsClimber = (Field f) => f.HasGem && f.CanMove && f.gem.type == GemType.ClimberGem;
	}
}
