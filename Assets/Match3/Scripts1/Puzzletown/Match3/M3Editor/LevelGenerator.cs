using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Shared.DataStructures;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000568 RID: 1384
	public static class LevelGenerator
	{
		// Token: 0x06002450 RID: 9296 RVA: 0x000A1700 File Offset: 0x0009FB00
		public static void EnsureEnoughColors(LevelData data)
		{
			List<MaterialAmount> list = data.gems.ToList<MaterialAmount>();
			List<GemColor> list2 = new List<GemColor>();
			List<GemColor> list3 = new List<GemColor>();
			if (list.IsNullOrEmptyCollection())
			{
				list3.AddRange(LevelGenerator.GetRandomDifferentColors(3, LevelGenerator.availableColors));
			}
			else
			{
				list2 = LevelGenerator.GetColorsFromAmounts(data.gems);
				foreach (GemColor item in list2)
				{
					LevelGenerator.availableColors.Remove(item);
				}
				if (list2.Count < 3)
				{
					int amount = 3 - list2.Count;
					list3.AddRange(LevelGenerator.GetRandomDifferentColors(amount, LevelGenerator.availableColors));
				}
				LevelGenerator.availableColors.AddRange(list2);
			}
			foreach (GemColor color in list3)
			{
				LevelGenerator.AddColorAmountToList(list, color);
			}
			data.gems = list.ToArray();
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000A1834 File Offset: 0x0009FC34
		public static List<GemColor> GetColorsFromAmounts(MaterialAmount[] amounts)
		{
			List<GemColor> list = new List<GemColor>();
			foreach (MaterialAmount materialAmount in amounts)
			{
				try
				{
					GemColor item = (GemColor)Enum.Parse(typeof(GemColor), materialAmount.type, true);
					list.Add(item);
				}
				catch (ArgumentException)
				{
					WoogaDebug.LogWarning(new object[]
					{
						"Invalid color found"
					});
				}
			}
			return list;
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000A18C0 File Offset: 0x0009FCC0
		public static void Reflect(Map<Field> fields, IEnumerable<Reflection> functions)
		{
			foreach (Reflection function in functions)
			{
				LayoutGenerator.ApplyReflection(fields, function);
			}
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000A1914 File Offset: 0x0009FD14
		public static void AddReflectedModifierGroup(Map<Field> fields, int length, FieldModification mod, List<Reflection> functions)
		{
			ModifierGenerator.ModifyGroup(fields, length, mod);
			foreach (Reflection function in functions)
			{
				LayoutGenerator.ReflectLayoutModifiers(fields, function);
			}
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000A1974 File Offset: 0x0009FD74
		public static void AddReflectedStackedGems(Map<Field> fields, FieldModification mod, LevelData data, bool useRecipeColor, LevelGenerationGlobals settings, List<Reflection> functions)
		{
			List<GemColor> colorsFromAmounts = LevelGenerator.GetColorsFromAmounts(data.gems);
			if (useRecipeColor)
			{
				colorsFromAmounts = LevelGenerator.GetColorsFromAmounts(data.objectives);
			}
			GemColor color = colorsFromAmounts[RandomHelper.Next(colorsFromAmounts.Count)];
			ModifierGenerator.PlaceStackedGems(fields, color, functions);
			foreach (Reflection function in functions)
			{
				LayoutGenerator.ReflectRemainingModifiers(fields, function);
			}
			LevelGenerator.AddStackedGemsSpawnRatio(data, color, settings);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000A1A10 File Offset: 0x0009FE10
		public static void AddStackedGemsSpawnRatio(LevelData data, GemColor color, LevelGenerationGlobals settings)
		{
			if (!LevelGenerator.HasAlreadyRatioOfColor(data.spawnRatios, color))
			{
				List<SpawnRatio> list = data.spawnRatios.ToList<SpawnRatio>();
				list.Add(new SpawnRatio
				{
					gemColor = color,
					gemType = GemType.StackedGemMedium,
					minSpawn = settings.StackedGemInitSpawnMin,
					maxSpawn = RandomHelper.Next(settings.StackedGemInitSpawnMaxMin, settings.StackedGemInitSpawnMaxMax + 1),
					probability = RandomHelper.Next(settings.StackedGemInitSpawnRatioMin, settings.StackedGemInitSpawnRatioMax + 1)
				});
				data.spawnRatios = list.ToArray();
			}
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000A1A9F File Offset: 0x0009FE9F
		public static void CutGroups(Map<Field> fields, int count, int minLength, int maxLength)
		{
			ModifierGenerator.ModifyGroups(fields, count, minLength, maxLength, ModifierGenerator.GenerateCut);
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000A1AB0 File Offset: 0x0009FEB0
		public static void RefineLevel(Fields fields, LevelConfig config, int gridSize, IEnumerable<Reflection> functions)
		{
			List<IntVector2> positions = LevelGeneratorRefine.FindUnfillable(fields, config);
			List<IntVector2> list = LevelGeneratorRefine.ReflectPositions(positions, gridSize, functions);
			foreach (IntVector2 vec in list)
			{
				fields[vec].isOn = false;
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000A1B20 File Offset: 0x0009FF20
		public static void PlaceSpawner(Map<Field> fields)
		{
			ModifierGenerator.RemoveSpawner(fields);
			ModifierGenerator.PlaceSpawnerAtTopPosition(fields);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000A1B30 File Offset: 0x0009FF30
		public static void GenerateAndPreplaceObjective(Map<Field> fields, Objective objective, LevelData data, LevelGenerationGlobals settings, bool horizontalReflection)
		{
			if (objective != Objective.Recipe)
			{
				if (objective != Objective.HiddenItem)
				{
					if (objective == Objective.DropItem)
					{
						ObjectiveGenerator.GenerateAndPreplaceDropItems(fields, data, settings, horizontalReflection);
					}
				}
				else
				{
					ObjectiveGenerator.GenerateAndPreplaceHiddenItems(fields, data);
				}
			}
			else
			{
				ObjectiveGenerator.GenerateColorObjective(fields, data);
			}
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000A1B80 File Offset: 0x0009FF80
		public static void FillBoardWithGems(Fields fields)
		{
			Gem gem = new Gem(GemColor.Random);
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.isOn && !field.IsBlocked && !field.HasGem)
					{
						field.AssignGem(gem);
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
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000A1C0C File Offset: 0x000A000C
		public static List<GemColor> GetRandomDifferentColors(int amount, List<GemColor> availableColors)
		{
			List<GemColor> list = new List<GemColor>();
			for (int i = 0; i < amount; i++)
			{
				GemColor item = availableColors[RandomHelper.Next(availableColors.Count)];
				list.Add(item);
				availableColors.Remove(item);
			}
			availableColors.AddRangeElementsIfNotAlreadyPresent(list, false);
			return list;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000A1C5C File Offset: 0x000A005C
		public static void AddColorAmountToList(List<MaterialAmount> results, GemColor color)
		{
			MaterialAmount item = new MaterialAmount(color.ToString().ToLower(), 50, MaterialAmountUsage.Undefined, 0);
			results.Add(item);
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000A1C90 File Offset: 0x000A0090
		private static bool HasAlreadyRatioOfColor(SpawnRatio[] ratios, GemColor color)
		{
			foreach (SpawnRatio spawnRatio in ratios)
			{
				if (spawnRatio.gemColor == color)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004FCD RID: 20429
		public const int MIN_COLORS_AMOUNT = 3;

		// Token: 0x04004FCE RID: 20430
		public static List<GemColor> availableColors = new List<GemColor>
		{
			GemColor.Blue,
			GemColor.Green,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Red,
			GemColor.Yellow
		};
	}
}
