using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A78 RID: 2680
	[CreateAssetMenu(fileName = "ScriptCloseQuestWindow", menuName = "Puzzletown/Tutorials/Create/CloseQuestWindow")]
	public class TutorialCloseQuestWindow : ATutorialScript
	{
		// Token: 0x06004013 RID: 16403 RVA: 0x00148F84 File Offset: 0x00147384
		protected override IEnumerator ExecuteRoutine()
		{
			QuestsPopupRoot[] questPopupRoots = global::UnityEngine.Object.FindObjectsOfType<QuestsPopupRoot>();
			if (questPopupRoots != null)
			{
				foreach (QuestsPopupRoot questsPopupRoot in questPopupRoots)
				{
					questsPopupRoot.Close(QuestTaskType.unknown, false);
				}
			}
			yield return null;
			yield break;
		}
	}
}
