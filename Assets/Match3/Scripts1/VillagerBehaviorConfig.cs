using System;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
namespace Match3.Scripts1
{
	[Serializable]
	public class VillagerBehaviorConfig
	{
		// Token: 0x060036B7 RID: 14007 RVA: 0x0010AAC4 File Offset: 0x00108EC4
		public VillagerDecoData FindVillagerDeco(VillagerData villager, BuildingConfig building, int currentLevel)
		{
			VillagerDecoData[] array = Array.FindAll<VillagerDecoData>(this.villager_deco, (VillagerDecoData vd) => VillagerBehaviorConfig.Match(vd, villager, building, currentLevel));
			return (!array.IsNullOrEmptyCollection()) ? array.RandomElement(false) : null;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x0010AB1C File Offset: 0x00108F1C
		public VillagerVillagerData[] FindVillagerVillager(string a, string b, int currentLevel)
		{
			VillagerVillagerData[] array = Array.FindAll<VillagerVillagerData>(this.villager_villager, (VillagerVillagerData vv) => VillagerBehaviorConfig.Match(vv, a, b, currentLevel));
			if (array.IsNullOrEmptyCollection())
			{
				return null;
			}
			string id = array.RandomElement(false).dialogue_id;
			return Array.FindAll<VillagerVillagerData>(this.villager_villager, (VillagerVillagerData vv) => vv.dialogue_id == id);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x0010AB94 File Offset: 0x00108F94
		public VillagerPlayerData[] FindVillagerPlayer(VillagerPlayerTrigger trigger, int currentLevel)
		{
			VillagerPlayerData[] array = Array.FindAll<VillagerPlayerData>(this.villager_player, (VillagerPlayerData vp) => VillagerBehaviorConfig.Match(vp, trigger, null, currentLevel));
			if (array.IsNullOrEmptyCollection())
			{
				return null;
			}
			string id = array.RandomElement(false).interaction_id;
			return Array.FindAll<VillagerPlayerData>(this.villager_player, (VillagerPlayerData vp) => vp.interaction_id == id);
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x0010AC04 File Offset: 0x00109004
		public VillagerPlayerData[] FindVillagerPlayer(VillagerPlayerTrigger trigger, string villager, int currentLevel)
		{
			string lowercase = villager.ToLower();
			VillagerPlayerData[] array = Array.FindAll<VillagerPlayerData>(this.villager_player, (VillagerPlayerData vp) => VillagerBehaviorConfig.Match(vp, trigger, lowercase, currentLevel));
			if (array.IsNullOrEmptyCollection())
			{
				return null;
			}
			string id = array.RandomElement(false).interaction_id;
			return Array.FindAll<VillagerPlayerData>(this.villager_player, (VillagerPlayerData vp) => vp.interaction_id == id);
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x0010AC7F File Offset: 0x0010907F
		private static bool Match(VillagerDecoData vd, VillagerData villager, BuildingConfig deco, int currentLevel)
		{
			return vd.lvl_start <= currentLevel && (vd.lvl_end <= 0 || vd.lvl_end >= currentLevel) && vd.VillagerLikesBuilding(villager, deco);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0010ACB4 File Offset: 0x001090B4
		private static bool Match(VillagerPlayerData vp, VillagerPlayerTrigger trigger, string lowercase, int currentLevel)
		{
			return vp.lvl_start <= currentLevel && (vp.lvl_end <= 0 || vp.lvl_end >= currentLevel) && vp.Trigger == trigger && (string.IsNullOrEmpty(lowercase) || string.IsNullOrEmpty(vp.character) || vp.character == lowercase || vp.character == "any");
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0010AD38 File Offset: 0x00109138
		private static bool Match(VillagerVillagerData vv, string a, string b, int currentLevel)
		{
			return vv.lvl_start <= currentLevel && (vv.lvl_end <= 0 || vv.lvl_end >= currentLevel) && (vv.VillagerLikesVillager(a, b) || vv.VillagerLikesVillager(b, a));
		}

		// Token: 0x04005EEB RID: 24299
		[SerializeField]
		private VillagerDecoData[] villager_deco;

		// Token: 0x04005EEC RID: 24300
		[SerializeField]
		private VillagerVillagerData[] villager_villager;

		// Token: 0x04005EED RID: 24301
		[SerializeField]
		private VillagerPlayerData[] villager_player;
	}
}
