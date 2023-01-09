using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200061D RID: 1565
	public static class ProcessorHelper
	{
		// Token: 0x060027E1 RID: 10209 RVA: 0x000B1544 File Offset: 0x000AF944
		public static void RemoveCoverAndBlocker(Field field, List<IMatchResult> results)
		{
			if (field.IsStone)
			{
				field.blockerIndex = 0;
				results.Add(new StoneExplosion(field, field.gridPosition));
			}
			else if (field.numChains > 0)
			{
				field.numChains = 0;
				results.Add(new ChainExplosion(field));
			}
			else if (field.HasCrates)
			{
				GemColor color = Crate.GetColor(field.cratesIndex);
				field.cratesIndex = Crate.GetIndex(color, 0);
				results.Add(new CrateExplosion(field, field.gridPosition));
			}
			if (field.IsResistantBlocker)
			{
				field.blockerIndex = 0;
				results.Add(new ResistantBlockerExplosion(field, field.gridPosition, true));
			}
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000B1610 File Offset: 0x000AFA10
		public static void RemoveGrowingWindow(Fields fields, Field field, List<IMatchResult> results)
		{
			if (field.isGrowingWindow)
			{
				field.isGrowingWindow = false;
				IntVector2 intVector = field.gridPosition + IntVector2.Down;
				IntVector2 belowPosition = (!fields.IsValid(intVector) || !fields[intVector].isGrowingWindow) ? Fields.invalidPos : intVector;
				results.Add(new GrowingWindowExplosion(field.gridPosition, belowPosition, field.gridPosition));
			}
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000B1688 File Offset: 0x000AFA88
		public static void RemoveChameleonGem(Field field, List<IMatchResult> results, GemFactory gemFactory)
		{
			if (field.HasGem && field.gem.IsChameleon)
			{
				field.isExploded = true;
				results.Add(new ChameleonMatched(field.gem, GemColor.Coins, GemColor.Undefined, true));
				gemFactory.StopSpawningChameleonGems();
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000B16D8 File Offset: 0x000AFAD8
		public static void RemoveGemCover(Field field, List<IMatchResult> results)
		{
			if (field.gem.IsIced)
			{
				field.gem.modifier = GemModifier.Undefined;
				results.Add(new IceExplosion(field.gridPosition, field.gem));
			}
			else if (field.gem.IsCoveredByDirt)
			{
				field.gem.modifier = GemModifier.Undefined;
				results.Add(new DirtExplosion(field.gridPosition, field.gem, 0, field.gridPosition));
				if (field.gem.color == GemColor.Treasure)
				{
					results.Add(new TreasureCollected(field.gridPosition, field.gem, field.gridPosition));
				}
				field.HasGem = false;
			}
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000B179C File Offset: 0x000AFB9C
		public static void RemoveGem(Field field, List<IMatchResult> results)
		{
			if (field.HasGem)
			{
				Group group = new Group(field.gem);
				results.Add(new Match(group, false));
				field.HasGem = false;
			}
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x000B17D9 File Offset: 0x000AFBD9
		public static void RemoveTiles(Field field, List<IMatchResult> results)
		{
			if (field.numTiles > 0 && field.numTiles <= 2)
			{
				field.numTiles = 0;
				results.Add(new TileExplosion(field));
			}
		}
	}
}
