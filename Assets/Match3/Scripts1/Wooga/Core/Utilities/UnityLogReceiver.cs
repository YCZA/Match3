namespace Wooga.Core.Utilities
{
	// Token: 0x020003B8 RID: 952
	public static class UnityLogReceiver
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x0007D50F File Offset: 0x0007B90F
		public static void Log(string message, SeverityId severity)
		{
			if (severity != SeverityId.Warning)
			{
				if (severity != SeverityId.Error)
				{
					global::UnityEngine.Debug.Log(message);
				}
				else
				{
					global::UnityEngine.Debug.LogError(message);
				}
			}
			else
			{
				global::UnityEngine.Debug.LogWarning(message);
			}
		}
	}
}
