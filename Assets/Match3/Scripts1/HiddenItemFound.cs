using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x02000618 RID: 1560
	public struct HiddenItemFound : IMatchResult
	{
		// Token: 0x060027D3 RID: 10195 RVA: 0x000B0F7F File Offset: 0x000AF37F
		public HiddenItemFound(int id, IntVector2 hitPosition)
		{
			this.id = id;
			this.hitPosition = hitPosition;
		}

		// Token: 0x04005221 RID: 21025
		public int id;

		// Token: 0x04005222 RID: 21026
		public IntVector2 hitPosition;
	}
}
