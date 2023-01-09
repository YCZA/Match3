using System.Collections.Generic;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020005F9 RID: 1529
	public abstract class AMatchProcessor : IMatchProcessor
	{
		// Token: 0x06002742 RID: 10050 RVA: 0x000AE49C File Offset: 0x000AC89C
		public IEnumerable<IMatchResult> Process(Fields fields, List<IMatchResult> matches)
		{
			this.results.Clear();
			foreach (IMatchResult matchResult in matches)
			{
				if (this.IsValid(matchResult))
				{
					this.ProcessMatch((IMatchGroup)matchResult, fields);
				}
			}
			return this.results;
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000AE518 File Offset: 0x000AC918
		private void CheckCurrent(IntVector2 pos, IntVector2 direction, Fields fields)
		{
			IntVector2 vec = pos + direction;
			if (fields.IsValid(vec))
			{
				this.CheckField(fields[vec], pos);
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000AE547 File Offset: 0x000AC947
		protected virtual bool IsValid(IMatchResult result)
		{
			return result is IMatchGroup;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000AE554 File Offset: 0x000AC954
		protected virtual void ProcessMatch(IMatchGroup match, Fields fields)
		{
			if (match.ShouldHitAdjacentFields)
			{
				foreach (Gem gem in match.Group)
				{
					this.CheckSurroundings(gem.position, fields);
				}
			}
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000AE5C4 File Offset: 0x000AC9C4
		protected virtual void CheckSurroundings(IntVector2 pos, Fields fields)
		{
			this.CheckCurrent(pos, IntVector2.Left, fields);
			this.CheckCurrent(pos, IntVector2.Up, fields);
			this.CheckCurrent(pos, IntVector2.Right, fields);
			this.CheckCurrent(pos, IntVector2.Down, fields);
		}

		// Token: 0x06002747 RID: 10055
		protected abstract void CheckField(Field field, IntVector2 createdFrom);

		// Token: 0x040051D6 RID: 20950
		protected List<IMatchResult> results = new List<IMatchResult>();
	}
}
