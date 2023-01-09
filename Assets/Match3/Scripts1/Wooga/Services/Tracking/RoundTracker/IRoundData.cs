using System;

namespace Match3.Scripts1.Wooga.Services.Tracking.RoundTracker
{
	// Token: 0x0200044D RID: 1101
	public interface IRoundData
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001FE9 RID: 8169
		DateTime lastRoundSessionStart { get; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001FEA RID: 8170
		double roundDuration { get; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001FEB RID: 8171
		string roundId { get; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001FEC RID: 8172
		int level { get; }

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001FED RID: 8173
		string levelName { get; }

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001FEE RID: 8174
		int playedRoundsInTotal { get; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001FEF RID: 8175
		int lostRoundsInTotal { get; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001FF0 RID: 8176
		int wonRoundsInTotal { get; }
	}
}
