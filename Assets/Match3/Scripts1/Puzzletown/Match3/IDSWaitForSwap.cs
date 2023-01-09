namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C1 RID: 1729
	public class IDSWaitForSwap : AInputDispatcherState
	{
		// Token: 0x06002B10 RID: 11024 RVA: 0x000C4A99 File Offset: 0x000C2E99
		public IDSWaitForSwap(PTMatchEngine engine) : base(engine)
		{
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000C4AA4 File Offset: 0x000C2EA4
		public override void HandleSwapped(Move move)
		{
			if (!this.engine.IsResolvingMoves && this.engine.Fields[move.from].HasGem && !this.engine.Fields[move.from].GemBlocked)
			{
				this.engine.ProcessMove(move);
				base.StateMachine.SetState<IDSBlock>();
			}
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000C4B1A File Offset: 0x000C2F1A
		public override void HandleClicked(IntVector2 position)
		{
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000C4B1C File Offset: 0x000C2F1C
		public override void HandleBoostSelected(BoostViewData data)
		{
			base.HandleBoostSelected(data);
			if (data.state == BoostState.Selected)
			{
				base.StateMachine.SetState<IDSWaitForBoost>();
			}
		}
	}
}
