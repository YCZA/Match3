using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Puzzletown.Config;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Puzzletown.Match3.Scoring;
using Match3.Scripts1.Puzzletown.Services;

namespace Match3.Scripts1.PTTracking
{
	// Token: 0x02000818 RID: 2072
	public static class LevelConfig_Tracking
	{
		// Token: 0x0600334A RID: 13130 RVA: 0x000F3330 File Offset: 0x000F1730
		public static void PopulateTracking(this LevelConfig config, Match3Score score, SeasonService seasonService, Dictionary<string, object> data)
		{
			data["mg"] = config.data.moves;
			if (config.LevelCollectionConfig.isABLevel)
			{
				data["lvl_name"] = "AB_" + config.Name;
				data["lvl_number"] = "AB_" + config.LevelCollectionConfig.level;
			}
			else
			{
				data["lvl_name"] = config.Name;
				data["lvl_number"] = config.LevelCollectionConfig.level;
			}
			data["r1_name"] = "diamonds";
			data["r1_amount"] = config.Diamonds;
			int num = (from reward in score.Rewards
			where reward.type == "coins"
			select reward).Sum((MaterialAmount reward) => reward.amount);
			data["r3_name"] = "coins";
			data["r3_amount"] = num;
			SeasonConfig activeSeason = seasonService.GetActiveSeason();
			if (config.seasonalRewards.amount > 0 && activeSeason != null)
			{
				data["sea_cur"] = activeSeason.Primary;
				data["sea_ev_id"] = activeSeason.id;
				data["sea_cur_amt"] = config.seasonalRewards.amount;
			}
			LevelConfig_Tracking.objData objData = new LevelConfig_Tracking.objData
			{
				objId = 1
			};
			objData = LevelConfig_Tracking.PopulateHiddenItems(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateDropItems(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateClimber(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateDirtAndTreasure(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateWaterAllFields(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateResistantBlocker(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateChameleon(config, score, data, objData);
			objData = LevelConfig_Tracking.PopulateRecipe(config, score, data, objData);
			data["mobj"] = objData.objId - 1;
			data["obj"] = objData.objCompletedCounter;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000F3558 File Offset: 0x000F1958
		private static LevelConfig_Tracking.objData PopulateHiddenItems(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			if (!config.HasHiddenItems)
			{
				return objData;
			}
			int num = score.AllCollected[config.data.hiddenItemName];
			int numHiddenItems = config.layout.NumHiddenItems;
			data["obj" + objData.objId + "_name"] = "hiddenitem";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_details"] = config.data.hiddenItemName;
			data["obj" + objData.objId + "_total"] = numHiddenItems.ToString();
			if (num >= numHiddenItems)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000F366C File Offset: 0x000F1A6C
		private static LevelConfig_Tracking.objData PopulateDropItems(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			int dropItemsCount = config.data.DropItemsCount;
			if (dropItemsCount <= 0)
			{
				return objData;
			}
			int num = score.AllCollected["droppable"];
			int dropItemsCount2 = config.data.DropItemsCount;
			data["obj" + objData.objId + "_name"] = "droppable";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_details"] = "droppable";
			data["obj" + objData.objId + "_total"] = dropItemsCount2.ToString();
			if (num >= dropItemsCount2)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000F377C File Offset: 0x000F1B7C
		private static LevelConfig_Tracking.objData PopulateClimber(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			int climberCount = config.data.ClimberCount;
			if (climberCount == 0)
			{
				return objData;
			}
			int num = score.AllCollected["climber"];
			data["obj" + objData.objId + "_name"] = "climber";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_total"] = climberCount.ToString();
			if (num >= climberCount)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000F3858 File Offset: 0x000F1C58
		private static LevelConfig_Tracking.objData PopulateDirtAndTreasure(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			int numTreasure = config.layout.NumTreasure;
			if (numTreasure == 0)
			{
				return objData;
			}
			int num = score.AllCollected["treasure"];
			data["obj" + objData.objId + "_name"] = "treasure";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_total"] = numTreasure.ToString();
			if (num >= numTreasure)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000F3934 File Offset: 0x000F1D34
		private static LevelConfig_Tracking.objData PopulateWaterAllFields(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			if (!config.layout.HasWaterAndUnwateredFields)
			{
				return objData;
			}
			int numUnwateredFields = config.layout.NumUnwateredFields;
			int num = score.AllCollected["water"];
			data["obj" + objData.objId + "_name"] = "water";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_total"] = numUnwateredFields.ToString();
			if (num >= numUnwateredFields)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000F3A1C File Offset: 0x000F1E1C
		private static LevelConfig_Tracking.objData PopulateResistantBlocker(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			if (!config.layout.HasResistantBlocker)
			{
				return objData;
			}
			int numResistantBlocker = config.layout.NumResistantBlocker;
			int num = score.AllCollected["resistant_blocker"];
			data["obj" + objData.objId + "_name"] = "resistant_blocker";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_total"] = numResistantBlocker.ToString();
			if (num >= numResistantBlocker)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000F3B00 File Offset: 0x000F1F00
		private static LevelConfig_Tracking.objData PopulateChameleon(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			int chameleonCount = config.data.ChameleonCount;
			if (chameleonCount == 0)
			{
				return objData;
			}
			int num = score.AllCollected["chameleon"];
			data["obj" + objData.objId + "_name"] = "chameleon";
			data["obj" + objData.objId + "_done"] = num.ToString();
			data["obj" + objData.objId + "_total"] = chameleonCount.ToString();
			if (num >= chameleonCount)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000F3BDC File Offset: 0x000F1FDC
		private static LevelConfig_Tracking.objData PopulateRecipe(LevelConfig config, Match3Score score, IDictionary<string, object> data, LevelConfig_Tracking.objData objData)
		{
			MaterialAmount[] array = (from item in config.data.objectives
			where item.type != "droppable" && item.type != "monkey" && item.type != "climber" && item.type != "treasure" && item.type != "resistant_blocker" && item.type != "water"
			select item).ToArray<MaterialAmount>();
			if (array.Length <= 0)
			{
				return objData;
			}
			string value = string.Join("_", (from item in array
			select score.AllCollected[item.type].ToString().PadLeft(3, '0')).ToArray<string>());
			data["obj" + objData.objId + "_name"] = "recipe";
			data["obj" + objData.objId + "_done"] = value;
			data["obj" + objData.objId + "_details"] = string.Join("_", (from e in array
			select e.type.ToUpper()[0].ToString()).ToArray<string>());
			data["obj" + objData.objId + "_total"] = string.Join("_", (from e in array
			select e.amount.ToString().PadLeft(3, '0')).ToArray<string>());
			bool flag = array.Aggregate(true, (bool current, MaterialAmount cRecipeItem) => current & score.AllCollected[cRecipeItem.type] >= cRecipeItem.amount);
			if (flag)
			{
				objData.objCompletedCounter++;
			}
			objData.objId++;
			return objData;
		}

		// Token: 0x02000819 RID: 2073
		private struct objData
		{
			// Token: 0x04005B81 RID: 23425
			public int objId;

			// Token: 0x04005B82 RID: 23426
			public int objCompletedCounter;
		}
	}
}
