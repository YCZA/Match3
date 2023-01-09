using Match3.Scripts1.Wooga.Core.Data;

namespace Match3.Scripts1.Wooga.Services.KeyValueStore
{
	// Token: 0x0200040F RID: 1039
	public class SbsMergeOperation
	{
		// Token: 0x06001EB2 RID: 7858 RVA: 0x00082037 File Offset: 0x00080437
		internal SbsMergeOperation(ISbsData mine, ISbsData theirs, SbsMergeOperation.SbsMergeOperationResultHandler resultHandler, SbsMergeOperation.SbsMergeOperationCancellationHandler cancellationHandler)
		{
			this._resultHandler = resultHandler;
			this._cancellationHandler = cancellationHandler;
			this.Mine = mine;
			this.Theirs = theirs;
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0008205C File Offset: 0x0008045C
		// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x00082064 File Offset: 0x00080464
		public ISbsData Theirs { get; private set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0008206D File Offset: 0x0008046D
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x00082075 File Offset: 0x00080475
		public ISbsData Mine { get; private set; }

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0008207E File Offset: 0x0008047E
		public void Resolve(ISbsData mergedData)
		{
			this._resultHandler(mergedData);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x0008208C File Offset: 0x0008048C
		public void Cancel()
		{
			this._cancellationHandler();
		}

		// Token: 0x04004A8C RID: 19084
		private readonly SbsMergeOperation.SbsMergeOperationResultHandler _resultHandler;

		// Token: 0x04004A8D RID: 19085
		private readonly SbsMergeOperation.SbsMergeOperationCancellationHandler _cancellationHandler;

		// Token: 0x02000410 RID: 1040
		// (Invoke) Token: 0x06001EBA RID: 7866
		internal delegate void SbsMergeOperationResultHandler(ISbsData mergedSbsData);

		// Token: 0x02000411 RID: 1041
		// (Invoke) Token: 0x06001EBE RID: 7870
		internal delegate void SbsMergeOperationCancellationHandler();
	}
}
