using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005EC RID: 1516
	public struct LinegemBombExplosion : ILinegemRotatingExplosion, IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, ISwapResult, IExplosionResult, IMatchResult
	{
		// Token: 0x060026FA RID: 9978 RVA: 0x000AD990 File Offset: 0x000ABD90
		public LinegemBombExplosion(Fields fields, IntVector2 to, IntVector2 from)
		{
			this.gem = fields[to].gem;
			this.from = from;
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.highlightPositions = new List<IntVector2>();
			IntVector2 position = this.gem.position + IntVector2.Left + IntVector2.Up;
			IntVector2 position2 = this.gem.position + IntVector2.Right + IntVector2.Down;
			this.blockingPositions = new int[,]
			{
				{
					-1,
					-1,
					-1,
					-1
				},
				{
					-1,
					-1,
					-1,
					-1
				},
				{
					-1,
					-1,
					-1,
					-1
				}
			};
			this.CheckCrossPosition(position, fields, 0);
			this.CheckCrossPosition(to, fields, 1);
			this.CheckCrossPosition(position2, fields, 2);
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000ADA4C File Offset: 0x000ABE4C
		public IntVector2 Center
		{
			get
			{
				return this.gem.position;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x000ADA59 File Offset: 0x000ABE59
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x000ADA61 File Offset: 0x000ABE61
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060026FE RID: 9982 RVA: 0x000ADA69 File Offset: 0x000ABE69
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000ADA6C File Offset: 0x000ABE6C
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x000ADA74 File Offset: 0x000ABE74
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000ADA7C File Offset: 0x000ABE7C
		private void CheckCrossPosition(IntVector2 position, Fields fields, int blockingPositionIndex)
		{
			int size = fields.size;
			for (int i = position.x; i < size; i++)
			{
				if (!this.CheckPosition(i, position, fields, true))
				{
					this.blockingPositions[blockingPositionIndex, 1] = i;
					break;
				}
			}
			for (int j = position.x - 1; j >= 0; j--)
			{
				if (!this.CheckPosition(j, position, fields, true))
				{
					this.blockingPositions[blockingPositionIndex, 0] = j;
					break;
				}
			}
			for (int k = position.y + 1; k < size; k++)
			{
				if (!this.CheckPosition(k, position, fields, false))
				{
					this.blockingPositions[blockingPositionIndex, 3] = k;
					break;
				}
			}
			for (int l = position.y - 1; l >= 0; l--)
			{
				if (!this.CheckPosition(l, position, fields, false))
				{
					this.blockingPositions[blockingPositionIndex, 2] = l;
					break;
				}
			}
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000ADB84 File Offset: 0x000ABF84
		private bool CheckPosition(int i, IntVector2 position, Fields fields, bool checkHorizontal)
		{
			int x = (!checkHorizontal) ? position.x : i;
			int y = (!checkHorizontal) ? i : position.y;
			IntVector2 intVector = new IntVector2(x, y);
			Field field = fields[intVector];
			if (field == null)
			{
				return true;
			}
			if (field.isOn && field.CanExplode && field.HasGem)
			{
				if (!field.GemBlocked && field.gem.IsAffectedBySuperGems)
				{
					this.Group.AddIfNotAlreadyPresent(field.gem, false);
				}
				else
				{
					this.Fields.AddIfNotAlreadyPresent(intVector, false);
				}
			}
			else if (field.isOn && !field.isWindow && !fields[intVector].IsColorWheel)
			{
				this.Fields.AddIfNotAlreadyPresent(intVector, false);
			}
			if (field.isOn)
			{
				this.HighlightPositions.AddIfNotAlreadyPresent(intVector, false);
			}
			return !field.gem.BlocksLineGem() || field.GemBlocked;
		}

		// Token: 0x0400519F RID: 20895
		public Gem gem;

		// Token: 0x040051A0 RID: 20896
		public readonly int[,] blockingPositions;

		// Token: 0x040051A1 RID: 20897
		private readonly Group group;

		// Token: 0x040051A2 RID: 20898
		private readonly IntVector2 from;

		// Token: 0x040051A3 RID: 20899
		private readonly List<IntVector2> fields;

		// Token: 0x040051A4 RID: 20900
		private readonly List<IntVector2> highlightPositions;
	}
}
