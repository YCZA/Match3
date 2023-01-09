using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AF RID: 1455
	public class BombSwapMatcher : ASwapMatcher
	{
		// Token: 0x0600260F RID: 9743 RVA: 0x000A9FA4 File Offset: 0x000A83A4
		public BombSwapMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000A9FB8 File Offset: 0x000A83B8
		protected override bool HasMatchInternal()
		{
			return this.from.type == GemType.Bomb && this.to.type == GemType.Bomb;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000A9FDC File Offset: 0x000A83DC
		protected override List<IMatchResult> FindMatches()
		{
			this.superExplosions.Clear();
			this.fields[this.from.position].gem.type = GemType.Undefined;
			Gem to = this.to;
			to.type = GemType.ActivatedSuperBomb;
			BombExplosion bombExplosion = new BombExplosion(this.fields, to, this.from.position, true);
			this.superExplosions.Add(bombExplosion);
			this.fields[this.from.position].gem.type = GemType.Bomb;
			return this.superExplosions;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000AA078 File Offset: 0x000A8478
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			if (!base.HasMatch(move))
			{
				return list;
			}
			this.fields[move.from].ResetExplosion();
			this.fields[move.to].ResetExplosion();
			list = this.FindMatches();
			foreach (IMatchResult matchResult in list)
			{
				BombExplosion bombExplosion = (BombExplosion)matchResult;
				this.fields[move.from].gem.type = GemType.Undefined;
				this.fields[bombExplosion.Center].AssignGem(bombExplosion.gem);
				this.fields[bombExplosion.Center].isExploded = true;
				base.HitGems(bombExplosion);
			}
			return list;
		}

		// Token: 0x04005108 RID: 20744
		private List<IMatchResult> superExplosions = new List<IMatchResult>();
	}
}
