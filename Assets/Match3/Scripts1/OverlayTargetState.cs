namespace Match3.Scripts1
{
	// Token: 0x020006A7 RID: 1703
	public struct OverlayTargetState
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x000C24D4 File Offset: 0x000C08D4
		public float ElapsedRatio
		{
			get
			{
				return (this.delayInSecs > 0f) ? (this.elapsedTime / this.delayInSecs) : 0f;
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000C24FD File Offset: 0x000C08FD
		public void Set(float delayInSecs, bool enableBoostMask, bool darken, bool maskDarken)
		{
			this.delayInSecs = delayInSecs;
			this.enableBoostMask = enableBoostMask;
			this.darken = darken;
			this.maskDarken = maskDarken;
			this.elapsedTime = 0f;
			this.shouldUpdate = true;
		}

		// Token: 0x040053C7 RID: 21447
		public bool shouldUpdate;

		// Token: 0x040053C8 RID: 21448
		public float delayInSecs;

		// Token: 0x040053C9 RID: 21449
		public float elapsedTime;

		// Token: 0x040053CA RID: 21450
		public bool enableBoostMask;

		// Token: 0x040053CB RID: 21451
		public bool darken;

		// Token: 0x040053CC RID: 21452
		public bool maskDarken;
	}
}
