using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Town;
using Wooga.UnityFramework;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A8B RID: 2699
	[CreateAssetMenu(fileName = "ScriptTutorialWaitForBuildingConfirm", menuName = "Puzzletown/Tutorials/Create/TutorialWaitForBuildingConfirm")]
	public class TutorialWaitForBuildingConfirm : ATutorialScript
	{
		// Token: 0x06004043 RID: 16451 RVA: 0x0014B100 File Offset: 0x00149500
		protected override IEnumerator ExecuteRoutine()
		{
			yield return SceneManager.Instance.Inject(this);
			this.overlay.maskClick.gameObject.SetActive(false);
			yield return this.overlay.onScreenClicked.Await();
			BuildingInstance instance = BuildingLocation.Selected;
			instance.Handle(BuildingOperation.Confirm);
			yield return this.townMain.BuildingsController.onBuildingComplete.Await<BuildingInstance>();
			yield break;
		}

		// Token: 0x040069EF RID: 27119
		[WaitForRoot(false, false)]
		private TownMainRoot townMain;
	}
}
