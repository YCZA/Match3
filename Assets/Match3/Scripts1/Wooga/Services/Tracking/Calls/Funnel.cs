using System.Collections.Generic;

namespace Match3.Scripts1.Wooga.Services.Tracking.Calls
{
	// Token: 0x02000438 RID: 1080
	public static class Funnel
	{
		// Token: 0x06001FA3 RID: 8099 RVA: 0x00085198 File Offset: 0x00083598
		public static void Step(uint stepNumber, string stepDescription)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["step_description"] = stepDescription;
			dictionary["step_number"] = stepNumber;
			Tracking.Track("funnel", dictionary);
		}
	}
}
