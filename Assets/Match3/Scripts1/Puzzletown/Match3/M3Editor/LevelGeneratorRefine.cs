using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000569 RID: 1385
	public static class LevelGeneratorRefine
	{
		// Token: 0x0600245F RID: 9311 RVA: 0x000A1D0C File Offset: 0x000A010C
		private static LevelConfig ValidateConfig(Fields fields, LevelConfig config)
		{
			if (config == null || config.data.gems.Length < 3)
			{
				config = new LevelConfig();
				config.data = new LevelData
				{
					gems = new MaterialAmount[]
					{
						new MaterialAmount(GemColor.Red.ToString(), 1, MaterialAmountUsage.Undefined, 0),
						new MaterialAmount(GemColor.Green.ToString(), 1, MaterialAmountUsage.Undefined, 0),
						new MaterialAmount(GemColor.Blue.ToString(), 1, MaterialAmountUsage.Undefined, 0)
					}
				};
				config.layout = new LevelLayout(fields);
			}
			return config;
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000A1DC8 File Offset: 0x000A01C8
		public static List<IntVector2> FindUnfillable(Fields fields, LevelConfig config)
		{
			config = LevelGeneratorRefine.ValidateConfig(fields, config);
			Trickler trickler = new Trickler(new GemFactory(config), new FieldMappings(fields));
			trickler.Trickle(fields);
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					if (fields[i, j].isOn)
					{
						if (!fields[i, j].HasGem)
						{
							list.Add(new IntVector2(i, j));
						}
						else
						{
							fields[i, j].HasGem = false;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000A1E74 File Offset: 0x000A0274
		public static List<IntVector2> ReflectPositions(List<IntVector2> positions, int gridSize, IEnumerable<Reflection> functions)
		{
			foreach (Reflection reflection in functions)
			{
				positions.AddRange(LayoutGenerator.ReflectPositions(reflection.canReflect, reflection.reflectedPos, positions, gridSize));
			}
			return positions;
		}
	}
}
