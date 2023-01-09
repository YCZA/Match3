using System;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Services
{
	// Token: 0x0200082B RID: 2091
	public abstract class AWeeklyEventDataService : ADataService
	{
		// Token: 0x060033F0 RID: 13296 RVA: 0x000D891E File Offset: 0x000D6D1E
		protected AWeeklyEventDataService(Func<GameState> i_getState) : base(i_getState)
		{
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060033F1 RID: 13297
		public abstract string ConfigKey { get; }

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060033F2 RID: 13298
		public abstract WeeklyEventData EventData { get; }

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000D8927 File Offset: 0x000D6D27
		public DateTime StartTime
		{
			get
			{
				return this.EventData.StartDateLocal;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060033F4 RID: 13300 RVA: 0x000D8934 File Offset: 0x000D6D34
		public DateTime EndTime
		{
			get
			{
				return this.EventData.EndDateLocal;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000D8941 File Offset: 0x000D6D41
		public string EventId
		{
			get
			{
				return this.EventData.id;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x060033F6 RID: 13302 RVA: 0x000D894E File Offset: 0x000D6D4E
		// (set) Token: 0x060033F7 RID: 13303 RVA: 0x000D895B File Offset: 0x000D6D5B
		public int Level
		{
			get
			{
				return this.EventData.level;
			}
			set
			{
				this.EventData.level = value;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x060033F8 RID: 13304 RVA: 0x000D896C File Offset: 0x000D6D6C
		public EventConfigContainer ConfigContainer
		{
			get
			{
				if (this._configContainer == null && PlayerPrefs.HasKey(this.ConfigKey))
				{
					string @string = PlayerPrefs.GetString(this.ConfigKey);
					this._configContainer = new EventConfigContainer();
					this._configContainer.id = this.EventData.id;
					this._configContainer.start = this.EventData.startTime;
					this._configContainer.end = this.EventData.endTime;
					this._configContainer.config = JsonUtility.FromJson<WeeklyEventConfig>(@string);
				}
				return this._configContainer;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x060033F9 RID: 13305 RVA: 0x000D8A04 File Offset: 0x000D6E04
		public WeeklyEventType EventType
		{
			get
			{
				if (this.ConfigContainer.config == null)
				{
					return WeeklyEventType.Undefined;
				}
				return this.ConfigContainer.config.weeklyEventType;
			}
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000D8A28 File Offset: 0x000D6E28
		public int UnlockLevel()
		{
			if (this.ConfigContainer == null || this.ConfigContainer.config == null || this.ConfigContainer.config.balancing == null)
			{
				return int.MaxValue;
			}
			return this.ConfigContainer.config.balancing.unlock_level;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000D8A80 File Offset: 0x000D6E80
		public int CheckPoint()
		{
			if (this.ConfigContainer.config == null || this.ConfigContainer.config.balancing == null)
			{
				return int.MaxValue;
			}
			return this.ConfigContainer.config.balancing.check_point;
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000D8AD0 File Offset: 0x000D6ED0
		public int ChestLevel(int index)
		{
			if (this.ConfigContainer.config == null || this.ConfigContainer.config.balancing == null || this.ConfigContainer.config.balancing.rewardLevels.Length < index)
			{
				return int.MaxValue;
			}
			return this.ConfigContainer.config.balancing.rewardLevels[index];
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000D8B3C File Offset: 0x000D6F3C
		public int FirstChestLevel()
		{
			return this.ChestLevel(0);
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x000D8B45 File Offset: 0x000D6F45
		public int SecondChestLevel()
		{
			return this.ChestLevel(1);
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x000D8B4E File Offset: 0x000D6F4E
		public bool IsRewardedLevel(int level)
		{
			return this.ConfigContainer.config != null && this.ConfigContainer.config.balancing != null && this.ConfigContainer.config.balancing.IsRewardLevel(level);
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000D8B90 File Offset: 0x000D6F90
		public Materials GetRewards(int level)
		{
			Materials result = new Materials();
			if (level == this.FirstChestLevel())
			{
				result = this.ConfigContainer.config.FirstRewards;
			}
			else if (level == this.SecondChestLevel())
			{
				result = this.ConfigContainer.config.SecondRewards;
			}
			else if (level == this.ChestLevel(2))
			{
				result = this.ConfigContainer.config.ThirdRewards;
			}
			return result;
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000D8C06 File Offset: 0x000D7006
		public virtual string GetTrophyForLevel(int level)
		{
			return null;
		}

		// Token: 0x04005BE9 RID: 23529
		private EventConfigContainer _configContainer;
	}
}
