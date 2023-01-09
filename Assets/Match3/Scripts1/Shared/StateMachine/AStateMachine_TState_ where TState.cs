using System;
using System.Collections.Generic;
using Match3.Scripts1.Wooga.Signals;

namespace Match3.Scripts1.Shared.StateMachine
{
	// Token: 0x02000B25 RID: 2853
	public class AStateMachine<TState> where TState : AStateMachine<TState>.AState
	{
		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060042F0 RID: 17136 RVA: 0x000C47EF File Offset: 0x000C2BEF
		// (set) Token: 0x060042F1 RID: 17137 RVA: 0x000C47F7 File Offset: 0x000C2BF7
		public TState CurrentState { get; protected set; }

		// Token: 0x060042F2 RID: 17138 RVA: 0x000C4800 File Offset: 0x000C2C00
		public void SetState<T>() where T : TState
		{
			this.CurrentState = this.states[typeof(T)];
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x000C481D File Offset: 0x000C2C1D
		public T GetState<T>() where T : TState
		{
			return (T)((object)this.states[typeof(T)]);
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x000C483E File Offset: 0x000C2C3E
		protected void AddState(TState state)
		{
			state.StateMachine = this;
			this.states[state.GetType()] = state;
		}

		// Token: 0x04006BA3 RID: 27555
		private Dictionary<Type, TState> states = new Dictionary<Type, TState>();

		// Token: 0x02000B26 RID: 2854
		public abstract class AState
		{
			// Token: 0x170009A6 RID: 2470
			// (get) Token: 0x060042F6 RID: 17142 RVA: 0x000C4885 File Offset: 0x000C2C85
			// (set) Token: 0x060042F7 RID: 17143 RVA: 0x000C488D File Offset: 0x000C2C8D
			public AStateMachine<TState> StateMachine { get; set; }

			// Token: 0x060042F8 RID: 17144 RVA: 0x000C4896 File Offset: 0x000C2C96
			public bool IsActive()
			{
				return this.StateMachine.CurrentState == this;
			}

			// Token: 0x060042F9 RID: 17145 RVA: 0x000C48AB File Offset: 0x000C2CAB
			protected void Finish()
			{
				this.onFinished.Dispatch(this);
			}

			// Token: 0x060042FA RID: 17146 RVA: 0x000C48B9 File Offset: 0x000C2CB9
			protected void Start()
			{
				this.onStarted.Dispatch(this);
			}

			// Token: 0x04006BA5 RID: 27557
			public readonly Signal<AStateMachine<TState>.AState> onFinished = new Signal<AStateMachine<TState>.AState>();

			// Token: 0x04006BA6 RID: 27558
			public readonly Signal<AStateMachine<TState>.AState> onStarted = new Signal<AStateMachine<TState>.AState>();
		}
	}
}
