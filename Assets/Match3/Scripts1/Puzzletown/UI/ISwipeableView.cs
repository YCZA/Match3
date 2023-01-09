namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000A6D RID: 2669
	public interface ISwipeableView
	{
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003FED RID: 16365
		SwipeableViewPosition currentPosition { get; }

		// Token: 0x06003FEE RID: 16366
		void AnimateView(RelativeDirection direction);
	}
}
