using System;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;

// Token: 0x0200081C RID: 2076
namespace Match3.Scripts1
{
	public class PTSingleRoundTracker
	{
		// Token: 0x0600336A RID: 13162 RVA: 0x000F4420 File Offset: 0x000F2820
		public PTSingleRoundTracker(DateTime roundStarted)
		{
			this.RoundStarted = roundStarted;
			this.RoundId = Guid.NewGuid().ToString("N");
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000F4452 File Offset: 0x000F2852
		// (set) Token: 0x0600336C RID: 13164 RVA: 0x000F445A File Offset: 0x000F285A
		public DateTime RoundStarted { get; protected set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000F4463 File Offset: 0x000F2863
		// (set) Token: 0x0600336E RID: 13166 RVA: 0x000F446B File Offset: 0x000F286B
		public DateTime RoundCompleted { get; protected set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x000F4474 File Offset: 0x000F2874
		// (set) Token: 0x06003370 RID: 13168 RVA: 0x000F447C File Offset: 0x000F287C
		public string RoundId { get; protected set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06003371 RID: 13169 RVA: 0x000F4485 File Offset: 0x000F2885
		// (set) Token: 0x06003372 RID: 13170 RVA: 0x000F448D File Offset: 0x000F288D
		public bool DidWin { get; protected set; }

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06003373 RID: 13171 RVA: 0x000F4496 File Offset: 0x000F2896
		// (set) Token: 0x06003374 RID: 13172 RVA: 0x000F449E File Offset: 0x000F289E
		public TrackingService.RoundOutcome Outcome { get; protected set; }

		// Token: 0x06003375 RID: 13173 RVA: 0x000F44A7 File Offset: 0x000F28A7
		public void SetCompleted(DateTime roundCompleted, TrackingService.RoundOutcome outcome)
		{
			this.RoundCompleted = roundCompleted;
			this.Outcome = outcome;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x000F44B8 File Offset: 0x000F28B8
		private int RoundTime
		{
			get
			{
				return (int)(this.RoundCompleted - this.RoundStarted).TotalSeconds;
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000F44E0 File Offset: 0x000F28E0
		public void PopulateTracking(Dictionary<string, object> newEvent)
		{
			newEvent["gd"] = this.RoundTime;
			newEvent["lvl_outcome"] = Convert.ToInt32(this.Outcome);
			newEvent["round_id"] = this.RoundId;
		}
	}
}
