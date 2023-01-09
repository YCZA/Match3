using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005FD RID: 1533
	public struct CannonCharged : IMatchResult
	{
		// Token: 0x06002755 RID: 10069 RVA: 0x000AE9E0 File Offset: 0x000ACDE0
		public CannonCharged(Gem chargingGem, IntVector2 chargedCannonPosition, int chargeAmount)
		{
			this.chargedCannonPosition = chargedCannonPosition;
			this.chargingGem = chargingGem;
			this.chargeAmount = chargeAmount;
		}

		// Token: 0x040051DC RID: 20956
		public readonly Gem chargingGem;

		// Token: 0x040051DD RID: 20957
		public readonly IntVector2 chargedCannonPosition;

		// Token: 0x040051DE RID: 20958
		public readonly int chargeAmount;
	}
}
