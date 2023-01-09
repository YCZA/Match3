using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200057A RID: 1402
	public class BoostRainbow : ABoost
	{
		// Token: 0x060024D1 RID: 9425 RVA: 0x000A4B03 File Offset: 0x000A2F03
		public BoostRainbow(Fields fields, IntVector2 position) : base(fields, position)
		{
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000A4B0D File Offset: 0x000A2F0D
		public override bool IsValid()
		{
			return this.fields[this.position].CanBeTargetedByBoost(Boosts.boost_rainbow);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000A4B28 File Offset: 0x000A2F28
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			Field field = this.fields[this.position];
			RainbowExplosion explosion = new RainbowExplosion(this.fields, field.gem.color, this.position, this.position);
			if (field.gem.IsCovered)
			{
				explosion.Group.Remove(field.gem);
				explosion.Fields.Add(field.gridPosition);
			}
			foreach (Gem gem in explosion.Group)
			{
				this.fields[gem.position].HasGem = false;
			}
			list.Add(new HammerRainbowExplosion(explosion));
			return list;
		}
	}
}
