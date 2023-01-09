using System;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.StateMachine;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.PlayerData;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006BE RID: 1726
	public class InputDispatcher : AStateMachine<AInputDispatcherState>, IInputDispatcherState
	{
		// Token: 0x06002AFF RID: 11007 RVA: 0x000C48C8 File Offset: 0x000C2CC8
		public InputDispatcher(PTMatchEngine engine, BoostFactory boostFactory, GameStateService gameState, AudioService audioService, BoostsUiRoot boostsUiRoot, Signal onAnimationFinished, Signal onLastHurray, LevelConfig config)
		{
			base.AddState(new IDSWaitForSwap(engine));
			IDSWaitForBoost idswaitForBoost = new IDSWaitForBoost(engine, boostFactory, gameState, audioService, config);
			idswaitForBoost.onBoostUsed.AddListener(new Action<Boosts>(engine.HandleBoostUsed));
			idswaitForBoost.onFinished.AddListener(delegate(AStateMachine<AInputDispatcherState>.AState s)
			{
				base.SetState<IDSWaitForSwap>();
			});
			onLastHurray.AddListener(new Action(this.HandleLastHurray));
			base.AddState(idswaitForBoost);
			base.AddState(new IDSBlock(engine, onAnimationFinished));
			base.AddState(new IDSLastHurray(engine));
			base.CurrentState = base.GetState<IDSWaitForSwap>();
			this.boostsUiRoot = boostsUiRoot;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000C4968 File Offset: 0x000C2D68
		public void HandleSwapped(Move move)
		{
			base.CurrentState.HandleSwapped(move);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000C4976 File Offset: 0x000C2D76
		public void HandleClicked(IntVector2 position)
		{
			base.CurrentState.HandleClicked(position);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000C4984 File Offset: 0x000C2D84
		public void HandleBoostSelected(BoostViewData data)
		{
			base.CurrentState.HandleBoostSelected(data);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000C4992 File Offset: 0x000C2D92
		public void HandleBoostAdded(BoostViewData data)
		{
			base.CurrentState.HandleBoostAdded(data, this.boostsUiRoot);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000C49A6 File Offset: 0x000C2DA6
		private void HandleLastHurray()
		{
			base.CurrentState = base.GetState<IDSLastHurray>();
		}

		// Token: 0x04005441 RID: 21569
		protected BoostsUiRoot boostsUiRoot;
	}
}
