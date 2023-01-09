using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000579 RID: 1401
	public class BoostHammer : ABoost
	{
		// Token: 0x060024CE RID: 9422 RVA: 0x000A4A10 File Offset: 0x000A2E10
		public BoostHammer(Fields fields, IntVector2 position, bool isWaterLevel) : base(fields, position)
		{
			this.isWaterLevel = isWaterLevel;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000A4A21 File Offset: 0x000A2E21
		public override bool IsValid()
		{
			return this.fields[this.position].CanBeTargetedByBoost(Boosts.boost_hammer);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000A4A3C File Offset: 0x000A2E3C
		public override List<IMatchResult> Apply()
		{
			List<IMatchResult> list = new List<IMatchResult>();
			Field field = this.fields[this.position];
			bool spreadWater = false;
			IMatchGroup match;
			if (field.IsBlocked || field.GemBlocked || field.gem.IsCovered || field.isGrowingWindow)
			{
				match = new HammerFieldMatch(field);
			}
			else
			{
				match = new Match(new Group(this.fields[this.position].gem), true);
				this.fields[this.position].HasGem = false;
				spreadWater = this.isWaterLevel;
			}
			list.Add(new HammerMatch(match, this.position, spreadWater));
			return list;
		}

		// Token: 0x04005065 RID: 20581
		private readonly bool isWaterLevel;
	}
}
