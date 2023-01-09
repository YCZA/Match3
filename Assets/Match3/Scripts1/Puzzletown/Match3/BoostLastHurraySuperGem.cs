using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A4 RID: 1444
	public class BoostLastHurraySuperGem : ABoost
	{
		// Token: 0x060025CF RID: 9679 RVA: 0x000A8B66 File Offset: 0x000A6F66
		public BoostLastHurraySuperGem(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000A8B70 File Offset: 0x000A6F70
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			Match match = new Match(new Group(this.fields[this.position].gem), true);
			this.fields[this.position].HasGem = false;
			list.Add(match);
			return list;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000A8BCA File Offset: 0x000A6FCA
		public override bool IsValid()
		{
			return this.fields[this.position].gem.IsSuperGem;
		}
	}
}
