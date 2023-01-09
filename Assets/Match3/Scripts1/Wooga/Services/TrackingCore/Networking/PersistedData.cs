

// using Wooga.Core.Sqlite;

namespace Match3.Scripts1.Wooga.Services.TrackingCore.Networking
{
	// Token: 0x02000474 RID: 1140
	internal class PersistedData
	{
		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x0008B1D4 File Offset: 0x000895D4
		// (set) Token: 0x060020FE RID: 8446 RVA: 0x0008B1DC File Offset: 0x000895DC
		// [PrimaryKey]
		// [AutoIncrement]
		public int Id { get; set; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x0008B1E5 File Offset: 0x000895E5
		// (set) Token: 0x06002100 RID: 8448 RVA: 0x0008B1ED File Offset: 0x000895ED
		public string Data { get; set; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x0008B1F6 File Offset: 0x000895F6
		// (set) Token: 0x06002102 RID: 8450 RVA: 0x0008B1FE File Offset: 0x000895FE
		// [MaxLength(32)]
		public string ModificationToken { get; set; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x0008B207 File Offset: 0x00089607
		// (set) Token: 0x06002104 RID: 8452 RVA: 0x0008B20F File Offset: 0x0008960F
		public bool IsConsumed { get; set; }
	}
}
