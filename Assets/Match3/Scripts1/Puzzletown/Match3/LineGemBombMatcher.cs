using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BC RID: 1468
	public class LineGemBombMatcher : ACombineSupergemMatcher
	{
		// Token: 0x0600263F RID: 9791 RVA: 0x000AB121 File Offset: 0x000A9521
		public LineGemBombMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000AB12A File Offset: 0x000A952A
		protected override bool HasSupergemCombination(Gem from, Gem to)
		{
			return (from.type == GemType.Bomb && to.IsLineGem()) || (to.type == GemType.Bomb && from.IsLineGem());
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000AB160 File Offset: 0x000A9560
		protected override void CreateCombinedExplosion(Gem from, Gem to, List<IMatchResult> results)
		{
			IntVector2 position = to.position;
			GemType type = to.type;
			GemType type2 = from.type;
			this.fields[to.position].gem.type = GemType.Undefined;
			this.fields[from.position].gem.type = GemType.Undefined;
			results.Add(new LinegemBombExplosion(this.fields, position, from.position));
			this.fields[to.position].gem.type = type;
			this.fields[from.position].gem.type = type2;
		}
	}
}
