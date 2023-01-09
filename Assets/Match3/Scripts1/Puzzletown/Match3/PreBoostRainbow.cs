using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200057D RID: 1405
	public class PreBoostRainbow : PreGameBoost
	{
		// Token: 0x060024DA RID: 9434 RVA: 0x000A4E1E File Offset: 0x000A321E
		public PreBoostRainbow(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000A4E28 File Offset: 0x000A3228
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			this.fields[this.position].gem.color = GemColor.Rainbow;
			list.Add(new BoostSpawnResult(this.fields[this.position].gem));
			return list;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000A4E7F File Offset: 0x000A327F
		public override bool IsValid()
		{
			return base.IsReplaceableNormalGem(this.position);
		}
	}
}
