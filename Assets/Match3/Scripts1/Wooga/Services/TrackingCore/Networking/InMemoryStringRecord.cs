namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x0200046F RID: 1135
	public class InMemoryStringRecord
	{
		// Token: 0x060020E7 RID: 8423 RVA: 0x0008ACA3 File Offset: 0x000890A3
		public InMemoryStringRecord(string data)
		{
			this.Data = data;
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x0008ACB2 File Offset: 0x000890B2
		public static InMemoryStringRecord Create(string data)
		{
			return new InMemoryStringRecord(data);
		}

		// Token: 0x04004BAA RID: 19370
		public bool Consumed;

		// Token: 0x04004BAB RID: 19371
		public readonly string Data;
	}
}
