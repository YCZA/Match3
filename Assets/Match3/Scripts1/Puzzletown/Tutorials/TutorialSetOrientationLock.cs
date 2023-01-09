using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A84 RID: 2692
	[CreateAssetMenu(fileName = "ScriptTutorialSetOrientationLock", menuName = "Puzzletown/Tutorials/Create/TutorialSetOrientationLock")]
	public class TutorialSetOrientationLock : ATutorialScript
	{
		// Token: 0x06004035 RID: 16437 RVA: 0x0014A7EC File Offset: 0x00148BEC
		protected override IEnumerator ExecuteRoutine()
		{
			if (this.isLocked)
			{
				yield return AUiAdjuster.ForceScreenSimilarOrientation(this.screenOrientation);
			}
			else
			{
				AUiAdjuster.SetOrientationLock(false);
			}
			yield break;
		}

		// Token: 0x040069E3 RID: 27107
		public bool isLocked;

		// Token: 0x040069E4 RID: 27108
		public ScreenOrientation screenOrientation;
	}
}
