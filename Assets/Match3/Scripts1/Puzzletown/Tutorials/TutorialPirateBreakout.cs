using System.Collections;
using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.UI;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A81 RID: 2689
	[CreateAssetMenu(fileName = "ScriptTutorialPirateBreakout", menuName = "Puzzletown/Tutorials/Create/TutorialPirateBreakout")]
	public class TutorialPirateBreakout : ATutorialScript
	{
		// Token: 0x0600402F RID: 16431 RVA: 0x0014A2E4 File Offset: 0x001486E4
		protected override IEnumerator ExecuteRoutine()
		{
			PirateBreakoutRoot town = global::UnityEngine.Object.FindObjectOfType<PirateBreakoutRoot>();
			List<string> dialogue = new List<string>
			{
				"piratebreakout.tutorial.1",
				"piratebreakout.tutorial.2"
			};
			yield return town.ShowDialogueWithPending(10, dialogue, false);
			town.ScrollToLevel(1, 1f);
			yield return new WaitForSeconds(1f);
			yield break;
		}
	}
}
