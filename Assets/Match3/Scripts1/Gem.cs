using System;
using System.Collections.Generic;

// Token: 0x02000599 RID: 1433
namespace Match3.Scripts1
{
	[Serializable]
	public struct Gem : IEquatable<Gem>
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x000A6D3C File Offset: 0x000A513C
		public Gem(GemColor color)
		{
			this.color = color;
			this.type = GemType.Undefined;
			this.modifier = GemModifier.Undefined;
			this.direction = GemDirection.Undefined;
			this.position = IntVector2.Zero;
			this.lastPortalUsed = -1;
			this.parameter = 0;
			this.processed = false;
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x000A6D7A File Offset: 0x000A517A
		public bool IsMatchable
		{
			get
			{
				return !this.IsSpecialColor && this.color != GemColor.Undefined;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x0600256D RID: 9581 RVA: 0x000A6D96 File Offset: 0x000A5196
		public bool IsAffectedBySuperGems
		{
			get
			{
				return this.IsMatchable || this.color == GemColor.Cannonball;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x000A6DB0 File Offset: 0x000A51B0
		public bool CanShuffle
		{
			get
			{
				return this.IsMatchable && this.type == GemType.Undefined && this.modifier == GemModifier.Undefined;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000A6DD4 File Offset: 0x000A51D4
		public bool CanTrickle
		{
			get
			{
				return !this.IsClimber && !this.IsCannon;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x000A6DED File Offset: 0x000A51ED
		public bool IsCovered
		{
			get
			{
				return this.modifier != GemModifier.Undefined;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002571 RID: 9585 RVA: 0x000A6DFB File Offset: 0x000A51FB
		public bool IsDroppable
		{
			get
			{
				return this.color == GemColor.Droppable;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x000A6E06 File Offset: 0x000A5206
		public bool IsClimber
		{
			get
			{
				return this.color == GemColor.Climber;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x000A6E12 File Offset: 0x000A5212
		public bool IsClimberGem
		{
			get
			{
				return this.type == GemType.ClimberGem;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x000A6E1D File Offset: 0x000A521D
		public bool IsRainbow
		{
			get
			{
				return this.color == GemColor.Rainbow;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x000A6E29 File Offset: 0x000A5229
		public bool CanNotBeCovered
		{
			get
			{
				return this.IsClimber || this.color == GemColor.Treasure || this.color == GemColor.Dirt;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x000A6E50 File Offset: 0x000A5250
		public bool IsStackedGem
		{
			get
			{
				return this.type == GemType.StackedGemBig || this.type == GemType.StackedGemMedium || this.type == GemType.StackedGemSmall;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x000A6E77 File Offset: 0x000A5277
		public bool IsCannon
		{
			get
			{
				return this.type == GemType.Cannon;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x000A6E83 File Offset: 0x000A5283
		public bool IsChameleon
		{
			get
			{
				return this.type == GemType.Chameleon || this.type == GemType.ChameleonReduced;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x000A6E9F File Offset: 0x000A529F
		public bool IsReducedColorChameleon
		{
			get
			{
				return this.type == GemType.ChameleonReduced;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x000A6EAB File Offset: 0x000A52AB
		public bool IsAllColorChameleon
		{
			get
			{
				return this.type == GemType.Chameleon;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x000A6EB7 File Offset: 0x000A52B7
		public bool IsCannonball
		{
			get
			{
				return this.color == GemColor.Cannonball;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x000A6EC3 File Offset: 0x000A52C3
		public bool IsSuperGem
		{
			get
			{
				return this.type == GemType.Bomb || this.type == GemType.LineHorizontal || this.type == GemType.LineVertical;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x000A6EE9 File Offset: 0x000A52E9
		public bool IsIced
		{
			get
			{
				return this.modifier == GemModifier.IceHp1 || this.modifier == GemModifier.IceHp2 || this.modifier == GemModifier.IceHp3;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x000A6F0F File Offset: 0x000A530F
		public bool IsCoveredByDirt
		{
			get
			{
				return this.modifier == GemModifier.DirtHp1 || this.modifier == GemModifier.DirtHp2 || this.modifier == GemModifier.DirtHp3;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x000A6F38 File Offset: 0x000A5338
		public bool IsSpecialColor
		{
			get
			{
				return this.color == GemColor.Rainbow || this.color == GemColor.Droppable || this.color == GemColor.Cannonball || this.color == GemColor.Dirt || this.color == GemColor.Treasure || this.IsClimber || this.IsCannon;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002580 RID: 9600 RVA: 0x000A6F99 File Offset: 0x000A5399
		public bool CanExplosionHit
		{
			get
			{
				return !this.IsCovered && !this.IsCannon;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000A6FB2 File Offset: 0x000A53B2
		public bool IsReplaceable()
		{
			return this.type == GemType.Undefined && !this.IsSpecialColor && this.modifier == GemModifier.Undefined;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000A6FD6 File Offset: 0x000A53D6
		public bool HasDirection()
		{
			return this.direction != GemDirection.Undefined;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000A6FE4 File Offset: 0x000A53E4
		public bool IsFirstColorInOrder()
		{
			return this.color == Gem.GEM_ORDER[0];
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000A6FF9 File Offset: 0x000A53F9
		public bool IsLastColorInOrder()
		{
			return this.color == Gem.GEM_ORDER[Gem.GEM_ORDER.Count - 1];
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000A701C File Offset: 0x000A541C
		public override string ToString()
		{
			return string.Format("[Gem]{0},{1},{2},{3},{4},{5}", new object[]
			{
				this.color,
				this.type,
				this.modifier,
				this.position,
				this.parameter,
				this.direction
			});
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000A7090 File Offset: 0x000A5490
		public bool Equals(Gem other)
		{
			return this.position.Equals(other.position) && this.color == other.color && this.type == other.type && this.direction == other.direction;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000A70EC File Offset: 0x000A54EC
		public static IntVector2 GemDirectionToVector(GemDirection gemDirection)
		{
			IntVector2 result = IntVector2.Left;
			switch (gemDirection)
			{
				case GemDirection.Left:
					result = IntVector2.Left;
					break;
				case GemDirection.Up:
					result = IntVector2.Up;
					break;
				case GemDirection.Right:
					result = IntVector2.Right;
					break;
				case GemDirection.Down:
					result = IntVector2.Down;
					break;
			}
			return result;
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000A714C File Offset: 0x000A554C
		public static GemDirection VectorToGemDirection(IntVector2 direction)
		{
			GemDirection result = GemDirection.Left;
			if (direction == IntVector2.Left)
			{
				result = GemDirection.Left;
			}
			if (direction == IntVector2.Up)
			{
				result = GemDirection.Up;
			}
			if (direction == IntVector2.Right)
			{
				result = GemDirection.Right;
			}
			if (direction == IntVector2.Down)
			{
				result = GemDirection.Down;
			}
			return result;
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000A71A4 File Offset: 0x000A55A4
		public static GemDirection GetRandomGemDirection()
		{
			int length = Enum.GetValues(typeof(GemDirection)).Length;
			return (GemDirection)RandomHelper.Next(1, length);
		}

		// Token: 0x040050A5 RID: 20645
		public const int NO_PORTAL_USED = -1;

		// Token: 0x040050A6 RID: 20646
		public static readonly List<GemColor> GEM_ORDER = new List<GemColor>
		{
			GemColor.Red,
			GemColor.Orange,
			GemColor.Purple,
			GemColor.Blue,
			GemColor.Green,
			GemColor.Yellow,
			GemColor.Coins
		};

		// Token: 0x040050A7 RID: 20647
		public const GemColor FIRST_COLOR_IN_GEM_ORDER = GemColor.Red;

		// Token: 0x040050A8 RID: 20648
		public const GemColor LAST_COLOR_IN_GEM_ORDER = GemColor.Coins;

		// Token: 0x040050A9 RID: 20649
		public IntVector2 position;

		// Token: 0x040050AA RID: 20650
		public GemColor color;

		// Token: 0x040050AB RID: 20651
		public GemType type;

		// Token: 0x040050AC RID: 20652
		public GemModifier modifier;

		// Token: 0x040050AD RID: 20653
		public GemDirection direction;

		// Token: 0x040050AE RID: 20654
		public bool processed;

		// Token: 0x040050AF RID: 20655
		public int lastPortalUsed;

		// Token: 0x040050B0 RID: 20656
		public int parameter;
	}
}
