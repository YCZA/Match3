using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AE RID: 1454
	public class BombInstantMatcher : AInstantMatcher
	{
		// Token: 0x0600260C RID: 9740 RVA: 0x000A9E08 File Offset: 0x000A8208
		public BombInstantMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000A9E1C File Offset: 0x000A821C
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			this.activatedExplosions.Clear();
			for (int i = 0; i < this.fields.size; i++)
			{
				for (int j = 0; j < this.fields.size; j++)
				{
					Field field = this.fields[i, j];
					Gem gem = field.gem;
					if (field.CanExplode && gem.IsActivatedBombGem())
					{
						Gem gem2 = gem;
						gem2.type = GemType.Undefined;
						bool isSuperBomb = gem.type == GemType.ActivatedSuperBomb;
						BombExplosion bombExplosion = new BombExplosion(this.fields, gem2, gem2.position, isSuperBomb);
						this.activatedExplosions.Add(bombExplosion);
					}
				}
			}
			base.RemoveDuplicatesInExplosions(this.activatedExplosions);
			return this.activatedExplosions;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000A9EEC File Offset: 0x000A82EC
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			List<IMatchResult> list = this.FindMatches(move, groups).ToList<IMatchResult>();
			foreach (IMatchResult matchResult in list)
			{
				BombExplosion bombExplosion = (BombExplosion)matchResult;
				if (bombExplosion.gem.IsActivatedBombGem())
				{
					this.fields[bombExplosion.Center].isExploded = true;
				}
				this.fields[bombExplosion.Center].AssignGem(bombExplosion.gem);
				base.HitGems(bombExplosion);
			}
			return list;
		}

		// Token: 0x04005107 RID: 20743
		private List<IMatchResult> activatedExplosions = new List<IMatchResult>();
	}
}
