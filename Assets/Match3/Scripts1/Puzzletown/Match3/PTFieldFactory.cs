namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200062C RID: 1580
	public class PTFieldFactory
	{
		// Token: 0x06002830 RID: 10288 RVA: 0x000B293C File Offset: 0x000B0D3C
		public Field CreateField(LevelLayout config, int x, int y)
		{
			Field field = new Field(x, y);
			int fieldSize = config.FieldSize;
			int index = LevelLayout.GetIndex(field.gridPosition, fieldSize);
			field.isOn = (config.fields[index] > 0);
			field.cratesIndex = PTFieldFactory.TryToGetValue(config.crates, index);
			field.numChains = PTFieldFactory.TryToGetValue(config.chains, index);
			field.portalId = PTFieldFactory.TryToGetValue(config.portals, index);
			field.blockerIndex = PTFieldFactory.TryToGetValue(config.stones, index);
			field.numTiles = PTFieldFactory.TryToGetValue(config.tiles, index);
			int num = PTFieldFactory.TryToGetValue(config.dropItems, index);
			field.isDropSpawner = (num == 1);
			field.isDropExit = (num == 2);
			int num2 = PTFieldFactory.TryToGetValue(config.climbers, index);
			field.isClimberSpawner = (num2 == 1);
			field.isClimberExit = (num2 == 2);
			field.spawnType = (SpawnTypes)PTFieldFactory.TryToGetValue(config.spawning, index);
			field.hiddenItemId = PTFieldFactory.TryToGetValue(config.hiddenItems, index);
			if (field.isOn && config.fields[index] == 2)
			{
				field.isWindow = true;
			}
			if (field.isOn && config.fields[index] == 3)
			{
				field.isGrowingWindow = true;
			}
			return field;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000B2A7A File Offset: 0x000B0E7A
		private static int TryToGetValue(int[] arr, int index)
		{
			if (arr == null)
			{
				return 0;
			}
			return (index >= arr.Length) ? 0 : arr[index];
		}
	}
}
