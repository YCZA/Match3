using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Match3;
using Match3.Scripts1.Shared.M3Engine;

namespace Match3.Scripts1
{
	// Token: 0x020005C9 RID: 1481
	public struct BombExplosion : IMatchWithAffectedFields, IMatchGroup, IHighlightPattern, ISwapResult, IMatchResult, IExplosionResult
	{
		// Token: 0x06002685 RID: 9861 RVA: 0x000ACDC8 File Offset: 0x000AB1C8
		public BombExplosion(Fields fields, Gem gem, IntVector2 from, bool isSuperBomb = false)
		{
			this.group = new Group();
			this.from = from;
			this.fields = new List<IntVector2>();
			this.highlightPositions = new List<IntVector2>();
			this.gem = gem;
			this.isSuperBomb = isSuperBomb;
			if (gem.type == GemType.ActivatedBomb || gem.type == GemType.ActivatedSuperBomb)
			{
				this.Fields.Add(gem.position);
			}
			else
			{
				this.Group.Add(gem);
			}
			IntVector2[] array = BombExplosion.BOMB_RADIUS;
			this.HighlightPositions.Add(gem.position);
			if (isSuperBomb)
			{
				array = new IntVector2[BombExplosion.BOMB_RADIUS.Length + BombExplosion.SUPER_BOMB_RADIUS.Length];
				BombExplosion.BOMB_RADIUS.CopyTo(array, 0);
				BombExplosion.SUPER_BOMB_RADIUS.CopyTo(array, BombExplosion.BOMB_RADIUS.Length);
			}
			foreach (IntVector2 b in array)
			{
				IntVector2 intVector = gem.position + b;
				if (fields.IsValid(intVector) && fields[intVector].isOn)
				{
					this.HighlightPositions.Add(intVector);
					if (fields[intVector].CanExplode && fields[intVector].HasGem)
					{
						Gem item = fields[intVector].gem;
						if (!fields[intVector].GemBlocked && item.IsAffectedBySuperGems)
						{
							this.Group.Add(item);
						}
						else
						{
							this.Fields.Add(intVector);
						}
					}
					else if (!fields[intVector].isWindow && !fields[intVector].IsColorWheel)
					{
						this.Fields.Add(intVector);
					}
				}
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x000ACF9F File Offset: 0x000AB39F
		public Group Group
		{
			get
			{
				return this.group;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x000ACFA7 File Offset: 0x000AB3A7
		public IntVector2 From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x000ACFAF File Offset: 0x000AB3AF
		public IntVector2 Center
		{
			get
			{
				return this.gem.position;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x000ACFBC File Offset: 0x000AB3BC
		public List<IntVector2> Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600268A RID: 9866 RVA: 0x000ACFC4 File Offset: 0x000AB3C4
		public List<IntVector2> HighlightPositions
		{
			get
			{
				return this.highlightPositions;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x000ACFCC File Offset: 0x000AB3CC
		public bool ShouldHitAdjacentFields
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04005160 RID: 20832
		public Gem gem;

		// Token: 0x04005161 RID: 20833
		public readonly bool isSuperBomb;

		// Token: 0x04005162 RID: 20834
		private readonly Group group;

		// Token: 0x04005163 RID: 20835
		private readonly IntVector2 from;

		// Token: 0x04005164 RID: 20836
		private readonly List<IntVector2> fields;

		// Token: 0x04005165 RID: 20837
		private readonly List<IntVector2> highlightPositions;

		// Token: 0x04005166 RID: 20838
		private static readonly IntVector2[] BOMB_RADIUS = new IntVector2[]
		{
			IntVector2.Left + IntVector2.Down,
			IntVector2.Down,
			IntVector2.Right + IntVector2.Down,
			IntVector2.Left,
			IntVector2.Right,
			IntVector2.Left + IntVector2.Up,
			IntVector2.Up,
			IntVector2.Right + IntVector2.Up
		};

		// Token: 0x04005167 RID: 20839
		private static readonly IntVector2[] SUPER_BOMB_RADIUS = new IntVector2[]
		{
			IntVector2.Left * 2 + IntVector2.Down * 2,
			IntVector2.Left + IntVector2.Down * 2,
			IntVector2.Down * 2,
			IntVector2.Right + IntVector2.Down * 2,
			IntVector2.Right * 2 + IntVector2.Down * 2,
			IntVector2.Left * 2 + IntVector2.Down,
			IntVector2.Left * 2,
			IntVector2.Left * 2 + IntVector2.Up,
			IntVector2.Right * 2 + IntVector2.Down,
			IntVector2.Right * 2,
			IntVector2.Right * 2 + IntVector2.Up,
			IntVector2.Left * 2 + IntVector2.Up * 2,
			IntVector2.Left + IntVector2.Up * 2,
			IntVector2.Up * 2,
			IntVector2.Right + IntVector2.Up * 2,
			IntVector2.Right * 2 + IntVector2.Up * 2
		};
	}
}
