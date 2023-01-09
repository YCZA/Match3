using System;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using UnityEngine;

// Token: 0x020008E7 RID: 2279
namespace Match3.Scripts1
{
	[Serializable]
	public class BuildingConfig : ABuildingConfig
	{
		// Token: 0x06003756 RID: 14166 RVA: 0x0010DD11 File Offset: 0x0010C111
		public static BuildingConfig FromJsonString(string jsonString)
		{
			return JsonUtility.FromJson<BuildingConfig>(jsonString);
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x0010DD19 File Offset: 0x0010C119
		// (set) Token: 0x06003758 RID: 14168 RVA: 0x0010DD21 File Offset: 0x0010C121
		public string TrackingDetail
		{
			get
			{
				return this.trackingDetail;
			}
			set
			{
				this.trackingDetail = value;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x0010DD2A File Offset: 0x0010C12A
		public string Asset
		{
			get
			{
				return (!string.IsNullOrEmpty(this.art_override)) ? this.art_override : this.name;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x0010DD4D File Offset: 0x0010C14D
		public string Name_LocaleKey
		{
			get
			{
				this._cachedName = (this._cachedName ?? ("building.name." + this.name));
				return this._cachedName;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x0010DD78 File Offset: 0x0010C178
		public string ActionTextKey
		{
			get
			{
				this._cachedActionKey = (this._cachedActionKey ?? ("building.action." + this.name));
				return this._cachedActionKey;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x0010DDA3 File Offset: 0x0010C1A3
		public bool CanHarvest
		{
			get
			{
				return this.harvest_timer > 0 && this.harvest_maximum > 0;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x0010DDBD File Offset: 0x0010C1BD
		public MaterialAmount Harmony
		{
			get
			{
				return new MaterialAmount("harmony", this.harmony, MaterialAmountUsage.Undefined, 0);
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x0010DDD4 File Offset: 0x0010C1D4
		public MaterialAmount Harvest
		{
			get
			{
				return new MaterialAmount(this.harvest_resource, (this.harvest_maximum <= 0) ? 0 : 1, MaterialAmountUsage.Undefined, 0)
				{
					MaxAmount = this.harvest_maximum,
					Period = this.harvest_timer
				};
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x0010DE1D File Offset: 0x0010C21D
		public MaterialAmount UnlockResource
		{
			get
			{
				return new MaterialAmount(this.unlock_resource, this.unlock_resource_amount, MaterialAmountUsage.Undefined, 0);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x0010DE32 File Offset: 0x0010C232
		public MaterialAmount SeasonCurrency
		{
			get
			{
				return new MaterialAmount("season_currency", this.season_currency, MaterialAmountUsage.Undefined, 0);
			}
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0010DE46 File Offset: 0x0010C246
		public bool IsSeasonGrandPrice()
		{
			return this.costs.Any((MaterialAmount c) => c.type == "season_currency");
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0010DE70 File Offset: 0x0010C270
		public bool IsSeasonal()
		{
			return this.type == 1;
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x0010DE7B File Offset: 0x0010C27B
		public bool IsAlphabet()
		{
			return this.type == 512;
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x0010DE8A File Offset: 0x0010C28A
		public bool IsFriendDeco()
		{
			return this.type == 1024;
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x0010DE99 File Offset: 0x0010C299
		public bool IsRubble()
		{
			return this.rubble_id > 0;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x0010DEA4 File Offset: 0x0010C2A4
		public bool IsTrophy()
		{
			return this.type == 128;
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0010DEB4 File Offset: 0x0010C2B4
		public string GetPlaceholderName()
		{
			if (this.size == 1 && (this.type & 24) > 0)
			{
				return "iso_road_stone";
			}
			int num = 1;
			if (this.size == 2 || this.size == 3)
			{
				num = 2;
			}
			if (this.size > 3)
			{
				num = 3;
			}
			return string.Format("iso_under_construction{0}", num);
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0010DF1C File Offset: 0x0010C31C
		public string GetGenericDestroyedPrefabName()
		{
			int num = Math.Min(this.size, BuildingResourceServiceRoot.MAX_PLACEHOLDER_SIZE);
			if (this.name.Contains("nature") || this.name.Contains("water_river"))
			{
				return string.Format("iso_nature_destroyed_{0}x{0}", num);
			}
			if ((this.type & 24) > 0)
			{
				return "iso_connectable_destroyed";
			}
			return string.Format("iso_deco_destroyed_{0}x{0}", num);
		}

		// Token: 0x04005F9F RID: 24479
		private string trackingDetail;

		// Token: 0x04005FA0 RID: 24480
		public MaterialAmount[] costs = new MaterialAmount[0];

		// Token: 0x04005FA1 RID: 24481
		private string _cachedName;

		// Token: 0x04005FA2 RID: 24482
		private string _cachedActionKey;
	}
}
