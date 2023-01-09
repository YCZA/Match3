using System;
using Match3.Scripts1.Puzzletown.Config;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082C RID: 2092
	[Serializable]
	public class WeeklyEventData
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x000D8E1A File Offset: 0x000D721A
		public virtual void SetActiveConfig(EventConfigContainer eventConfig)
		{
			this.id = eventConfig.id;
			this.startTime = eventConfig.start;
			this.endTime = eventConfig.end;
			this.level = 1;
			this.set = eventConfig.config.levelSet;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000D8E58 File Offset: 0x000D7258
		public virtual void UpdateConfig(EventConfigContainer eventConfig)
		{
			this.startTime = eventConfig.start;
			this.endTime = eventConfig.end;
			this.set = eventConfig.config.levelSet;
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x000D8E84 File Offset: 0x000D7284
		public DateTime StartDateLocal
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.startTime, DateTimeKind.Utc).ToLocalTime();
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x000D8EA8 File Offset: 0x000D72A8
		public DateTime EndDateLocal
		{
			get
			{
				return Scripts1.DateTimeExtensions.FromUnixTimeStamp(this.endTime, DateTimeKind.Utc).ToLocalTime();
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003407 RID: 13319 RVA: 0x000D8ECC File Offset: 0x000D72CC
		private bool Ongoing
		{
			get
			{
				DateTime now = DateTime.Now;
				return this.StartDateLocal < now && this.EndDateLocal > now;
			}
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000D8F00 File Offset: 0x000D7300
		public int CompareProgress(WeeklyEventData other)
		{
			bool ongoing = this.Ongoing;
			bool ongoing2 = other.Ongoing;
			if (ongoing && ongoing2 && this.level != other.level)
			{
				return (this.level <= other.level) ? -1 : 1;
			}
			if (ongoing != ongoing2)
			{
				return (!ongoing) ? -1 : 1;
			}
			return 0;
		}

		// Token: 0x04005BEA RID: 23530
		public string id;

		// Token: 0x04005BEB RID: 23531
		public int startTime;

		// Token: 0x04005BEC RID: 23532
		public int endTime;

		// Token: 0x04005BED RID: 23533
		public int level;

		// Token: 0x04005BEE RID: 23534
		public int set;
	}
}
