using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A86 RID: 2694
	[CreateAssetMenu(fileName = "ScriptSetVillagersLocked", menuName = "Puzzletown/Tutorials/Create/SetVillagersLocked")]
	public class TutorialSetVillagersLocked : ATutorialScript
	{
		// Token: 0x06004039 RID: 16441 RVA: 0x0014A9BC File Offset: 0x00148DBC
		protected override IEnumerator ExecuteRoutine()
		{
			VillagersControllerRoot villagersController = global::UnityEngine.Object.FindObjectOfType<VillagersControllerRoot>();
			foreach (VillagerView villagerView in villagersController.Villagers)
			{
				villagerView.isLocked = this.locked;
			}
			yield return null;
			yield break;
		}

		// Token: 0x040069E7 RID: 27111
		public bool locked;
	}
}
