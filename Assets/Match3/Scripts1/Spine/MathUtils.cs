using System;

namespace Match3.Scripts1.Spine
{
	// Token: 0x02000218 RID: 536
	public static class MathUtils
	{
		// Token: 0x06001068 RID: 4200 RVA: 0x00028320 File Offset: 0x00026720
		static MathUtils()
		{
			for (int i = 0; i < 16384; i++)
			{
				MathUtils.sin[i] = (float)Math.Sin((double)(((float)i + 0.5f) / 16384f * 6.2831855f));
			}
			for (int j = 0; j < 360; j += 90)
			{
				MathUtils.sin[(int)((float)j * 45.511112f) & 16383] = (float)Math.Sin((double)((float)j * 0.017453292f));
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x000283AF File Offset: 0x000267AF
		public static float Sin(float radians)
		{
			return MathUtils.sin[(int)(radians * 2607.5945f) & 16383];
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x000283C5 File Offset: 0x000267C5
		public static float Cos(float radians)
		{
			return MathUtils.sin[(int)((radians + 1.5707964f) * 2607.5945f) & 16383];
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x000283E1 File Offset: 0x000267E1
		public static float SinDeg(float degrees)
		{
			return MathUtils.sin[(int)(degrees * 45.511112f) & 16383];
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000283F7 File Offset: 0x000267F7
		public static float CosDeg(float degrees)
		{
			return MathUtils.sin[(int)((degrees + 90f) * 45.511112f) & 16383];
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00028414 File Offset: 0x00026814
		public static float Atan2(float y, float x)
		{
			if (x == 0f)
			{
				if (y > 0f)
				{
					return 1.5707964f;
				}
				if (y == 0f)
				{
					return 0f;
				}
				return -1.5707964f;
			}
			else
			{
				float num = y / x;
				float num2;
				if (Math.Abs(num) >= 1f)
				{
					num2 = 1.5707964f - num / (num * num + 0.28f);
					return (y >= 0f) ? num2 : (num2 - 3.1415927f);
				}
				num2 = num / (1f + 0.28f * num * num);
				if (x < 0f)
				{
					return num2 + ((y >= 0f) ? 3.1415927f : -3.1415927f);
				}
				return num2;
			}
		}

		// Token: 0x04004132 RID: 16690
		public const float PI = 3.1415927f;

		// Token: 0x04004133 RID: 16691
		public const float PI2 = 6.2831855f;

		// Token: 0x04004134 RID: 16692
		public const float radDeg = 57.295776f;

		// Token: 0x04004135 RID: 16693
		public const float degRad = 0.017453292f;

		// Token: 0x04004136 RID: 16694
		private const int SIN_BITS = 14;

		// Token: 0x04004137 RID: 16695
		private const int SIN_MASK = 16383;

		// Token: 0x04004138 RID: 16696
		private const int SIN_COUNT = 16384;

		// Token: 0x04004139 RID: 16697
		private const float radFull = 6.2831855f;

		// Token: 0x0400413A RID: 16698
		private const float degFull = 360f;

		// Token: 0x0400413B RID: 16699
		private const float radToIndex = 2607.5945f;

		// Token: 0x0400413C RID: 16700
		private const float degToIndex = 45.511112f;

		// Token: 0x0400413D RID: 16701
		private static float[] sin = new float[16384];
	}
}
