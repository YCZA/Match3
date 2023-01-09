using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C8 RID: 1224
	public class SceneLoaderFlow<T, TParams> : IBlocker where T : APtSceneRoot<TParams>
	{
		// Token: 0x06002240 RID: 8768 RVA: 0x00095B3E File Offset: 0x00093F3E
		public SceneLoaderFlow(TParams param, bool waitForClearScreen = false, float delay = 0f)
		{
			this.m_param = param;
			this.m_waitForClearScreen = waitForClearScreen;
			this.m_delay = delay;
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x00095B5B File Offset: 0x00093F5B
		public bool BlockInput
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00095B60 File Offset: 0x00093F60
		public IEnumerator ExecuteRoutine()
		{
			if (this.m_delay > 0f)
			{
				yield return new WaitForSeconds(this.m_delay);
			}
			if (this.m_waitForClearScreen)
			{
				do
				{
					yield return null;
				}
				while (Doober.ActiveDoobers > 0);
			}
			SceneManager.Instance.Inject(this);
			bool eventSystemPreviouslyActive = this.eventSystemRoot.gameObject.activeSelf;
			Wooroutine<T> loader = SceneManager.Instance.LoadSceneWithParams<T, TParams>(this.m_param, null);
			yield return loader;
			this.eventSystemRoot.Enable();
			yield return loader.ReturnValue.onDestroyed;
			if (!eventSystemPreviouslyActive)
			{
				this.eventSystemRoot.Disable();
			}
		}

		// Token: 0x04004DA7 RID: 19879
		private readonly TParams m_param;

		// Token: 0x04004DA8 RID: 19880
		private readonly bool m_waitForClearScreen;

		// Token: 0x04004DA9 RID: 19881
		private readonly float m_delay;

		// Token: 0x04004DAA RID: 19882
		[WaitForRoot(false, false)]
		private EventSystemRoot eventSystemRoot;
	}
}
