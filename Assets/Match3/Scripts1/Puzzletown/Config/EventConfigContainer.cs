using System;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x02000970 RID: 2416
	[Serializable]
	public class EventConfigContainer : EventConfig
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x00123AC4 File Offset: 0x00121EC4
		public DateTime EndDateLocal
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.end, DateTimeKind.Utc).ToLocalTime();
			}
		}

		// Token: 0x040062CD RID: 25293
		public WeeklyEventConfig config;
	}
}
