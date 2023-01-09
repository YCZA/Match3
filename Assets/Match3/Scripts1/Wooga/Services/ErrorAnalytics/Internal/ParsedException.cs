using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.ErrorAnalytics.Internal
{
	// Token: 0x020003F9 RID: 1017
	public class ParsedException
	{
		// Token: 0x04004A3D RID: 19005
		public string errorClass;

		// Token: 0x04004A3E RID: 19006
		public string errorMessage;

		// Token: 0x04004A3F RID: 19007
		public string stackTrace;

		// Token: 0x04004A40 RID: 19008
		public string uuid;

		// Token: 0x04004A41 RID: 19009
		public object data;

		// Token: 0x04004A42 RID: 19010
		public Exception exception;

		// Token: 0x04004A43 RID: 19011
		public ErrorAnalytics.LogSeverity severity;

		// Token: 0x04004A44 RID: 19012
		public Dictionary<string, object> meta = new Dictionary<string, object>();

		// Token: 0x04004A45 RID: 19013
		internal List<ParsingUtility.StackTraceElement> elements;
	}
}
