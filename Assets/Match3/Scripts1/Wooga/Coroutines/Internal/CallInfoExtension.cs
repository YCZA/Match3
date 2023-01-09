namespace Wooga.Coroutines.Internal
{
	// Token: 0x020003CF RID: 975
	public static class CallInfoExtension
	{
		// Token: 0x06001D9B RID: 7579 RVA: 0x0007EE28 File Offset: 0x0007D228
		public static string GetCallInfo(this object obj)
		{
			CallInfoProvider callInfoProvider = obj as CallInfoProvider;
			return (callInfoProvider == null) ? null : callInfoProvider.GetCallInfo();
		}
	}
}
