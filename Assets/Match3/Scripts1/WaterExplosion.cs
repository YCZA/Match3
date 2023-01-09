using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000628 RID: 1576
	public struct WaterExplosion : IFieldModifierExplosion, IMatchResult
	{
		// Token: 0x0600281A RID: 10266 RVA: 0x000B254C File Offset: 0x000B094C
		public WaterExplosion(Field field)
		{
			this.position = field.gridPosition;
			this.countForObjective = !field.IsDefinedGemSpawner;
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600281B RID: 10267 RVA: 0x000B2569 File Offset: 0x000B0969
		public IntVector2 Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600281C RID: 10268 RVA: 0x000B2571 File Offset: 0x000B0971
		public string Type
		{
			get
			{
				return "water";
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600281D RID: 10269 RVA: 0x000B2578 File Offset: 0x000B0978
		public bool CountForObjective
		{
			get
			{
				return this.countForObjective;
			}
		}

		// Token: 0x0400523C RID: 21052
		private readonly bool countForObjective;

		// Token: 0x0400523D RID: 21053
		private readonly IntVector2 position;
	}
}
