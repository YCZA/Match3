using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000572 RID: 1394
	public static class ObjectiveGenerator
	{
		// Token: 0x06002491 RID: 9361 RVA: 0x000A2E14 File Offset: 0x000A1214
		public static void GenerateColorObjective(Map<Field> fields, LevelData data)
		{
			List<MaterialAmount> list = new List<MaterialAmount>();
			List<GemColor> list2 = new List<GemColor>();
			List<GemColor> colorsFromAmounts = LevelGenerator.GetColorsFromAmounts(data.gems);
			int amount = RandomHelper.Next(1, 4);
			list2 = LevelGenerator.GetRandomDifferentColors(amount, colorsFromAmounts);
			foreach (GemColor color in list2)
			{
				LevelGenerator.AddColorAmountToList(list, color);
			}
			data.objectives = list.ToArray();
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000A2EA4 File Offset: 0x000A12A4
		public static void GenerateAndPreplaceHiddenItems(Map<Field> fields, LevelData data)
		{
			ModifierGenerator.PlaceTilesOnAllFields(fields, 1);
			ObjectiveGenerator.hiddenItemBrushBig.PaintField(ObjectiveGenerator.GetRandomFieldForHiddenItem(fields, ObjectiveGenerator.BIG), fields as Fields);
			ObjectiveGenerator.hiddenItemBrushMedium.PaintField(ObjectiveGenerator.GetRandomFieldForHiddenItem(fields, ObjectiveGenerator.MEDIUM), fields as Fields);
			ObjectiveGenerator.hiddenItemBrushSmall.PaintField(ObjectiveGenerator.GetRandomFieldForHiddenItem(fields, ObjectiveGenerator.SMALL), fields as Fields);
			HiddenItemInfoDict hiddenItems = HiddenItemProcessor.GetHiddenItems(fields as Fields);
			int count = hiddenItems.Count;
			if (string.IsNullOrEmpty(data.hiddenItemName))
			{
				data.hiddenItemName = "hiddenitem";
			}
			MaterialAmount materialAmount = new MaterialAmount(data.hiddenItemName, count, MaterialAmountUsage.Undefined, 0);
			data.objectives = new MaterialAmount[]
			{
				materialAmount
			};
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000A2F64 File Offset: 0x000A1364
		public static void GenerateAndPreplaceDropItems(Map<Field> fields, LevelData data, LevelGenerationGlobals settings, bool horizontalSymmetry = false)
		{
			IntVector2 randomPosForDropItem = ObjectiveGenerator.GetRandomPosForDropItem(fields);
			int num = 1;
			ObjectiveGenerator.SetDropSpawnAndAssignDroppable(fields[randomPosForDropItem]);
			if (horizontalSymmetry)
			{
				IntVector2 vec = LayoutGenerator.HorizontallyReflectedPos(randomPosForDropItem, fields.size);
				ObjectiveGenerator.SetDropSpawnAndAssignDroppable(fields[vec]);
				num++;
			}
			SpawnRatio spawnRatio = new SpawnRatio();
			spawnRatio.gemColor = GemColor.Droppable;
			spawnRatio.minSpawn = 1;
			spawnRatio.maxSpawn = num;
			int dropItemInitSpawnRatioMin = settings.DropItemInitSpawnRatioMin;
			int excludedMaxValue = settings.DropItemInitSpawnRatioMax + 1;
			spawnRatio.probability = RandomHelper.Next(dropItemInitSpawnRatioMin, excludedMaxValue);
			ModifierGenerator.PlaceDropExitAtBottomPosition(fields);
			MaterialAmount materialAmount = new MaterialAmount("droppable", num, MaterialAmountUsage.Undefined, 0);
			data.objectives = new MaterialAmount[]
			{
				materialAmount
			};
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000A3014 File Offset: 0x000A1414
		private static Field GetRandomFieldForHiddenItem(Map<Field> fields, int itemSize)
		{
			int size = fields.size;
			Field result = fields[size - 1, size - 1];
			List<Field> list = new List<Field>();
			int num = size - itemSize;
			int num2 = itemSize - 1;
			for (int i = 0; i <= num; i++)
			{
				for (int j = num2; j < size; j++)
				{
					Field field = fields[i, j];
					if (field.isOn && field.hiddenItemId == 0)
					{
						IEnumerable block = (fields as Fields).GetBlock(field.gridPosition, itemSize, itemSize);
						bool flag = true;
						IEnumerator enumerator = block.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Field field2 = (Field)obj;
								if (field2 == null || !field2.isOn || field2.hiddenItemId > 0)
								{
									flag = false;
									break;
								}
							}
						}
						finally
						{
							IDisposable disposable;
							if ((disposable = (enumerator as IDisposable)) != null)
							{
								disposable.Dispose();
							}
						}
						if (flag)
						{
							list.Add(field);
						}
					}
				}
			}
			if (list.Count != 0)
			{
				int index = RandomHelper.Next(list.Count);
				result = list[index];
			}
			return result;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000A315C File Offset: 0x000A155C
		private static IntVector2 GetRandomPosForDropItem(Map<Field> fields)
		{
			List<Field> list = new List<Field>();
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = size - 1; j >= 0; j--)
				{
					Field field = fields[i, j];
					if (field.isOn && field.IsSpawner)
					{
						list.Add(field);
						break;
					}
				}
			}
			int index = RandomHelper.Next(list.Count);
			return list[index].gridPosition;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000A31E5 File Offset: 0x000A15E5
		private static void SetDropSpawnAndAssignDroppable(Field field)
		{
			field.spawnType = SpawnTypes.None;
			field.isDropExit = false;
			field.isDropSpawner = true;
			field.AssignGem(ObjectiveGenerator.droppable);
		}

		// Token: 0x04005016 RID: 20502
		private static int SMALL = 2;

		// Token: 0x04005017 RID: 20503
		private static int MEDIUM = 3;

		// Token: 0x04005018 RID: 20504
		private static int BIG = 4;

		// Token: 0x04005019 RID: 20505
		private static readonly ABrush hiddenItemBrushSmall = new HiddenItemBrush(null, ObjectiveGenerator.SMALL, true, null);

		// Token: 0x0400501A RID: 20506
		private static readonly ABrush hiddenItemBrushMedium = new HiddenItemBrush(null, ObjectiveGenerator.MEDIUM, true, null);

		// Token: 0x0400501B RID: 20507
		private static readonly ABrush hiddenItemBrushBig = new HiddenItemBrush(null, ObjectiveGenerator.BIG, true, null);

		// Token: 0x0400501C RID: 20508
		private static Gem droppable = new Gem(GemColor.Droppable);
	}
}
