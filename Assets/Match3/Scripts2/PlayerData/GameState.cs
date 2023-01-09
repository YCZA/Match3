using System;
using System.Collections.Generic;
using Match3.Scripts1;
using Match3.Scripts1.Puzzletown.Features.DailyGifts;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Wooga.Services;
using UnityEngine;

namespace Match3.Scripts2.PlayerData
{
	// Token: 0x020007A7 RID: 1959
	[Serializable]
	public class GameState : APlayerPrefsObject<GameState>
	{
		// Token: 0x0600300C RID: 12300 RVA: 0x000E1860 File Offset: 0x000DFC60
		public GameState()
		{
			this.installTimestamp = DateTime.UtcNow.ToUnixTimeStamp();
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000E1960 File Offset: 0x000DFD60
		public static GameState FromJson(string json)
		{
			GameStateService.GameStateData gameStateData = JsonUtility.FromJson<GameStateService.GameStateData>(json);
			return (gameStateData.format_version <= 0) ? JsonUtility.FromJson<GameState>(json) : gameStateData.data;
		}

		// 游戏数据保存，只有2处调用
		public override void Save()
		{
			this.timestamp = DateTime.UtcNow.ToUnixTimeStamp();
			if (Application.isPlaying)
			{
				this.lastDeviceId = GameStateService.DeviceId;
				// Debug.Log("保存存档, 设置id: " + lastDeviceId);
			}
			base.Save();
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x000E19C0 File Offset: 0x000DFDC0
		public string LastSaveHash(string deviceId)
		{
			string result = string.Empty;
			if (this.saveHashes != null)
			{
				foreach (SaveHash saveHash in this.saveHashes)
				{
					if (saveHash.deviceId == deviceId)
					{
						result = saveHash.saveHash;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000E1A44 File Offset: 0x000DFE44
		public void AddSaveHash(string deviceId)
		{
			bool flag = false;
			if (this.saveHashes == null)
			{
				this.saveHashes = new List<SaveHash>();
			}
			foreach (SaveHash saveHash in this.saveHashes)
			{
				if (saveHash.deviceId == deviceId)
				{
					saveHash.saveHash = this.GenerateSaveHash();
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.saveHashes.Add(new SaveHash(deviceId, this.GenerateSaveHash()));
			}
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x000E1AF4 File Offset: 0x000DFEF4
		public void CleanSaveHashes()
		{
			List<SaveHash> list = new List<SaveHash>();
			foreach (SaveHash saveHash in this.saveHashes)
			{
				if (!string.IsNullOrEmpty(saveHash.deviceId))
				{
					list.Add(saveHash);
				}
			}
			this.saveHashes = list;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x000E1B70 File Offset: 0x000DFF70
		public void MigrateOldDeviceIdToCurrent()
		{
			string device_id = SBS.Authentication.GetUserContext().device_id;
			if (!string.IsNullOrEmpty(device_id) && this.lastDeviceId == device_id)
			{
				this.lastDeviceId = GameStateService.DeviceId;
				foreach (SaveHash saveHash in this.saveHashes)
				{
					if (saveHash.deviceId == device_id)
					{
						saveHash.deviceId = GameStateService.DeviceId;
					}
				}
			}
			this.CleanSaveHashes();
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000E1C20 File Offset: 0x000E0020
		private string GenerateSaveHash()
		{
			return Guid.NewGuid().ToString().Substring(0, 10);
		}

		// Token: 0x040058FF RID: 22783
		public int timestamp;

		// Token: 0x04005900 RID: 22784
		public bool isOverride;

		// Token: 0x04005901 RID: 22785
		public string lastDeviceId;

		// Token: 0x04005902 RID: 22786
		public string playerName;

		// Token: 0x04005903 RID: 22787
		public string pushToken;

		// Token: 0x04005904 RID: 22788
		public string installVersion;

		// Token: 0x04005905 RID: 22789
		public int installTimestamp;

		// Token: 0x04005906 RID: 22790
		public List<SaveHash> saveHashes = new List<SaveHash>();

		// Token: 0x04005907 RID: 22791
		public List<GlobalReplaceKey> replaceKeys = new List<GlobalReplaceKey>();

		// Token: 0x04005908 RID: 22792
		public BuildingsData buildings = new BuildingsData();

		// Token: 0x04005909 RID: 22793
		public string SharedStoredBuildings = string.Empty;

		// Token: 0x0400590A RID: 22794
		public List<BuildingsData> adventureIslands = new List<BuildingsData>();

		// Token: 0x0400590B RID: 22795
		public ResourceData resources = new ResourceData();

		// Token: 0x0400590C RID: 22796
		public ProgressionData progression = new ProgressionData();

		// Token: 0x0400590D RID: 22797
		public LivesData lives = new LivesData();

		// Token: 0x0400590E RID: 22798
		public QuestProgressCollection quests = new QuestProgressCollection();

		// Token: 0x0400590F RID: 22799
		public FacebookData.FacebookGameData fbData = new FacebookData.FacebookGameData();

		// Token: 0x04005910 RID: 22800
		public DebugData debugData = new DebugData();

		// Token: 0x04005911 RID: 22801
		public List<Transaction> transactionData = new List<Transaction>();

		// Token: 0x04005912 RID: 22802
		public AdSpinData adSpinData = new AdSpinData();

		// Token: 0x04005913 RID: 22803
		public List<SeenFlag> seenFlags = new List<SeenFlag>();

		// Token: 0x04005914 RID: 22804
		public List<SerializedLeagueState> leagueStates = new List<SerializedLeagueState>();

		// Token: 0x04005915 RID: 22805
		public ChallengeData challengeData = new ChallengeData();

		// Token: 0x04005916 RID: 22806
		public BankData bankData = new BankData();

		// Token: 0x04005917 RID: 22807
		public WeeklyEventData diveForTreasureData = new WeeklyEventData();

		// Token: 0x04005918 RID: 22808
		public WeeklyEventData pirateBreakoutData = new WeeklyEventData();

		// Token: 0x04005919 RID: 22809
		public DailyDealsData dailyDealsData = new DailyDealsData();

		// Token: 0x0400591A RID: 22810
		public DailyGiftsData dailyGiftData = new DailyGiftsData();

		// Token: 0x0400591B RID: 22811
		public SeasonalData seasonalPromoData = new SeasonalData();

		// Token: 0x0400591C RID: 22812
		public LevelOfDayModel levelOfDayData;

		// Token: 0x0400591D RID: 22813
		public bool keepGameLanguageEn;

		// Token: 0x0400591E RID: 22814
		public PromoPopupData promoPopupData;

		// Token: 0x0400591F RID: 22815
		public SaleData saleData;
	}
}
