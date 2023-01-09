namespace Match3.Scripts1.Shared.M3Engine
{
	// Token: 0x02000B01 RID: 2817
	public interface IFinalMovement : IMatchResult
	{
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600428B RID: 17035
		// (set) Token: 0x0600428C RID: 17036
		bool IsFinal { get; set; }

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600428D RID: 17037
		IntVector2 Position { get; }
	}
}
