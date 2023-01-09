using System.Collections;
using System.Linq;
using Match3.Scripts1.Puzzletown.Services;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A74 RID: 2676
	[CreateAssetMenu(fileName = "ScriptTutorialClearRubble", menuName = "Puzzletown/Tutorials/Create/TutorialClearRubble")]
	public class TutorialClearRubble : ATutorialScript
	{
		// Token: 0x0600400B RID: 16395 RVA: 0x00148978 File Offset: 0x00146D78
		protected override IEnumerator ExecuteRoutine()
		{
			TownMainRoot town = global::UnityEngine.Object.FindObjectOfType<TownMainRoot>();
			BuildingsController bc = town.BuildingsController;
			BuildingInstance building = bc.Buildings.First((BuildingInstance b) => b.blueprint.rubble_id == 1);
			TutorialOverlayRoot overlay = global::UnityEngine.Object.FindObjectOfType<TutorialOverlayRoot>();
			while (BlockerManager.global.HasBlockers)
			{
				yield return null;
			}
			yield return overlay.PanToPosition(building.view.FocusPosition);
			yield break;
		}

		// Token: 0x040069C2 RID: 27074
		public string clearRubbleQuestId;
	}
}
