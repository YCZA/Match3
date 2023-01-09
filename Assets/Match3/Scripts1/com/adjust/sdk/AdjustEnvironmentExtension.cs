namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x02000016 RID: 22
	public static class AdjustEnvironmentExtension
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00005FC7 File Offset: 0x000043C7
		public static string ToLowercaseString(this AdjustEnvironment adjustEnvironment)
		{
			if (adjustEnvironment == AdjustEnvironment.Sandbox)
			{
				return "sandbox";
			}
			if (adjustEnvironment != AdjustEnvironment.Production)
			{
				return "unknown";
			}
			return "production";
		}
	}
}
