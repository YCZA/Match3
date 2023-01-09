using System.Collections;
using Match3.Scripts1.Puzzletown.Tutorials;
using Match3.Scripts2.Building.Shop;
using UnityEngine;

// Token: 0x02000A73 RID: 2675
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "ScriptTutorialCancelConstruction", menuName = "Puzzletown/Tutorials/Create/TutorialCancelConstruction")]
	public class TutorialCancelConstruction : ATutorialScript
	{
		// Token: 0x06004009 RID: 16393 RVA: 0x0014887C File Offset: 0x00146C7C
		protected override IEnumerator ExecuteRoutine()
		{
			yield return new WaitForSeconds(0.45f);
			BuildingUiControlPanel buildingControlPanel = global::UnityEngine.Object.FindObjectOfType<BuildingUiControlPanel>();
			buildingControlPanel.Handle(BuildingOperation.Cancel);
			TownShopRoot shopUi = global::UnityEngine.Object.FindObjectOfType<TownShopRoot>();
			shopUi.Close();
			yield return new WaitForSeconds(0.25f);
			yield break;
		}
	}
}
