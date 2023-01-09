using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005FB RID: 1531
	public struct CannonballExplosion : IMatchResult
	{
		// Token: 0x0600274C RID: 10060 RVA: 0x000AE704 File Offset: 0x000ACB04
		public CannonballExplosion(IntVector2 position, IntVector2 createdFrom)
		{
			this.position = position;
			this.createdFrom = createdFrom;
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000AE714 File Offset: 0x000ACB14
		public IntVector2 CreatedFrom
		{
			get
			{
				return this.createdFrom;
			}
		}

		// Token: 0x040051D7 RID: 20951
		public IntVector2 position;

		// Token: 0x040051D8 RID: 20952
		private readonly IntVector2 createdFrom;
	}
}
