

// using Wooga.Core.Sqlite;

namespace Wooga.Core.Utilities.RequestCache
{
	// Token: 0x020003B0 RID: 944
	// [Table("Requests")]
	internal class RequestData
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x0007CA60 File Offset: 0x0007AE60
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x0007CA68 File Offset: 0x0007AE68
		public string Resource { get; set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x0007CA71 File Offset: 0x0007AE71
		// (set) Token: 0x06001C7B RID: 7291 RVA: 0x0007CA79 File Offset: 0x0007AE79
		public string Etag { get; set; }

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x0007CA82 File Offset: 0x0007AE82
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x0007CA8A File Offset: 0x0007AE8A
		// [PrimaryKey]
		public string Path { get; set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0007CA93 File Offset: 0x0007AE93
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x0007CA9B File Offset: 0x0007AE9B
		public long LastUsed { get; set; }
	}
}
