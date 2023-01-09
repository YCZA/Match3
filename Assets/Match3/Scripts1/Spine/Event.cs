namespace Match3.Scripts1.Spine
{
	// Token: 0x0200020D RID: 525
	public class Event
	{
		// Token: 0x06000FD3 RID: 4051 RVA: 0x0002614C File Offset: 0x0002454C
		public Event(float time, EventData data)
		{
			this.Time = time;
			this.Data = data;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x00026162 File Offset: 0x00024562
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0002616A File Offset: 0x0002456A
		public EventData Data { get; private set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x00026173 File Offset: 0x00024573
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0002617B File Offset: 0x0002457B
		public int Int { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x00026184 File Offset: 0x00024584
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0002618C File Offset: 0x0002458C
		public float Float { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x00026195 File Offset: 0x00024595
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x0002619D File Offset: 0x0002459D
		public string String { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x000261A6 File Offset: 0x000245A6
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x000261AE File Offset: 0x000245AE
		public float Time { get; private set; }

		// Token: 0x06000FDE RID: 4062 RVA: 0x000261B7 File Offset: 0x000245B7
		public override string ToString()
		{
			return this.Data.Name;
		}
	}
}
