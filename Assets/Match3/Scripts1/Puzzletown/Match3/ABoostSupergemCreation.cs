using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005A6 RID: 1446
	public abstract class ABoostSupergemCreation : ABoost
	{
		// Token: 0x060025D6 RID: 9686 RVA: 0x000A8D57 File Offset: 0x000A7157
		protected ABoostSupergemCreation(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000A8D64 File Offset: 0x000A7164
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			Gem gem = this.fields[this.position].gem;
			Gem gem2 = this.CreateGem(gem);
			this.fields[this.position].gem = gem2;
			BoostSpawnResult boostSpawnResult = new BoostSpawnResult(gem2);
			list.Add(boostSpawnResult);
			return list;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000A8DC2 File Offset: 0x000A71C2
		public override bool IsValid()
		{
			return this.fields[this.position].gem.type == GemType.Undefined && this.fields[this.position].gem.IsMatchable;
		}

		// Token: 0x060025D9 RID: 9689
		protected abstract Gem CreateGem(Gem oldGem);
	}
}
