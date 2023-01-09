

// Token: 0x0200058B RID: 1419
namespace Match3.Scripts1
{
	public class Stone : AFieldModifier
	{
		// Token: 0x06002525 RID: 9509 RVA: 0x000A5988 File Offset: 0x000A3D88
		public Stone(int count) : base(count)
		{
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000A5991 File Offset: 0x000A3D91
		public static bool IsStone(int blockerIndex)
		{
			return blockerIndex > 0 && blockerIndex <= 5;
		}

		// Token: 0x0400508B RID: 20619
		public const int ZERO_HP_COUNT = 0;

		// Token: 0x0400508C RID: 20620
		public const int MAX_HP_COUNT = 5;
	}
}
