

// Token: 0x0200058D RID: 1421
namespace Match3.Scripts1
{
	public class ColorWheel : AFieldModifier
	{
		// Token: 0x0600252A RID: 9514 RVA: 0x000A59D4 File Offset: 0x000A3DD4
		public ColorWheel(int index) : base(index)
		{
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000A59DD File Offset: 0x000A3DDD
		public static bool IsColorWheel(int blockerIndex)
		{
			return blockerIndex >= 9 && blockerIndex <= 12;
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000A59F2 File Offset: 0x000A3DF2
		public static bool IsColorWheelCorner(int blockerIndex)
		{
			return blockerIndex == 9 || blockerIndex == 11;
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000A5A04 File Offset: 0x000A3E04
		public static bool IsColorWheelVariant(int blockerIndex)
		{
			return blockerIndex == 11 || blockerIndex == 12;
		}

		// Token: 0x0400508F RID: 20623
		public const int SIZE = 2;

		// Token: 0x04005090 RID: 20624
		public const int COLOR_WHEEL_NORMAL_CORNER_ID = 9;

		// Token: 0x04005091 RID: 20625
		public const int COLOR_WHEEL_NORMAL_ID = 10;

		// Token: 0x04005092 RID: 20626
		public const int COLOR_WHEEL_VARIATION_CORNER_ID = 11;

		// Token: 0x04005093 RID: 20627
		public const int COLOR_WHEEL_VARIATION_ID = 12;
	}
}
