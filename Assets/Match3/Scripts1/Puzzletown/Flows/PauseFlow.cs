using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Flows
{
	// Token: 0x020004C1 RID: 1217
	public class PauseFlow : IBlocker
	{
		// Token: 0x06002228 RID: 8744 RVA: 0x000942D7 File Offset: 0x000926D7
		public PauseFlow(float seconds)
		{
			this.m_seconds = seconds;
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x000942E6 File Offset: 0x000926E6
		public bool BlockInput
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000942EC File Offset: 0x000926EC
		public IEnumerator ExecuteRoutine()
		{
			Wooroutine<OverlayRoot> overlay = SceneManager.Instance.LoadScene<OverlayRoot>(null);
			yield return overlay;
			yield return new WaitForSeconds(this.m_seconds);
			overlay.ReturnValue.Destroy();
			yield break;
		}

		// Token: 0x04004D87 RID: 19847
		private readonly float m_seconds;
	}
}
