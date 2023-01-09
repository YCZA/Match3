using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000580 RID: 1408
	public abstract class ABoost : IBoost
	{
		// Token: 0x060024E3 RID: 9443 RVA: 0x000A49FA File Offset: 0x000A2DFA
		protected ABoost(Fields fields, IntVector2 position)
		{
			this.position = position;
			this.fields = fields;
		}

		// Token: 0x060024E4 RID: 9444
		public abstract List<IMatchResult> Apply();

		// Token: 0x060024E5 RID: 9445
		public abstract bool IsValid();

		// Token: 0x04005068 RID: 20584
		protected IntVector2 position;

		// Token: 0x04005069 RID: 20585
		protected readonly Fields fields;
	}
}
