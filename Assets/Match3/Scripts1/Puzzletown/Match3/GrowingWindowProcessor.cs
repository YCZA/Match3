namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000613 RID: 1555
	public class GrowingWindowProcessor : AMatchProcessor
	{
		// Token: 0x060027BE RID: 10174 RVA: 0x000B0956 File Offset: 0x000AED56
		public GrowingWindowProcessor(FieldMappings fieldMappings, SpawnerMappings spawnerMappings)
		{
			this.fieldMappings = fieldMappings;
			this.spawnerMappings = spawnerMappings;
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000B096C File Offset: 0x000AED6C
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.fields = fields;
			base.ProcessMatch(match, fields);
			if (match is IMatchWithAffectedFields)
			{
				foreach (IntVector2 intVector in ((IMatchWithAffectedFields)match).Fields)
				{
					this.CheckField(fields[intVector], intVector);
				}
			}
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000B09F0 File Offset: 0x000AEDF0
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (field.isGrowingWindow)
			{
				IntVector2 intVector = field.gridPosition + IntVector2.Down;
				IntVector2 belowPosition = (!this.fields.IsValid(intVector) || !this.fields[intVector].isGrowingWindow) ? Fields.invalidPos : intVector;
				this.results.Add(new GrowingWindowExplosion(field.gridPosition, belowPosition, createdFrom));
				field.isExploded = true;
				field.isGrowingWindow = false;
				field.removedModifier = true;
				this.fieldMappings.UpdateFieldMappings(this.fields);
				this.spawnerMappings.UpdateSpawnPositions(this.fields);
			}
		}

		// Token: 0x04005213 RID: 21011
		private Fields fields;

		// Token: 0x04005214 RID: 21012
		private readonly FieldMappings fieldMappings;

		// Token: 0x04005215 RID: 21013
		private readonly SpawnerMappings spawnerMappings;
	}
}
