using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005ED RID: 1517
	public struct LineGemExplosion : ILinegemRotatingExplosion, IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, IExplosionResult, IMatchResult
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x000ADCA4 File Offset: 0x000AC0A4
		public LineGemExplosion(Fields fields, Gem gem)
		{
			this.gem = gem;
			this.group = new Group();
			this.fields = new List<IntVector2>();
			this.highlightPositions = new List<IntVector2>();
			this.isHorizontal = (gem.type == GemType.LineHorizontal);
			this.blockingPosStart = -1;
			this.blockingPosEnd = -1;
			IntVector2 position = gem.position;
			int num = (!this.isHorizontal) ? position.y : position.x;
			int size = fields.size;
			for (int i = num; i < size; i++)
			{
				if (!this.CheckPosition(i, position, fields))
				{
					this.blockingPosEnd = i;
					break;
				}
			}
			for (int j = num - 1; j >= 0; j--)
			{
				if (!this.CheckPosition(j, position, fields))
				{
					this.blockingPosStart = j;
					break;
				}
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x000ADD85 File Offset: 0x000AC185
		public IntVector2 Center
		{
			get
			{
				return this.gem.position;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x000ADD92 File Offset: 0x000AC192
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002706 RID: 9990 RVA: 0x000ADD9A File Offset: 0x000AC19A
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x000ADD9D File Offset: 0x000AC19D
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002708 RID: 9992 RVA: 0x000ADDA5 File Offset: 0x000AC1A5
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000ADDB0 File Offset: 0x000AC1B0
		private bool CheckPosition(int i, IntVector2 gemPos, Fields fields)
		{
			int x = (!this.isHorizontal) ? gemPos.x : i;
			int y = (!this.isHorizontal) ? i : gemPos.y;
			IntVector2 intVector = new IntVector2(x, y);
			if (fields[intVector].CanExplode && fields[intVector].HasGem)
			{
				if (!fields[intVector].GemBlocked && fields[intVector].gem.IsAffectedBySuperGems && !fields[intVector].gem.IsCovered)
				{
					this.Group.Add(fields[intVector].gem);
				}
				else
				{
					this.Fields.Add(intVector);
				}
			}
			else if (fields[intVector].isOn && !fields[intVector].isWindow && !fields[intVector].IsColorWheel)
			{
				this.Fields.Add(intVector);
			}
			if (fields[intVector].isOn)
			{
				this.HighlightPositions.Add(intVector);
			}
			return !fields[intVector].gem.BlocksLineGem() || fields[intVector].GemBlocked;
		}

		// Token: 0x040051A5 RID: 20901
		public Gem gem;

		// Token: 0x040051A6 RID: 20902
		public readonly int blockingPosStart;

		// Token: 0x040051A7 RID: 20903
		public readonly int blockingPosEnd;

		// Token: 0x040051A8 RID: 20904
		public readonly bool isHorizontal;

		// Token: 0x040051A9 RID: 20905
		private readonly Group group;

		// Token: 0x040051AA RID: 20906
		private readonly List<IntVector2> fields;

		// Token: 0x040051AB RID: 20907
		private readonly List<IntVector2> highlightPositions;
	}
}
