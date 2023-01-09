using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005D1 RID: 1489
	public struct ColorWheelExplosion : IMatchGroup, IMatchWithAffectedFields, ISwapResult, IMatchResult, IExplosionResult
	{
		// Token: 0x060026AF RID: 9903 RVA: 0x000AD48C File Offset: 0x000AB88C
		public ColorWheelExplosion(Fields fields, IntVector2 position)
		{
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.center = position;
			this.from = position;
			this.highlightPositions = null;
			this.CollectGemsAndFields(fields);
			this.highlightPositions = this.group.Positions;
			this.highlightPositions.AddRange(this.Fields);
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x000AD4ED File Offset: 0x000AB8ED
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000AD4F5 File Offset: 0x000AB8F5
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x000AD4FD File Offset: 0x000AB8FD
		public IntVector2 Center
		{
			get
			{
				return this.center;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x000AD505 File Offset: 0x000AB905
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000AD50D File Offset: 0x000AB90D
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000AD515 File Offset: 0x000AB915
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000AD518 File Offset: 0x000AB918
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
						this.group.Add(field.gem);
					}
					else if (field.isOn && !field.IsColorWheel)
					{
						this.fields.Add(field.gridPosition);
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

		// Token: 0x04005176 RID: 20854
		private readonly Group group;

		// Token: 0x04005177 RID: 20855
		private readonly IntVector2 from;

		// Token: 0x04005178 RID: 20856
		private readonly IntVector2 center;

		// Token: 0x04005179 RID: 20857
		private readonly List<IntVector2> fields;

		// Token: 0x0400517A RID: 20858
		private readonly List<IntVector2> highlightPositions;
	}
}
