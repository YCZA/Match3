using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000621 RID: 1569
	public struct StackedGemExplosion : IExplosionResult, IMatchGroup, IMatchResult
	{
		// Token: 0x060027F5 RID: 10229 RVA: 0x000B1A8D File Offset: 0x000AFE8D
		public StackedGemExplosion(Gem gem, Group group)
		{
			this.gem = gem;
			this.group = group;
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000B1A9D File Offset: 0x000AFE9D
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000B1AA5 File Offset: 0x000AFEA5
		public IntVector2 Center
		{
			get
			{
				return this.gem.position;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000B1AB2 File Offset: 0x000AFEB2
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04005235 RID: 21045
		public Gem gem;

		// Token: 0x04005236 RID: 21046
		private readonly Group group;
	}
}
