using System;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C0 RID: 1728
	public class IDSBlock : AInputDispatcherState
	{
		// Token: 0x06002B0B RID: 11019 RVA: 0x000C4A26 File Offset: 0x000C2E26
		public IDSBlock(PTMatchEngine engine, Signal onAnimationFinished) : base(engine)
		{
			onAnimationFinished.AddListener(new Action(this.HandleAnimationFinished));
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000C4A44 File Offset: 0x000C2E44
		private void HandleAnimationFinished()
		{
			if (base.StateMachine.CurrentState != this)
			{
				return;
			}
			if (base.StateMachine.GetState<IDSWaitForBoost>().BoostInfo != null)
			{
				base.StateMachine.SetState<IDSWaitForBoost>();
			}
			else
			{
				base.StateMachine.SetState<IDSWaitForSwap>();
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000C4A93 File Offset: 0x000C2E93
		public override void HandleSwapped(Move move)
		{
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000C4A95 File Offset: 0x000C2E95
		public override void HandleClicked(IntVector2 position)
		{
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000C4A97 File Offset: 0x000C2E97
		public override void HandleBoostAdded(BoostViewData data, BoostsUiRoot boostsUiRoot)
		{
		}
	}
}
