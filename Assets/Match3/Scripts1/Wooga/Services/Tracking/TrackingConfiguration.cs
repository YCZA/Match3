using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Services.Tracking.Parameters;

namespace Match3.Scripts1.Wooga.Services.Tracking
{
	// Token: 0x02000469 RID: 1129
	public class TrackingConfiguration
	{
		// Token: 0x04004B93 RID: 19347
		public SessionTracker.SessionTracker.SessionStartCallback onSessionStart;

		// Token: 0x04004B94 RID: 19348
		public Action<Dictionary<string, object>> onSessionEnd;

		// Token: 0x04004B95 RID: 19349
		protected internal List<IParameterProvider> parameterProviders = new List<IParameterProvider>();
	}
}
