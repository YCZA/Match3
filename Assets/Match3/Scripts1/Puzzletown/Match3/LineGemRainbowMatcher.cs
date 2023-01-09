using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;
using Shared.Pooling;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005BD RID: 1469
	public class LineGemRainbowMatcher : ACombineSupergemMatcher
	{
		// Token: 0x06002642 RID: 9794 RVA: 0x000AB217 File Offset: 0x000A9617
		public LineGemRainbowMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000AB220 File Offset: 0x000A9620
		protected override bool HasSupergemCombination(Gem from, Gem to)
		{
			return (to.color == GemColor.Rainbow && from.IsLineGem()) || (from.color == GemColor.Rainbow && to.IsLineGem());
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000AB258 File Offset: 0x000A9658
		protected override void CreateCombinedExplosion(Gem from, Gem to, List<IMatchResult> results)
		{
			Gem gem = from;
			Gem rainbow = to;
			if (from.color == GemColor.Rainbow)
			{
				gem = to;
				rainbow = from;
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
				gem3.type = gem.type;
				LineGemExplosion lineGemExplosion = new LineGemExplosion(this.fields, gem3);
				list.Add(lineGemExplosion);
				rainbowSuperGemExplosion.AddShowSupergemPosition(gem3.position);
			}
			results.Add(rainbowSuperGemExplosion);
			Pool<Group>.ReturnToPool(allMatchableGems);
			base.RemoveDuplicatesInExplosions(list);
			results.AddRange(list);
			this.fields[gem.position].gem.type = gem.type;
			for (int k = 0; k < group.Count; k++)
			{
				Gem gem4 = group[k];
				this.fields[gem4.position].willExplode = false;
			}
			results.Add(new Match(group, true));
		}
	}
}
