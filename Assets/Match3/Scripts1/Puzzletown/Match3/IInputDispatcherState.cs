namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006BD RID: 1725
	public interface IInputDispatcherState
	{
		// Token: 0x06002AFC RID: 11004
		void HandleSwapped(Move move);

		// Token: 0x06002AFD RID: 11005
		void HandleClicked(IntVector2 position);

		// Token: 0x06002AFE RID: 11006
		void HandleBoostSelected(BoostViewData data);
	}
}
