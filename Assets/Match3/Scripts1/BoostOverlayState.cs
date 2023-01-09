using Match3.Scripts1.Puzzletown.Match3;

namespace Match3.Scripts1
{
	// Token: 0x0200071B RID: 1819
	public struct BoostOverlayState
	{
		// Token: 0x06002CFF RID: 11519 RVA: 0x000D0C8C File Offset: 0x000CF08C
		public BoostOverlayState(bool isOn, bool isInstant, Boosts type)
		{
			this.isOn = isOn;
			this.shouldChangeInstantly = isInstant;
			this.boostType = type;
		}

		// Token: 0x04005688 RID: 22152
		public readonly bool isOn;

		// Token: 0x04005689 RID: 22153
		public readonly bool shouldChangeInstantly;

		// Token: 0x0400568A RID: 22154
		public readonly Boosts boostType;
	}
}
