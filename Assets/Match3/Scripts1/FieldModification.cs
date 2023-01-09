namespace Match3.Scripts1
{
	// Token: 0x02000570 RID: 1392
	public struct FieldModification
	{
		// Token: 0x0600247D RID: 9341 RVA: 0x000A2886 File Offset: 0x000A0C86
		public FieldModification(IsFieldSuitable isSuitable, Modification modification)
		{
			this.isSuitable = isSuitable;
			this.modification = modification;
		}

		// Token: 0x04005005 RID: 20485
		public IsFieldSuitable isSuitable;

		// Token: 0x04005006 RID: 20486
		public Modification modification;
	}
}
