using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7A RID: 2682
	[CreateAssetMenu(fileName = "ScriptTutorialDiveForTreasureShowReward", menuName = "Puzzletown/Tutorials/Create/TutorialDiveForTreasureShowReward")]
	public class TutorialDiveForTreasureShowReward : ATutorialScript
	{
		// Token: 0x06004018 RID: 16408 RVA: 0x0014933C File Offset: 0x0014773C
		protected override IEnumerator ExecuteRoutine()
		{
			DiveForTreasureRoot town = global::UnityEngine.Object.FindObjectOfType<DiveForTreasureRoot>();
			town.ShowBubbles();
			yield return null;
			yield break;
		}
	}
}
