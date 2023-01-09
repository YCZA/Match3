using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005AC RID: 1452
	public abstract class ASwapMatcher : AMatcher
	{
		// Token: 0x060025FC RID: 9724 RVA: 0x000A9834 File Offset: 0x000A7C34
		public ASwapMatcher(Fields fields) : base(fields)
		{
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000A983D File Offset: 0x000A7C3D
		protected override IEnumerable<IMatchResult> FindMatches(Move move, List<Group> groups)
		{
			this.InitMatcherWithMove(move);
			return this.FindMatches(move);
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000A984D File Offset: 0x000A7C4D
		protected override IEnumerable<IMatchResult> RemoveMatches(Move move, List<Group> groups)
		{
			return this.RemoveMatches(move);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000A9858 File Offset: 0x000A7C58
		public void InitMatcherWithMove(Move move)
		{
			if (this.fields.IsSwapPossible(move.from, move.to))
			{
				this.from = this.fields[move.from].gem;
				this.to = this.fields[move.to].gem;
			}
			else
			{
				this.from = default(Gem);
				this.to = default(Gem);
			}
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000A98E0 File Offset: 0x000A7CE0
		public void MarkMatches(Move move)
		{
			if (this.HasMatch(move))
			{
				this.fields[move.from].willExplode = true;
				this.fields[move.to].willExplode = true;
			}
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000A9920 File Offset: 0x000A7D20
		public bool HasMatch(Move move)
		{
			return this.fields.IsSwapPossible(move.from, move.to) && !move.Equals(default(Move)) && !this.from.Equals(this.to) && this.HasMatchInternal();
		}

		// Token: 0x06002602 RID: 9730
		protected abstract bool HasMatchInternal();

		// Token: 0x06002603 RID: 9731 RVA: 0x000A9980 File Offset: 0x000A7D80
		protected List<IMatchResult> FindMatches(Move move)
		{
			List<IMatchResult> result = new List<IMatchResult>();
			if (this.HasMatch(move))
			{
				result = this.FindMatches();
			}
			return result;
		}

		// Token: 0x06002604 RID: 9732
		protected abstract List<IMatchResult> FindMatches();

		// Token: 0x06002605 RID: 9733 RVA: 0x000A99A8 File Offset: 0x000A7DA8
		protected virtual IEnumerable<IMatchResult> RemoveMatches(Move move)
		{
			List<IMatchResult> list = new List<IMatchResult>();
			if (!this.HasMatch(move))
			{
				return list;
			}
			this.fields[move.from].willExplode = false;
			this.fields[move.to].willExplode = false;
			list = this.FindMatches();
			foreach (IMatchResult matchResult in list)
			{
				if (matchResult is IMatchWithGem)
				{
					Gem gem = ((IMatchWithGem)matchResult).Gem;
					this.fields[gem.position].AssignGem(gem);
					this.fields[gem.position].isExploded = true;
				}
				if (matchResult is IMatchGroup)
				{
					base.HitGems((IMatchGroup)matchResult);
				}
			}
			return list;
		}

		// Token: 0x04005103 RID: 20739
		protected Gem from;

		// Token: 0x04005104 RID: 20740
		protected Gem to;
	}
}
