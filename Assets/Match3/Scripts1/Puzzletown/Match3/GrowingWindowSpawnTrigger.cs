using System;
using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000634 RID: 1588
	public class GrowingWindowSpawnTrigger : ITrigger
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x000B4620 File Offset: 0x000B2A20
		public GrowingWindowSpawnTrigger(FieldMappings fieldMappings, SpawnerMappings spawnerMappings)
		{
			this.fieldMappings = fieldMappings;
			this.spawnerMappings = spawnerMappings;
		}

		// eli key point 处理生长窗口
		public void ExecuteTrigger(Fields fields, List<List<IMatchResult>> allResults)
		{
			foreach (List<IMatchResult> list in allResults)
			{
				foreach (IMatchResult matchResult in list)
				{
					if (matchResult is GrowingWindowExplosion || matchResult is GrowingWindowSpawn)
					{
						return;
					}
				}
			}
			GrowingWindowSpawn growingWindowSpawn = this.SpawnNextGrowingWindow(fields);
			if (!growingWindowSpawn.Equals(default(GrowingWindowSpawn)))
			{
				allResults.Add(new List<IMatchResult>
				{
					growingWindowSpawn
				});
			}
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000B4790 File Offset: 0x000B2B90
		private GrowingWindowSpawn SpawnNextGrowingWindow(Fields fields)
		{
			GrowingWindowSpawn result = default(GrowingWindowSpawn);
			this.GetAdjacentPossibleTargets(fields);
			bool flag = this.verticalTargets.IsNullOrEmptyCollection() && this.horizontalTargets.IsNullOrEmptyCollection();
			if (flag)
			{
				return result;
			}
			IntVector2 intVector = this.FindBestTarget(fields, this.verticalTargets, this.horizontalTargets);
			if (intVector != Fields.invalidPos)
			{
				IntVector2 belowPosition = this.TryGetGrowingWindowPosition(fields, intVector + IntVector2.Down);
				IntVector2 abovePosition = this.TryGetGrowingWindowPosition(fields, intVector + IntVector2.Up);
				IntVector2 leftPosition = this.TryGetGrowingWindowPosition(fields, intVector + IntVector2.Left);
				IntVector2 rightPosition = this.TryGetGrowingWindowPosition(fields, intVector + IntVector2.Right);
				result = new GrowingWindowSpawn(intVector, abovePosition, belowPosition, leftPosition, rightPosition, fields[intVector].gem);
				fields[intVector].isGrowingWindow = true;
				fields[intVector].HasGem = false;
				this.fieldMappings.UpdateFieldMappings(fields);
				this.spawnerMappings.UpdateAfterSpawn(fields, intVector);
			}
			return result;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000B4895 File Offset: 0x000B2C95
		private IntVector2 TryGetGrowingWindowPosition(Fields fields, IntVector2 pos)
		{
			return (!fields.IsValid(pos) || !fields[pos].isGrowingWindow) ? Fields.invalidPos : pos;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000B48C0 File Offset: 0x000B2CC0
		private void GetAdjacentPossibleTargets(Fields fields)
		{
			this.verticalTargets.Clear();
			this.horizontalTargets.Clear();
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (fields.IsValid(intVector) && fields[intVector].isOn)
					{
						bool isGrowingWindow = fields[intVector].isGrowingWindow;
						IntVector2 intVector2 = intVector + IntVector2.Up;
						IntVector2 intVector3 = intVector + IntVector2.Right;
						if (isGrowingWindow)
						{
							if (fields.IsValid(intVector2) && GrowingWindowSpawnTrigger.IsPossibleTarget(fields[intVector2]) && !fields[intVector2].isGrowingWindow)
							{
								this.AddToTargetList(this.verticalTargets, intVector2);
							}
							if (fields.IsValid(intVector3) && GrowingWindowSpawnTrigger.IsPossibleTarget(fields[intVector3]) && !fields[intVector3].isGrowingWindow)
							{
								this.AddToTargetList(this.horizontalTargets, intVector3);
							}
						}
						else
						{
							if (fields.IsValid(intVector2) && GrowingWindowSpawnTrigger.IsPossibleTarget(fields[intVector]) && fields[intVector2].isGrowingWindow)
							{
								this.AddToTargetList(this.verticalTargets, intVector);
							}
							if (fields.IsValid(intVector3) && GrowingWindowSpawnTrigger.IsPossibleTarget(fields[intVector]) && fields[intVector3].isGrowingWindow)
							{
								this.AddToTargetList(this.horizontalTargets, intVector);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x000B4A7A File Offset: 0x000B2E7A
		private void AddToTargetList(List<IntVector2> targetList, IntVector2 position)
		{
			if (!targetList.Contains(position))
			{
				targetList.Add(position);
			}
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000B4A90 File Offset: 0x000B2E90
		private IntVector2 FindBestTarget(Fields fields, List<IntVector2> verticalTargetList, List<IntVector2> horizontalTargetList)
		{
			verticalTargetList.Shuffle<IntVector2>();
			horizontalTargetList.Shuffle<IntVector2>();
			foreach (Predicate<Field> predicate in this.predicates)
			{
				foreach (IntVector2 vec in verticalTargetList)
				{
					Field field = fields[vec];
					if (predicate(field))
					{
						return field.gridPosition;
					}
				}
				foreach (IntVector2 vec2 in horizontalTargetList)
				{
					Field field2 = fields[vec2];
					if (predicate(field2))
					{
						return field2.gridPosition;
					}
				}
			}
			return Fields.invalidPos;
		}

		// Token: 0x04005256 RID: 21078
		private readonly FieldMappings fieldMappings;

		// Token: 0x04005257 RID: 21079
		private readonly SpawnerMappings spawnerMappings;

		// Token: 0x04005258 RID: 21080
		private readonly List<IntVector2> verticalTargets = new List<IntVector2>();

		// Token: 0x04005259 RID: 21081
		private readonly List<IntVector2> horizontalTargets = new List<IntVector2>();

		// Token: 0x0400525A RID: 21082
		private static readonly Predicate<Field> IsPossibleTarget = (Field f) => f.isOn && !f.isWindow && f.CanMove;

		// Token: 0x0400525B RID: 21083
		private static readonly Predicate<Field> IsColorGem = (Field f) => f.HasGem && f.gem.CanShuffle && f.CanMove;

		// Token: 0x0400525C RID: 21084
		private static readonly Predicate<Field> IsEmptyField = (Field f) => f.NeedsGem;

		// Token: 0x0400525D RID: 21085
		private static readonly Predicate<Field> IsSuperGem = (Field f) => f.HasGem && !f.gem.IsCovered && f.gem.IsSuperGem && !f.gem.IsRainbow;

		// Token: 0x0400525E RID: 21086
		private static readonly Predicate<Field> IsRainbowGem = (Field f) => f.HasGem && !f.gem.IsCovered && f.gem.IsRainbow;

		// Token: 0x0400525F RID: 21087
		private static readonly Predicate<Field> IsSpecialGem = (Field f) => f.HasGem && !f.gem.IsCovered && (f.gem.IsClimberGem || f.gem.IsStackedGem);

		// Token: 0x04005260 RID: 21088
		private readonly List<Predicate<Field>> predicates = new List<Predicate<Field>>(5)
		{
			GrowingWindowSpawnTrigger.IsColorGem,
			GrowingWindowSpawnTrigger.IsEmptyField,
			GrowingWindowSpawnTrigger.IsSuperGem,
			GrowingWindowSpawnTrigger.IsRainbowGem,
			GrowingWindowSpawnTrigger.IsSpecialGem
		};
	}
}
