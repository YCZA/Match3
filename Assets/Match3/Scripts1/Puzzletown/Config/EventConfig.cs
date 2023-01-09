using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3.Scripts1.Puzzletown.Config
{
	// Token: 0x0200049A RID: 1178
	[Serializable]
	public class EventConfig
	{
		// Token: 0x06002164 RID: 8548 RVA: 0x0008C478 File Offset: 0x0008A878
		public static List<T> GetUpcomingEventConfigs<T>(int now, List<T> configs) where T : EventConfig
		{
			List<T> list = new List<T>();
			if (configs != null)
			{
				list.AddRange(from config in configs
				where config.IsUpcoming(now)
				select config);
			}
			if (list.Count > 0)
			{
				list.Sort(new Comparison<T>(EventConfig.SortByStartTimeAscending<T>));
			}
			return list;
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0008C4D8 File Offset: 0x0008A8D8
		public static T GetFirstUpcomingEventConfig<T>(int now, List<T> configs) where T : EventConfig
		{
			List<T> upcomingEventConfigs = EventConfig.GetUpcomingEventConfigs<T>(now, configs);
			if (upcomingEventConfigs.Count > 0)
			{
				return upcomingEventConfigs[0];
			}
			return (T)((object)null);
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0008C507 File Offset: 0x0008A907
		public static int SortByStartTimeAscending<T>(T a, T b) where T : EventConfig
		{
			if (a.start < b.start)
			{
				return -1;
			}
			if (a.start == b.start)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0008C544 File Offset: 0x0008A944
		public static int SortByEndTimeAscending<T>(T a, T b) where T : EventConfig
		{
			if (a.end < b.end)
			{
				return -1;
			}
			if (a.end == b.end)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0008C581 File Offset: 0x0008A981
		public bool IsOngoing(int now)
		{
			return this.start < now && this.end > now;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x0008C59B File Offset: 0x0008A99B
		public bool IsUpcoming(int now)
		{
			return this.start > now && this.end > now;
		}

		// Token: 0x04004C7D RID: 19581
		public string id;

		// Token: 0x04004C7E RID: 19582
		public int start;

		// Token: 0x04004C7F RID: 19583
		public int end;
	}
}
