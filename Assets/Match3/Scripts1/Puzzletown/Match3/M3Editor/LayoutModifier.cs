namespace Match3.Scripts1.Puzzletown.Match3.M3Editor
{
	// Token: 0x02000563 RID: 1379
	public static class LayoutModifier
	{
		// Token: 0x06002437 RID: 9271 RVA: 0x000A10E9 File Offset: 0x0009F4E9
		public static IntVector2 CWRotatedPos(IntVector2 pos, int size)
		{
			return new IntVector2(pos.y, size - pos.x - 1);
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000A1102 File Offset: 0x0009F502
		public static IntVector2 CCWRotatedPos(IntVector2 pos, int size)
		{
			return new IntVector2(size - pos.y - 1, pos.x);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000A111B File Offset: 0x0009F51B
		public static IntVector2 LeftShiftedPos(IntVector2 pos, int size)
		{
			return LayoutModifier.ShiftedPos(pos, size, IntVector2.Left);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000A1129 File Offset: 0x0009F529
		public static IntVector2 RightShiftedPos(IntVector2 pos, int size)
		{
			return LayoutModifier.ShiftedPos(pos, size, IntVector2.Right);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000A1137 File Offset: 0x0009F537
		public static IntVector2 UpShiftedPos(IntVector2 pos, int size)
		{
			return LayoutModifier.ShiftedPos(pos, size, IntVector2.Up);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000A1145 File Offset: 0x0009F545
		public static IntVector2 DownShiftedPos(IntVector2 pos, int size)
		{
			return LayoutModifier.ShiftedPos(pos, size, IntVector2.Down);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000A1153 File Offset: 0x0009F553
		private static IntVector2 ShiftedPos(IntVector2 pos, int size, IntVector2 shift)
		{
			return new IntVector2((pos.x + size + shift.x) % size, (pos.y + size + shift.y) % size);
		}
	}
}
