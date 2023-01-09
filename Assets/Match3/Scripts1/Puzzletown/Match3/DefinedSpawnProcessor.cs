using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000611 RID: 1553
	public class DefinedSpawnProcessor : IMatchProcessor
	{
		// Token: 0x060027B6 RID: 10166 RVA: 0x000B067C File Offset: 0x000AEA7C
		public DefinedSpawnProcessor(APTBoardFactory boardFactory)
		{
			this.boardFactory = boardFactory;
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000B0698 File Offset: 0x000AEA98
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> allResults)
		{
			this.results.Clear();
			for (int i = 0; i < allResults.Count; i++)
			{
				IMatchResult matchResult = allResults[i];
				if (this.DefinedSpawnNeeded(matchResult, fields))
				{
					allResults[i] = this.SpawnDefinedGem((DefinedSpawnNeeded)matchResult, fields);
				}
			}
			return this.results;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000B06FB File Offset: 0x000AEAFB
		private bool DefinedSpawnNeeded(IMatchResult result, Fields fields)
		{
			return result is DefinedSpawnNeeded && this.DefinedSpawnNeeded((DefinedSpawnNeeded)result, fields);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000B0718 File Offset: 0x000AEB18
		private bool DefinedSpawnNeeded(DefinedSpawnNeeded spawnNeeded, Fields fields)
		{
			return !spawnNeeded.IsProcessed && !fields[spawnNeeded.Position].HasGem;
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000B0740 File Offset: 0x000AEB40
		private DefinedSpawnNeeded SpawnDefinedGem(DefinedSpawnNeeded spawnNeeded, Fields fields)
		{
			spawnNeeded.IsProcessed = true;
			Gem gemFromConfig = this.boardFactory.GetGemFromConfig(spawnNeeded.Position, false);
			if (gemFromConfig.color == GemColor.Random)
			{
				gemFromConfig.color = this.boardFactory.GemFactory.GetRandomColor();
			}
			if (gemFromConfig.type == GemType.ClimberGem && this.boardFactory.GemFactory.HasStoppedSpawningClimberGems())
			{
				gemFromConfig.type = GemType.Undefined;
			}
			IntVector2 position = spawnNeeded.Position;
			fields[position].AssignGem(gemFromConfig);
			this.results.Add(new DefinedSpawnResult(position, gemFromConfig));
			return spawnNeeded;
		}

		// Token: 0x04005211 RID: 21009
		private readonly List<IMatchResult> results = new List<IMatchResult>();

		// Token: 0x04005212 RID: 21010
		private readonly APTBoardFactory boardFactory;
	}
}
