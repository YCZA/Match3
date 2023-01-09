

// Token: 0x0200058C RID: 1420
namespace Match3.Scripts1
{
	public class ResistantBlocker : AFieldModifier
	{
		// Token: 0x06002527 RID: 9511 RVA: 0x000A59A4 File Offset: 0x000A3DA4
		public ResistantBlocker(int index) : base(index)
		{
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000A59AD File Offset: 0x000A3DAD
		public static bool IsResistantBlocker(int blockerIndex)
		{
			return blockerIndex >= 6 && blockerIndex <= 8;
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000A59C0 File Offset: 0x000A3DC0
		public static int GetHp(int index)
		{
			return (index < 6) ? 0 : (index - 6 + 1);
		}

		// Token: 0x0400508D RID: 20621
		public const int MIN_HP_COUNT = 6;

		// Token: 0x0400508E RID: 20622
		public const int MAX_HP_COUNT = 8;
	}
}
