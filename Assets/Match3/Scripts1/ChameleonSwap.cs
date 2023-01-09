namespace Match3.Scripts1
{
	// Token: 0x02000604 RID: 1540
	public struct ChameleonSwap
	{
		// Token: 0x06002779 RID: 10105 RVA: 0x000AF74D File Offset: 0x000ADB4D
		public ChameleonSwap(IntVector2 origin, IntVector2 target, IntVector2 facingDirection)
		{
			this.origin = origin;
			this.target = target;
			this.facingDirection = facingDirection;
		}

		// Token: 0x040051EE RID: 20974
		public readonly IntVector2 origin;

		// Token: 0x040051EF RID: 20975
		public readonly IntVector2 target;

		// Token: 0x040051F0 RID: 20976
		public readonly IntVector2 facingDirection;
	}
}
