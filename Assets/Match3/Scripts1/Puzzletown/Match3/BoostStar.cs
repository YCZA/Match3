using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200057B RID: 1403
	public class BoostStar : ABoost
	{
		// Token: 0x060024D4 RID: 9428 RVA: 0x000A4C1C File Offset: 0x000A301C
		public BoostStar(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000A4C28 File Offset: 0x000A3028
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			Field field = this.fields[this.position];
			Gem gem = field.gem;
			bool flag = false;
			if (!field.HasGem)
			{
				gem = new Gem(GemColor.Blue);
				gem.position = this.position;
			}
			if (!field.GemBlocked && gem.color == GemColor.Cannonball)
			{
				this.fields[this.position].gem.color = GemColor.Blue;
				flag = true;
			}
			gem.type = GemType.LineHorizontal;
			LineGemExplosion horizontal = new LineGemExplosion(this.fields, gem);
			gem.type = GemType.LineVertical;
			LineGemExplosion vertical = new LineGemExplosion(this.fields, gem);
			StarExplosion explosion = new StarExplosion(horizontal, vertical, gem.position);
			if (flag)
			{
				gem.type = GemType.Undefined;
				this.fields[this.position].gem.color = GemColor.Cannonball;
				explosion.Group.ReplacePosition(gem);
			}
			foreach (Gem gem2 in explosion.Group)
			{
				this.fields[gem2.position].HasGem = false;
			}
			list.Add(new HammerStarExplosion(explosion));
			return list;
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000A4D9C File Offset: 0x000A319C
		public override bool IsValid()
		{
			return this.fields[this.position].CanBeTargetedByBoost(Boosts.boost_star);
		}
	}
}
