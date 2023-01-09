using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005F4 RID: 1524
	public struct SuperRainbowExplosion : IMatchGroup, IMatchWithAffectedFields, IHighlightPattern, ISwapResult, IMatchResult, IExplosionResult
	{
		// Token: 0x06002729 RID: 10025 RVA: 0x000AE200 File Offset: 0x000AC600
		public SuperRainbowExplosion(Fields fields, Move move)
		{
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.highlightPositions = null;
			this.center = move.to;
			this.from = move.from;
			this.CollectGemsAndFields(fields);
			if (!this.group.Includes(move.from))
			{
				this.group.Add(fields[move.from].gem);
			}
			if (!this.group.Includes(move.to))
			{
				this.group.Add(fields[move.to].gem);
			}
			this.highlightPositions = this.group.Positions;
			this.highlightPositions.AddRange(this.Fields);
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000AE2D5 File Offset: 0x000AC6D5
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x000AE2DD File Offset: 0x000AC6DD
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000AE2E5 File Offset: 0x000AC6E5
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x000AE2ED File Offset: 0x000AC6ED
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000AE2F5 File Offset: 0x000AC6F5
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x000AE2FD File Offset: 0x000AC6FD
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000AE300 File Offset: 0x000AC700
		private void CollectGemsAndFields(Fields fields)
		{
			IEnumerator enumerator = fields.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Field field = (Field)obj;
					if (!field.GemBlocked && field.gem.IsAffectedBySuperGems)
					{
						this.Group.Add(field.gem);
					}
					else if (field.isOn && !field.IsColorWheel)
					{
						this.Fields.Add(field.gridPosition);
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
		}

		// Token: 0x040051C6 RID: 20934
		private readonly Group group;

		// Token: 0x040051C7 RID: 20935
		private readonly IntVector2 from;

		// Token: 0x040051C8 RID: 20936
		private readonly IntVector2 center;

		// Token: 0x040051C9 RID: 20937
		private readonly List<IntVector2> fields;

		// Token: 0x040051CA RID: 20938
		private readonly List<IntVector2> highlightPositions;
	}
}
