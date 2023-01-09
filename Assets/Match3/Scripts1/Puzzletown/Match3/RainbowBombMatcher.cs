using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BB RID: 1467
	public class RainbowBombMatcher : ACombineSupergemMatcher
	{
		// Token: 0x0600263C RID: 9788 RVA: 0x000AAF1B File Offset: 0x000A931B
		public RainbowBombMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000AAF24 File Offset: 0x000A9324
		protected override bool HasSupergemCombination(Gem from, Gem to)
		{
			return (to.color == GemColor.Rainbow && from.type == GemType.Bomb) || (from.color == GemColor.Rainbow && to.type == GemType.Bomb);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000AAF60 File Offset: 0x000A9360
		protected override void CreateCombinedExplosion(Gem from, Gem to, List<IMatchResult> results)
		{
			Gem rainbow = from;
			Gem gem = to;
			if (from.type == GemType.Bomb)
			{
				rainbow = to;
				gem = from;
			}
			this.fields[gem.position].gem.type = GemType.Undefined;
			Group allMatchableGems = this.fields.GetAllMatchableGems(gem.color);
			RainbowSuperGemExplosion rainbowSuperGemExplosion = new RainbowSuperGemExplosion(rainbow, allMatchableGems, gem);
			Group group = Pool<Group>.Get();
			for (int i = 0; i < allMatchableGems.Count; i++)
			{
				Gem gem2 = allMatchableGems[i];
				if (!base.CanExplosionHit(gem2) || gem2.type != GemType.Undefined)
				{
					this.fields[gem2.position].willExplode = true;
					group.Add(gem2);
					allMatchableGems.RemoveAt(i--);
				}
			}
			List<IMatchResult> list = ListPool<IMatchResult>.Create(10);
			for (int j = 0; j < allMatchableGems.Count; j++)
			{
				Gem gem3 = allMatchableGems[j];
				BombExplosion bombExplosion = new BombExplosion(this.fields, gem3, gem3.position, false);
				rainbowSuperGemExplosion.AddShowSupergemPosition(gem3.position);
				list.Add(bombExplosion);
			}
			results.Add(rainbowSuperGemExplosion);
			Pool<Group>.ReturnToPool(allMatchableGems);
			base.RemoveDuplicatesInExplosions(list);
			results.AddRange(list);
			this.fields[gem.position].gem.type = GemType.Bomb;
			for (int k = 0; k < group.Count; k++)
			{
				Gem gem4 = group[k];
				this.fields[gem4.position].willExplode = false;
			}
			results.Add(new Match(group, true));
		}
	}
}
