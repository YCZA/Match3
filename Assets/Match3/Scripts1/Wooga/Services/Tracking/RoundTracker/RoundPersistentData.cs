using System;
using System.Collections.Generic;
using Wooga.Core.Utilities;

namespace Match3.Scripts1.Wooga.Services.Tracking.RoundTracker
{
	// Token: 0x0200044E RID: 1102
	public class RoundPersistentData : IRoundData
	{
		// Token: 0x06001FF1 RID: 8177 RVA: 0x000860E9 File Offset: 0x000844E9
		public RoundPersistentData()
		{
			this.lastRoundSessionStart = Time.UtcNow();
			this.levelData = new Dictionary<string, LevelData>();
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x00086107 File Offset: 0x00084507
		// (set) Token: 0x06001FF3 RID: 8179 RVA: 0x0008610F File Offset: 0x0008450F
		public DateTime lastRoundSessionStart { get; set; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x00086118 File Offset: 0x00084518
		// (set) Token: 0x06001FF5 RID: 8181 RVA: 0x00086120 File Offset: 0x00084520
		public double roundDuration { get; set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x00086129 File Offset: 0x00084529
		// (set) Token: 0x06001FF7 RID: 8183 RVA: 0x00086131 File Offset: 0x00084531
		public string roundId { get; set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x0008613A File Offset: 0x0008453A
		// (set) Token: 0x06001FF9 RID: 8185 RVA: 0x00086142 File Offset: 0x00084542
		public int level { get; set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x0008614B File Offset: 0x0008454B
		// (set) Token: 0x06001FFB RID: 8187 RVA: 0x00086153 File Offset: 0x00084553
		public string levelName { get; set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x0008615C File Offset: 0x0008455C
		// (set) Token: 0x06001FFD RID: 8189 RVA: 0x00086164 File Offset: 0x00084564
		public int playedRoundsInTotal { get; set; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0008616D File Offset: 0x0008456D
		// (set) Token: 0x06001FFF RID: 8191 RVA: 0x00086175 File Offset: 0x00084575
		public int lostRoundsInTotal { get; set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0008617E File Offset: 0x0008457E
		// (set) Token: 0x06002001 RID: 8193 RVA: 0x00086186 File Offset: 0x00084586
		public int wonRoundsInTotal { get; set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x0008618F File Offset: 0x0008458F
		// (set) Token: 0x06002003 RID: 8195 RVA: 0x00086197 File Offset: 0x00084597
		public Dictionary<string, LevelData> levelData { get; set; }
	}
}
