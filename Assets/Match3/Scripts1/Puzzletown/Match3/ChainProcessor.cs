using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x02000601 RID: 1537
	public class ChainProcessor : AMatchProcessor
	{
		// Token: 0x06002767 RID: 10087 RVA: 0x000AED30 File Offset: 0x000AD130
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.positionToGems.Clear();
			this.chainedPositions.Clear();
			if (match is IMatchWithGem)
			{
				IMatchWithGem matchWithGem = (IMatchWithGem)match;
				this.positionToGems.Add(matchWithGem.Gem.position, matchWithGem.Gem);
			}
			foreach (Gem value in match.Group)
			{
				if (!this.positionToGems.ContainsKey(value.position))
				{
					this.positionToGems.Add(value.position, value);
				}
				this.CheckSurroundings(value.position, fields);
			}
			match.Group.RemoveAll((Gem gem) => this.chainedPositions.Contains(gem.position));
			if (match is IMatchWithAffectedFields)
			{
				IMatchWithAffectedFields matchWithAffectedFields = (IMatchWithAffectedFields)match;
				foreach (IntVector2 intVector in matchWithAffectedFields.Fields)
				{
					if ((fields[intVector].HasGem && !fields[intVector].gem.IsMatchable) || fields[intVector].IsResistantBlocker)
					{
						this.positionToGems.Add(intVector, fields[intVector].gem);
						this.CheckSurroundings(intVector, fields);
					}
				}
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000AEED4 File Offset: 0x000AD2D4
		protected override void CheckSurroundings(IntVector2 pos, Fields fields)
		{
			this.CheckField(fields[pos], pos);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000AEEE4 File Offset: 0x000AD2E4
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			if (field.numChains > 0 && field.CanExplode)
			{
				field.numChains--;
				Gem gem = this.positionToGems[field.gridPosition];
				field.AssignGem(gem);
				this.chainedPositions.Add(field.gridPosition);
				field.removedModifier = true;
				field.isExploded = true;
				this.results.Add(new ChainExplosion(field));
			}
		}

		// Token: 0x040051E7 RID: 20967
		private Dictionary<IntVector2, Gem> positionToGems = new Dictionary<IntVector2, Gem>();

		// Token: 0x040051E8 RID: 20968
		private List<IntVector2> chainedPositions = new List<IntVector2>();
	}
}
