using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200060B RID: 1547
	public class ClimberProcessor : IMatchProcessor
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x000AFAD0 File Offset: 0x000ADED0
		public ClimberProcessor(FieldMappings fieldMappings, GemFactory gemFactory)
		{
			this.mappings = fieldMappings;
			this.gemFactory = gemFactory;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000AFB20 File Offset: 0x000ADF20
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> allResults)
		{
			this.results.Clear();
			this.updateFieldMappings = false;
			if (this.spawnNewClimber)
			{
				this.SpawnNewClimber(fields);
			}
			for (int i = 0; i < allResults.Count; i++)
			{
				IMatchResult matchResult = allResults[i];
				if (this.CollectedClimberGemNeedsProcessing(matchResult))
				{
					allResults[i] = this.ProcessCollectedClimberGem((ClimberGemCollected)matchResult, fields);
				}
				else if (this.CollectedClimberNeedsProcessing(fields, matchResult))
				{
					allResults[i] = this.ProcessCollectedClimber((ClimberCollected)matchResult, fields);
				}
			}
			if (this.updateFieldMappings)
			{
				this.mappings.UpdateFieldMappings(fields);
			}
			return this.results;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000AFBE0 File Offset: 0x000ADFE0
		public static IntVector2 GetClimberPosition(Fields fields)
		{
			for (int i = 0; i < fields.size; i++)
			{
				for (int j = 0; j < fields.size; j++)
				{
					Field field = fields[i, j];
					if (field.HasGem && field.gem.color == GemColor.Climber)
					{
						return field.gridPosition;
					}
				}
			}
			return Fields.invalidPos;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000AFC50 File Offset: 0x000AE050
		private bool CollectedClimberGemNeedsProcessing(IMatchResult result)
		{
			return result is ClimberGemCollected && !((ClimberGemCollected)result).IsProcessed;
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000AFC7C File Offset: 0x000AE07C
		private ClimberGemCollected ProcessCollectedClimberGem(ClimberGemCollected collected, Fields fields)
		{
			collected.IsProcessed = true;
			IntVector2 climberPosition = ClimberProcessor.GetClimberPosition(fields);
			if (climberPosition != Fields.invalidPos)
			{
				ClimberMoves climberMoves = default(ClimberMoves);
				climberMoves.Steps = new List<List<IMatchResult>>();
				IntVector2 intVector = this.FindClimberPositionAbove(fields, climberPosition);
				if (intVector != Fields.invalidPos && !fields[intVector].BlocksClimber)
				{
					this.updateFieldMappings = true;
					ProcessorHelper.RemoveCoverAndBlocker(fields[intVector], this.results);
					fields.SwapGems(climberPosition, intVector);
					IntVector2 portalExitPosition = this.mappings.GetPortalExitPosition(climberPosition);
					IntVector2 portalEntryPosition = this.mappings.GetPortalEntryPosition(intVector);
					if (Portal.IsExit(this.mappings.PortalUsedAbove(climberPosition)))
					{
						climberMoves.Steps.Add(this.GetMovesThroughPortal(fields, climberPosition, intVector, portalEntryPosition, portalExitPosition));
					}
					else
					{
						ClimberMove climberMove = new ClimberMove(new Move(climberPosition, intVector, true, false, true));
						climberMoves.Steps.Add(new List<IMatchResult>
						{
							climberMove
						});
					}
				}
				this.results.Add(climberMoves);
			}
			return collected;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000AFD9C File Offset: 0x000AE19C
		private List<IMatchResult> GetMovesThroughPortal(Fields fields, IntVector2 climberPos, IntVector2 abovePosition, IntVector2 portalEntry, IntVector2 portalExit)
		{
			ClimberMove climberMove = ClimberMove.FromPortal(climberPos, portalExit + IntVector2.Up);
			List<IMatchResult> list = new List<IMatchResult>
			{
				climberMove
			};
			if (fields[climberPos].HasGem)
			{
				Move move = Move.FromPortal(abovePosition, portalEntry + IntVector2.Down);
				list.Add(move);
				SpawnResult spawnResult = new SpawnResult(climberPos, fields[climberPos].gem);
				list.Add(spawnResult);
			}
			ClimberSpawn climberSpawn = new ClimberSpawn(abovePosition, fields[abovePosition].gem, true);
			list.Add(climberSpawn);
			return list;
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000AFE44 File Offset: 0x000AE244
		private bool CollectedClimberNeedsProcessing(Fields fields, IMatchResult result)
		{
			return result is ClimberCollected && !((ClimberCollected)result).IsProcessed;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000AFE70 File Offset: 0x000AE270
		private ClimberCollected ProcessCollectedClimber(ClimberCollected collected, Fields fields)
		{
			collected.IsProcessed = true;
			if (this.gemFactory.StopSpawningClimberGems())
			{
				this.SwitchClimberGemsToNormalGems(fields);
			}
			else
			{
				this.spawnNewClimber = true;
				this.SpawnNewClimber(fields);
			}
			return collected;
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000AFEA8 File Offset: 0x000AE2A8
		private void SpawnNewClimber(Fields fields)
		{
			IntVector2 intVector = this.FindSpawnPosition(fields);
			if (intVector != Fields.invalidPos)
			{
				this.updateFieldMappings = true;
				Field field = fields[intVector];
				this.gemFactory.AddSpawnedClimber();
				ProcessorHelper.RemoveGrowingWindow(fields, field, this.results);
				ProcessorHelper.RemoveCoverAndBlocker(field, this.results);
				ProcessorHelper.RemoveGemCover(field, this.results);
				ProcessorHelper.RemoveGem(field, this.results);
				Field field2 = field;
				field2.gem.color = GemColor.Climber;
				this.results.Add(new ClimberSpawn(intVector, field2.gem, false));
				fields[intVector].isExploded = true;
				this.spawnNewClimber = false;
			}
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000AFF58 File Offset: 0x000AE358
		private void SwitchClimberGemsToNormalGems(Fields fields)
		{
			Group allGemsWithType = fields.GetAllGemsWithType(GemType.ClimberGem);
			Group group = new Group(allGemsWithType.Count);
			foreach (Gem gem in allGemsWithType)
			{
				fields[gem.position].gem.type = GemType.Undefined;
				group.Add(fields[gem.position].gem);
				fields[gem.position].isExploded = true;
				fields[gem.position].removedModifier = true;
			}
			this.results.Add(new UpdateGems(group));
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000B0028 File Offset: 0x000AE428
		private IntVector2 FindClimberPositionAbove(Fields fields, IntVector2 pos)
		{
			if (!fields.IsValid(pos))
			{
				return Fields.invalidPos;
			}
			IntVector2 intVector = this.mappings.Above(pos);
			if (intVector != Fields.invalidPos && fields[intVector].isOn)
			{
				return intVector;
			}
			return Fields.invalidPos;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000B007C File Offset: 0x000AE47C
		private IntVector2 FindSpawnPosition(Fields fields)
		{
			IntVector2 result = Fields.invalidPos;
			this.normalGemsAndEmptyPositions.Clear();
			this.coveredNormalGems.Clear();
			this.blockersAndSupergems.Clear();
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (field.CanSpawnClimber && !field.gem.IsCannon)
					{
						if ((!field.IsBlocked && !field.HasGem) || (field.gem.IsMatchable && field.gem.type == GemType.Undefined))
						{
							if (field.CanMove && !field.gem.IsCovered)
							{
								this.normalGemsAndEmptyPositions.Add(field.gridPosition);
							}
							else
							{
								this.coveredNormalGems.Add(field.gridPosition);
							}
						}
						else if (field.IsBlocked || field.gem.IsAffectedBySuperGems || field.gem.IsCoveredByDirt)
						{
							this.blockersAndSupergems.Add(field.gridPosition);
						}
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
			if (this.normalGemsAndEmptyPositions.Count > 0)
			{
				result = RandomHelper.Next<IntVector2>(this.normalGemsAndEmptyPositions);
			}
			else if (this.coveredNormalGems.Count > 0)
			{
				result = RandomHelper.Next<IntVector2>(this.coveredNormalGems);
			}
			else if (this.blockersAndSupergems.Count > 0)
			{
				result = RandomHelper.Next<IntVector2>(this.blockersAndSupergems);
			}
			return result;
		}

		// Token: 0x040051FF RID: 20991
		private List<IMatchResult> results = new List<IMatchResult>();

		// Token: 0x04005200 RID: 20992
		private readonly List<IntVector2> normalGemsAndEmptyPositions = new List<IntVector2>();

		// Token: 0x04005201 RID: 20993
		private readonly List<IntVector2> coveredNormalGems = new List<IntVector2>();

		// Token: 0x04005202 RID: 20994
		private readonly List<IntVector2> blockersAndSupergems = new List<IntVector2>();

		// Token: 0x04005203 RID: 20995
		private readonly FieldMappings mappings;

		// Token: 0x04005204 RID: 20996
		private readonly GemFactory gemFactory;

		// Token: 0x04005205 RID: 20997
		private bool updateFieldMappings;

		// Token: 0x04005206 RID: 20998
		private bool spawnNewClimber;
	}
}
