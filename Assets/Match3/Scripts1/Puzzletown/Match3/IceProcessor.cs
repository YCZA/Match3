using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x0200061A RID: 1562
	public class IceProcessor : AMatchProcessor
	{
		// Token: 0x060027DC RID: 10204 RVA: 0x000B1294 File Offset: 0x000AF694
		protected override void ProcessMatch(IMatchGroup match, Fields fields)
		{
			this.positionToGems.Clear();
			this.icedPositions.Clear();
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
				this.CheckField(fields[value.position], value.position);
			}
			match.Group.RemoveAll((Gem gem) => this.icedPositions.Contains(gem.position));
			if (match is IMatchWithAffectedFields)
			{
				IMatchWithAffectedFields matchWithAffectedFields = (IMatchWithAffectedFields)match;
				foreach (IntVector2 intVector in matchWithAffectedFields.Fields)
				{
					if (fields[intVector].HasGem && fields[intVector].gem.IsIced)
					{
						this.positionToGems.Add(intVector, fields[intVector].gem);
						this.CheckField(fields[intVector], intVector);
					}
				}
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000B143C File Offset: 0x000AF83C
		protected override void CheckField(Field field, IntVector2 createdFrom)
		{
			Gem gem = this.positionToGems[field.gridPosition];
			if (gem.IsIced && field.CanExplode)
			{
				gem.modifier--;
				field.AssignGem(gem);
				this.icedPositions.Add(field.gridPosition);
				field.isExploded = true;
				field.removedModifier = true;
				this.results.Add(new IceExplosion(field.gridPosition, gem));
			}
		}

		// Token: 0x04005223 RID: 21027
		private Dictionary<IntVector2, Gem> positionToGems = new Dictionary<IntVector2, Gem>();

		// Token: 0x04005224 RID: 21028
		private List<IntVector2> icedPositions = new List<IntVector2>();
	}
}
