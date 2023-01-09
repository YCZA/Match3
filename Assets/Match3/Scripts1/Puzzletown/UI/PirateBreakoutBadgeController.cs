using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F6 RID: 2550
	[RequireComponent(typeof(PirateBreakoutBadgeUi))]
	public class PirateBreakoutBadgeController : MonoBehaviour
	{
		// Token: 0x06003D7D RID: 15741 RVA: 0x00136760 File Offset: 0x00134B60
		public void Init(TownBottomPanelRoot townBottomPanel)
		{
			WooroutineRunner.StartCoroutine(this.badge.SetupRoutine(townBottomPanel), null);
		}

		// Token: 0x0400664A RID: 26186
		public PirateBreakoutBadgeUi badge;
	}
}
