using Match3.Scripts1.Shared.StateMachine;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006BF RID: 1727
	public abstract class AInputDispatcherState : AStateMachine<AInputDispatcherState>.AState, IInputDispatcherState
	{
		// Token: 0x06002B06 RID: 11014 RVA: 0x000C49BC File Offset: 0x000C2DBC
		public AInputDispatcherState(PTMatchEngine engine)
		{
			this.engine = engine;
		}

		// Token: 0x06002B07 RID: 11015
		public abstract void HandleSwapped(Move move);

		// Token: 0x06002B08 RID: 11016
		public abstract void HandleClicked(IntVector2 position);

		// Token: 0x06002B09 RID: 11017 RVA: 0x000C49CB File Offset: 0x000C2DCB
		public virtual void HandleBoostSelected(BoostViewData data)
		{
			if (data.state == BoostState.Selected)
			{
				base.StateMachine.GetState<IDSWaitForBoost>().BoostInfo = data;
			}
			else
			{
				base.StateMachine.GetState<IDSWaitForBoost>().BoostInfo = null;
			}
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000C4A00 File Offset: 0x000C2E00
		public virtual void HandleBoostAdded(BoostViewData data, BoostsUiRoot boostsUiRoot)
		{
			BuyBoostFlow.Input input = new BuyBoostFlow.Input(data, "ingame");
			new BuyBoostFlow(boostsUiRoot).Start(input);
		}

		// Token: 0x04005442 RID: 21570
		protected PTMatchEngine engine;
	}
}
