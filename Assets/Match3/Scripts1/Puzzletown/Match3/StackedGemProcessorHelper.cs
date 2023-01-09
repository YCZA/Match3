using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000622 RID: 1570
	public static class StackedGemProcessorHelper
	{
		// Token: 0x060027F9 RID: 10233 RVA: 0x000B1AB8 File Offset: 0x000AFEB8
		public static Group ReplaceGems(Gem gem, int needAmount, Fields fields)
		{
			Group gemsToReplace = StackedGemProcessorHelper.GetGemsToReplace(needAmount, gem.color, fields);
			StackedGemProcessorHelper.SwitchColorOfGems(gemsToReplace, gem.color, fields);
			// 不够，则往空位上放色块(暂时不往空位上放色块)
			// if (gemsToReplace.Count < needAmount)
			// {
			// 	int poorAmount = needAmount - gemsToReplace.Count;
			// 	// 得到空的位置
			// 	List<IntVector2> positionToSpawn = StackedGemProcessorHelper.GetPositionToSpawn(poorAmount, fields);
			// 	Gem newGem = new Gem(gem.color);
			// 	foreach (IntVector2 vec in positionToSpawn)
			// 	{
			// 		fields[vec].AssignGem(newGem);
			// 		gemsToReplace.Add(fields[vec].gem);
			// 	}
			// }
			return gemsToReplace;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000B1B74 File Offset: 0x000AFF74
		private static Group GetGemsToReplace(int amount, GemColor excludedColor, Fields fields)
		{
			Group movableMatchableNormalGems = StackedGemProcessorHelper.GetMovableMatchableNormalGems(excludedColor, fields);
			// 随机移除，直到只剩下amount个宝石
			// Debug.LogError("GetGemsToReplace: " + movableMatchableNormalGems.Count);
			while (movableMatchableNormalGems.Count > amount)
			{
				int index = RandomHelper.Next(0, movableMatchableNormalGems.Count);
				movableMatchableNormalGems.RemoveAt(index);
			}
			return movableMatchableNormalGems;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000B1BB0 File Offset: 0x000AFFB0
		private static Group GetMovableMatchableNormalGems(GemColor excludedColor, Fields fields)
		{
			Group group = new Group();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					Field field = fields[i, j];
					if (field.gem.IsMatchable && field.CanMove && field.gem.type == GemType.Undefined && field.gem.modifier == GemModifier.Undefined)
					{
						GemColor color = field.gem.color;
						if (color != excludedColor && color != GemColor.Undefined)
						{
							group.Add(field.gem);
						}
					}
				}
			}
			return group;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000B1C60 File Offset: 0x000B0060
		private static void SwitchColorOfGems(Group group, GemColor stackedColor, Fields fields)
		{
			for (int i = 0; i < group.Count; i++)
			{
				fields[group[i].position].gem.color = stackedColor;
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000B1CA4 File Offset: 0x000B00A4
		private static List<IntVector2> GetPositionToSpawn(int amount, Fields fields)
		{
			List<IntVector2> freeFieldPositions = StackedGemProcessorHelper.GetFreeFieldPositions(fields);
			while (freeFieldPositions.Count > amount)
			{
				int index = RandomHelper.Next(0, freeFieldPositions.Count);
				freeFieldPositions.RemoveAt(index);
			}
			return freeFieldPositions;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000B1CE0 File Offset: 0x000B00E0
		private static List<IntVector2> GetFreeFieldPositions(Fields fields)
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					Field field = fields[i, j];
					if (field.isOn && !field.isWindow && !field.HasGem && field.CanMove && !field.IsBlocked)
					{
						list.Add(field.gridPosition);
					}
				}
			}
			return list;
		}
	}
}
