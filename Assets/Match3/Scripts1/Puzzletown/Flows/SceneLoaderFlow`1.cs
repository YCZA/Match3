using System;
using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C7 RID: 1223
	public class SceneLoaderFlow<T> : IBlocker where T : APtSceneRoot
	{
		// Token: 0x0600223C RID: 8764 RVA: 0x000959F2 File Offset: 0x00093DF2
		public SceneLoaderFlow(bool waitForDestroy = true)
		{
			this.waitForDestroy = waitForDestroy;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00095A01 File Offset: 0x00093E01
		public SceneLoaderFlow(Action<T> OnLoaded, bool waitForDestroy = true)
		{
			this.waitForDestroy = waitForDestroy;
			this.m_OnLoaded = OnLoaded;
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x00095A17 File Offset: 0x00093E17
		public bool BlockInput
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00095A1C File Offset: 0x00093E1C
		public IEnumerator ExecuteRoutine()
		{
			Wooroutine<T> loader = SceneManager.Instance.LoadScene<T>(null);
			yield return loader;
			if (this.m_OnLoaded != null)
			{
				this.m_OnLoaded(loader.ReturnValue);
			}
			if (this.waitForDestroy)
			{
				yield return loader.ReturnValue.onDestroyed;
			}
			yield break;
		}

		// Token: 0x04004DA5 RID: 19877
		private bool waitForDestroy;

		// Token: 0x04004DA6 RID: 19878
		private Action<T> m_OnLoaded;
	}
}
