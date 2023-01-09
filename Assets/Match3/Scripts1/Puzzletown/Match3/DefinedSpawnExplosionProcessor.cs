using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200060F RID: 1551
	public class DefinedSpawnExplosionProcessor : IMatchProcessor
	{
		// Token: 0x060027AE RID: 10158 RVA: 0x000B051C File Offset: 0x000AE91C
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> allResults)
		{
			this.results.Clear();
			for (int i = 0; i < allResults.Count; i++)
			{
				IMatchResult matchResult = allResults[i];
				if (matchResult is IMatchGroup)
				{
					this.ProcessMatchGroup((IMatchGroup)matchResult, fields);
				}
				else if (matchResult is CannonballExplosion)
				{
					this.ProcessCannonballExplosion((CannonballExplosion)matchResult, fields);
				}
			}
			return this.results;
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000B0590 File Offset: 0x000AE990
		private void ProcessMatchGroup(IMatchGroup match, Fields fields)
		{
			foreach (Gem gem in match.Group)
			{
				this.CheckAndAddSpawnIfNeeded(fields[gem.position], gem.type == GemType.Bomb);
			}
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000B0604 File Offset: 0x000AEA04
		private void ProcessCannonballExplosion(CannonballExplosion explosion, Fields fields)
		{
			this.CheckAndAddSpawnIfNeeded(fields[explosion.position], false);
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000B061A File Offset: 0x000AEA1A
		private void CheckAndAddSpawnIfNeeded(Field field, bool skipExplodingBomb = false)
		{
			if (!field.removedModifier && field.IsDefinedGemSpawner && !skipExplodingBomb)
			{
				this.results.Add(new DefinedSpawnNeeded(field.gridPosition));
			}
		}

		// Token: 0x0400520E RID: 21006
		private List<IMatchResult> results = new List<IMatchResult>();
	}
}
