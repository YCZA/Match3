namespace Match3.Scripts1.Wooga.Services.AssetBundleManager
{
	// Token: 0x02000305 RID: 773
	public static class DownloadMonitorExtension
	{
		// Token: 0x0600184A RID: 6218 RVA: 0x0006F2B5 File Offset: 0x0006D6B5
		public static bool HasProgressMonitor(this DownloadMonitor monitor)
		{
			return monitor != null && monitor.OnProgress != null;
		}
	}
}
