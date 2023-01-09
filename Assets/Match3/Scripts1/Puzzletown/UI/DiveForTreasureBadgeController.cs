using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009C6 RID: 2502
	[RequireComponent(typeof(DiveForTreasureBadgeUi))]
	public class DiveForTreasureBadgeController : MonoBehaviour
	{
		// Token: 0x06003CA0 RID: 15520 RVA: 0x0012FF5B File Offset: 0x0012E35B
		public void Init(TownBottomPanelRoot townBottomPanel)
		{
			WooroutineRunner.StartCoroutine(this.badge.SetupRoutine(townBottomPanel), null);
		}

		// Token: 0x04006559 RID: 25945
		public DiveForTreasureBadgeUi badge;
	}
}
