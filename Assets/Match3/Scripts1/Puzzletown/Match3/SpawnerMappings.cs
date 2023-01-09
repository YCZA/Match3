using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200062F RID: 1583
	public class SpawnerMappings
	{
		// Token: 0x06002840 RID: 10304 RVA: 0x000B3B14 File Offset: 0x000B1F14
		public SpawnerMappings(FieldMappings fieldMappings)
		{
			this.fieldMappings = fieldMappings;
			this.set = new HashSet<SpawnerMappings.SpawnerEntry>();
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000B3B30 File Offset: 0x000B1F30
		public void Initialize(Fields fields)
		{
			this.set.Clear();
			int size = fields.size;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Field field = fields[i, j];
					if (field.IsSpawner || field.isDropSpawner || field.CanSpawnChameleons)
					{
						SpawnerMappings.SpawnerTypes spawnerType = SpawnerMappings.SpawnerTypes.GemSpawner;
						if (field.isDropSpawner)
						{
							spawnerType = SpawnerMappings.SpawnerTypes.DroppableSpawner;
						}
						else if (field.CanSpawnChameleons)
						{
							spawnerType = SpawnerMappings.SpawnerTypes.ChameleonSpawner;
						}
						if (field.isGrowingWindow)
						{
							this.set.Add(new SpawnerMappings.SpawnerEntry(field.gridPosition, Fields.invalidPos, spawnerType));
							this.ResetSpawnerField(field);
						}
						else
						{
							this.set.Add(new SpawnerMappings.SpawnerEntry(field.gridPosition, field.gridPosition, spawnerType));
						}
					}
				}
			}
			this.UpdateSpawnPositions(fields);
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000B3C1C File Offset: 0x000B201C
		public void UpdateSpawnPositions(Fields fields)
		{
			foreach (SpawnerMappings.SpawnerEntry spawnerEntry in this.set)
			{
				if (spawnerEntry.currentPosition != spawnerEntry.desiredPosition)
				{
					if (!fields[spawnerEntry.desiredPosition].isGrowingWindow)
					{
						this.ResetToDesiredPosition(fields, spawnerEntry);
					}
					else
					{
						IntVector2 intVector = this.fieldMappings.Below(spawnerEntry.desiredPosition);
						Field field = fields[intVector];
						if (intVector != Fields.invalidPos && intVector != spawnerEntry.currentPosition && field.CanPlaceSpawner)
						{
							if (spawnerEntry.currentPosition != Fields.invalidPos)
							{
								this.ResetSpawnerField(fields[spawnerEntry.currentPosition]);
							}
							this.SetSpawnerField(fields[intVector], spawnerEntry.spawnerType);
							this.ResetSpawnerField(fields[spawnerEntry.desiredPosition]);
							if (spawnerEntry.spawnerType == SpawnerMappings.SpawnerTypes.DroppableSpawner && field.isDropExit)
							{
								fields[intVector].isDropSpawner = false;
								fields[intVector].spawnType = SpawnTypes.NormalSpawn;
							}
							spawnerEntry.currentPosition = intVector;
						}
					}
				}
			}
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000B3D80 File Offset: 0x000B2180
		private void ResetToDesiredPosition(Fields fields, SpawnerMappings.SpawnerEntry spawner)
		{
			if (fields.IsValid(spawner.currentPosition))
			{
				this.ResetSpawnerField(fields[spawner.currentPosition]);
			}
			spawner.currentPosition = spawner.desiredPosition;
			this.SetSpawnerField(fields[spawner.desiredPosition], spawner.spawnerType);
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000B3DD4 File Offset: 0x000B21D4
		public void UpdateAfterSpawn(Fields fields, IntVector2 position)
		{
			foreach (SpawnerMappings.SpawnerEntry spawnerEntry in this.set)
			{
				if (spawnerEntry.desiredPosition == position)
				{
					spawnerEntry.currentPosition = Fields.invalidPos;
				}
			}
			this.UpdateSpawnPositions(fields);
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000B3E4C File Offset: 0x000B224C
		private void SetSpawnerField(Field field, SpawnerMappings.SpawnerTypes spawnType)
		{
			if (spawnType != SpawnerMappings.SpawnerTypes.GemSpawner)
			{
				if (spawnType != SpawnerMappings.SpawnerTypes.DroppableSpawner)
				{
					if (spawnType == SpawnerMappings.SpawnerTypes.ChameleonSpawner)
					{
						field.spawnType = SpawnTypes.ChameleonSpawn;
						field.isDropSpawner = false;
					}
				}
				else
				{
					field.spawnType = SpawnTypes.None;
					field.isDropSpawner = true;
				}
			}
			else
			{
				field.spawnType = SpawnTypes.NormalSpawn;
				field.isDropSpawner = false;
			}
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000B3EAB File Offset: 0x000B22AB
		private void ResetSpawnerField(Field field)
		{
			field.isDropSpawner = false;
			field.spawnType = SpawnTypes.None;
		}

		// Token: 0x04005246 RID: 21062
		private readonly FieldMappings fieldMappings;

		// Token: 0x04005247 RID: 21063
		private readonly HashSet<SpawnerMappings.SpawnerEntry> set;

		// Token: 0x02000630 RID: 1584
		private enum SpawnerTypes
		{
			// Token: 0x04005249 RID: 21065
			GemSpawner,
			// Token: 0x0400524A RID: 21066
			DroppableSpawner,
			// Token: 0x0400524B RID: 21067
			ChameleonSpawner
		}

		// Token: 0x02000631 RID: 1585
		private class SpawnerEntry
		{
			// Token: 0x06002847 RID: 10311 RVA: 0x000B3EBB File Offset: 0x000B22BB
			public SpawnerEntry(IntVector2 desiredPosition, IntVector2 currentPosition, SpawnerMappings.SpawnerTypes spawnerType)
			{
				this.desiredPosition = desiredPosition;
				this.currentPosition = currentPosition;
				this.spawnerType = spawnerType;
			}

			// Token: 0x0400524C RID: 21068
			public readonly SpawnerMappings.SpawnerTypes spawnerType;

			// Token: 0x0400524D RID: 21069
			public readonly IntVector2 desiredPosition;

			// Token: 0x0400524E RID: 21070
			public IntVector2 currentPosition;
		}
	}
}
