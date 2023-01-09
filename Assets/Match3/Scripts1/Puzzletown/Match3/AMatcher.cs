using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AA RID: 1450
	public abstract class AMatcher : IMatcher
	{
		// Token: 0x060025DF RID: 9695 RVA: 0x000A8E2C File Offset: 0x000A722C
		public AMatcher(Fields fields)
		{
			this.fields = fields;
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x060025E0 RID: 9696 RVA: 0x000A8E46 File Offset: 0x000A7246
		protected Fields Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000A8E4E File Offset: 0x000A724E
		public IEnumerable<IMatchResult> FindMatches(Fields fields, Move move, List<Group> groups)
		{
			this.fields = fields;
			return this.FindMatches(move, groups);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000A8E5F File Offset: 0x000A725F
		public IEnumerable<IMatchResult> RemoveMatches(Fields fields, Move move, List<Group> groups)
		{
			this.fields = fields;
			return this.RemoveMatches(move, groups);
		}

		// Token: 0x060025E3 RID: 9699
		protected abstract IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups);

		// Token: 0x060025E4 RID: 9700 RVA: 0x000A8E70 File Offset: 0x000A7270
		protected virtual IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			IEnumerable<IMatchResult> enumerable = this.FindMatches(move, groups);
			foreach (IMatchResult matchResult in enumerable)
			{
				IMatchWithGem matchWithGem = matchResult as IMatchWithGem;
				if (matchWithGem != null)
				{
					Gem gem = matchWithGem.Gem;
					this.fields[gem.position].AssignGem(gem);
					this.fields[gem.position].isExploded = true;
				}
				IMatchGroup matchGroup = matchResult as IMatchGroup;
				if (matchGroup != null)
				{
					this.HitGems(matchGroup);
				}
			}
			return enumerable;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000A8F24 File Offset: 0x000A7324
		protected void HitGems(IMatchGroup match)
		{
			bool flag = match is IMatchWithGem;
			for (int i = 0; i < match.Group.Count; i++)
			{
				Gem gem = match.Group[i];
				if (!flag || !(gem.position == ((IMatchWithGem)match).Gem.position))
				{
					if (this.fields[gem.position].CanExplode && this.fields[gem.position].HasGem)
					{
						this.HitGem(gem);
					}
					else
					{
						match.Group.RemoveAt(i--);
					}
				}
			}
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000A8FE7 File Offset: 0x000A73E7
		protected bool CanExplosionHit(Gem gem)
		{
			return gem.CanExplosionHit && this.fields[gem.position].CanExplosionHitGem();
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000A900F File Offset: 0x000A740F
		protected void HitGem(Gem gem)
		{
			this.fields[gem.position].HasGem = false;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000A902C File Offset: 0x000A742C
		protected bool RemovesDuplicates(IMatchGroup match, Gem superGem)
		{
			return match is IMatchWithGem && ((IMatchWithGem)match).Gem.Equals(superGem);
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000A905C File Offset: 0x000A745C
		protected IntVector2 FindIdealSuperGemPosition(Group group, Move move)
		{
			IntVector2[] array = new IntVector2[]
			{
				move.to,
				move.from
			};
			foreach (IntVector2 intVector in array)
			{
				if (group.Includes(intVector) && this.IsPositionReplaceable(intVector))
				{
					return intVector;
				}
			}
			return this.FindIdealSuperGemPosition(group);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000A90DC File Offset: 0x000A74DC
		private IntVector2 FindIdealSuperGemPosition(Group group)
		{
			int count = group.Count;
			int num = count / 2;
			IntVector2 position = group[num].position;
			if (this.IsPositionReplaceable(position))
			{
				return position;
			}
			for (int i = num + 1; i < count; i++)
			{
				position = group[i].position;
				if (this.IsPositionReplaceable(position))
				{
					return position;
				}
			}
			for (int j = num - 1; j >= 0; j--)
			{
				position = group[j].position;
				if (this.IsPositionReplaceable(position))
				{
					return position;
				}
			}
			for (int k = count - 1; k > 0; k--)
			{
				position = group[k].position;
				if (this.fields[position].gem.IsReplaceable() && (this.fields[position].CanMove || this.IsPositionReplaceableAllowingChains(position)))
				{
					return position;
				}
			}
			return Fields.invalidPos;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000A91F0 File Offset: 0x000A75F0
		private bool IsPositionReplaceable(IntVector2 pos)
		{
			return this.fields[pos].CanMove && this.fields[pos].gem.IsReplaceable();
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000A9224 File Offset: 0x000A7624
		private bool IsPositionReplaceableAllowingChains(IntVector2 pos)
		{
			return !this.fields[pos].IsBlocked && !this.fields[pos].GemBlocked && !this.fields[pos].gem.IsCannon && !this.fields[pos].IsDefinedGemSpawner && this.fields[pos].gem.IsReplaceable();
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000A92A8 File Offset: 0x000A76A8
		protected void RemoveDuplicatesInExplosions(List<IMatchResult> results)
		{
			this.duplicates.Clear();
			for (int i = 0; i < results.Count; i++)
			{
				IExplosionResult explosionResult = (IExplosionResult)results[i];
				this.duplicates.Add(this.fields[explosionResult.Center].gem);
			}
			for (int j = 0; j < results.Count; j++)
			{
				IMatchGroup matchGroup = (IMatchGroup)results[j];
				matchGroup.Group.RemoveDuplicates(this.duplicates);
				if (matchGroup is IHighlightPattern)
				{
					IHighlightPattern highlightPattern = (IHighlightPattern)matchGroup;
					this.RemovePositionFromHighlight(highlightPattern);
					highlightPattern.HighlightPositions.Add(((IExplosionResult)matchGroup).Center);
				}
				this.duplicates.AddRange(matchGroup.Group);
				matchGroup.Group.Add(this.fields[((IExplosionResult)matchGroup).Center].gem);
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000A93A4 File Offset: 0x000A77A4
		private void RemovePositionFromHighlight(IHighlightPattern result)
		{
			foreach (Gem gem in this.duplicates)
			{
				result.HighlightPositions.Remove(gem.position);
			}
		}

		// Token: 0x040050FF RID: 20735
		private Group duplicates = new Group();

		// Token: 0x04005100 RID: 20736
		protected Fields fields;
	}
}
