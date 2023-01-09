using Match3.Scripts1.Puzzletown.Config;

// Token: 0x02000A4A RID: 2634
namespace Match3.Scripts1
{
	public struct TournamentBadgeIcon
	{
		// Token: 0x06003F1B RID: 16155 RVA: 0x00142701 File Offset: 0x00140B01
		public TournamentBadgeIcon(TournamentType tType, bool isGlow)
		{
			this.type = tType;
			this.glow = isGlow;
		}

		// Token: 0x04006899 RID: 26777
		public readonly TournamentType type;

		// Token: 0x0400689A RID: 26778
		public readonly bool glow;
	}
}
