using Match3.Scripts1.Audio;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Shared.M3Engine;
using Match3.Scripts1.Wooga.Signals;
using Match3.Scripts2.Android.DataStatistics;
using Match3.Scripts2.PlayerData; // using Firebase.Analytics;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006C2 RID: 1730
	public class IDSWaitForBoost : AInputDispatcherState
	{
		// Token: 0x06002B14 RID: 11028 RVA: 0x000C4B3C File Offset: 0x000C2F3C
		public IDSWaitForBoost(PTMatchEngine engine, BoostFactory boostFactory, GameStateService gameState, AudioService audioService, LevelConfig config) : base(engine)
		{
			this.gameState = gameState;
			this.boostFactory = boostFactory;
			this.audioService = audioService;
			this.config = config;
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000C4B66 File Offset: 0x000C2F66
		// (set) Token: 0x06002B16 RID: 11030 RVA: 0x000C4B6E File Offset: 0x000C2F6E
		public BoostViewData BoostInfo { get; set; }

		// Token: 0x06002B17 RID: 11031 RVA: 0x000C4B77 File Offset: 0x000C2F77
		public override void HandleSwapped(Move move)
		{
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000C4B7C File Offset: 0x000C2F7C
		public override void HandleClicked(IntVector2 position)
		{
			IBoost boost = this.boostFactory.GetBoost(this.BoostInfo.Type, position);
			if (!this.engine.IsResolvingMoves && boost.IsValid())
			{
				// buried point: 关卡中使用道具
				DataStatistics.Instance.TriggerUseBooster(config.Level.level, BoostInfo.Type.ToString());
				
				this.gameState.Resources.Pay(new MaterialAmount(this.BoostInfo.name, 1, MaterialAmountUsage.Undefined, 0));
				this.engine.AllowFreeSwapping = true;
				this.engine.ApplyBoost(boost);
				this.engine.AllowFreeSwapping = false;
				this.audioService.PlaySFX(AudioId.ReplaceGem, false, false, false);
				this.onBoostUsed.Dispatch(this.BoostInfo.Type);
				base.StateMachine.SetState<IDSBlock>();
				this.BoostInfo = null;
			}
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000C4C40 File Offset: 0x000C3040
		public override void HandleBoostSelected(BoostViewData data)
		{
			base.HandleBoostSelected(data);
			if (data.state != BoostState.Selected)
			{
				base.StateMachine.SetState<IDSWaitForSwap>();
				this.BoostInfo = null;
			}
		}

		// Token: 0x04005443 RID: 21571
		private BoostFactory boostFactory;

		// Token: 0x04005444 RID: 21572
		private GameStateService gameState;

		// Token: 0x04005445 RID: 21573
		private AudioService audioService;
		
		private LevelConfig config;

		// Token: 0x04005447 RID: 21575
		public Signal<Boosts> onBoostUsed = new Signal<Boosts>();
	}
}
