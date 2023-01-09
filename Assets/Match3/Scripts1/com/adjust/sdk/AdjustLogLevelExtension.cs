namespace Match3.Scripts1.com.adjust.sdk
{
	// Token: 0x0200001B RID: 27
	public static class AdjustLogLevelExtension
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00006504 File Offset: 0x00004904
		public static string ToLowercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "verbose";
			case AdjustLogLevel.Debug:
				return "debug";
			case AdjustLogLevel.Info:
				return "info";
			case AdjustLogLevel.Warn:
				return "warn";
			case AdjustLogLevel.Error:
				return "error";
			case AdjustLogLevel.Assert:
				return "assert";
			case AdjustLogLevel.Suppress:
				return "suppress";
			default:
				return "unknown";
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000656C File Offset: 0x0000496C
		public static string ToUppercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "VERBOSE";
			case AdjustLogLevel.Debug:
				return "DEBUG";
			case AdjustLogLevel.Info:
				return "INFO";
			case AdjustLogLevel.Warn:
				return "WARN";
			case AdjustLogLevel.Error:
				return "ERROR";
			case AdjustLogLevel.Assert:
				return "ASSERT";
			case AdjustLogLevel.Suppress:
				return "SUPPRESS";
			default:
				return "UNKNOWN";
			}
		}
	}
}
