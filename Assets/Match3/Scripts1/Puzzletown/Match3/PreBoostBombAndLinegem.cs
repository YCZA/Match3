using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200057F RID: 1407
	public class PreBoostBombAndLinegem : PreGameBoost
	{
		// Token: 0x060024E0 RID: 9440 RVA: 0x000A4E9B File Offset: 0x000A329B
		public PreBoostBombAndLinegem(Fields fields, IntVector2 position, IntVector2 bombPosition) : base(fields, position)
		{
			this.bombPosition = bombPosition;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000A4EC0 File Offset: 0x000A32C0
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			this.fields[this.position].gem.type = this.lineDirections[RandomHelper.Next(this.lineDirections.Length)];
			list.Add(new BoostSpawnResult(this.fields[this.position].gem));
			this.fields[this.bombPosition].gem.type = GemType.Bomb;
			list.Add(new BoostSpawnResult(this.fields[this.bombPosition].gem));
			return list;
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000A4F6B File Offset: 0x000A336B
		public override bool IsValid()
		{
			return base.IsReplaceableNormalGem(this.position) && base.IsReplaceableNormalGem(this.bombPosition);
		}

		// Token: 0x04005066 RID: 20582
		private readonly IntVector2 bombPosition;

		// Token: 0x04005067 RID: 20583
		private readonly GemType[] lineDirections = new GemType[]
		{
			GemType.LineHorizontal,
			GemType.LineVertical
		};
	}
}
