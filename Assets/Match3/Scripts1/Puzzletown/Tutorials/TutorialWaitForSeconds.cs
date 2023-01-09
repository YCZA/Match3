using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A8C RID: 2700
	[CreateAssetMenu(fileName = "ScriptWaitForSeconds", menuName = "Puzzletown/Tutorials/Create/WaitForSeconds")]
	public class TutorialWaitForSeconds : ATutorialScript
	{
		// Token: 0x06004045 RID: 16453 RVA: 0x0014B254 File Offset: 0x00149654
		protected override IEnumerator ExecuteRoutine()
		{
			yield return new WaitForSeconds(this.waitTime);
			yield break;
		}

		// Token: 0x040069F0 RID: 27120
		public float waitTime;
	}
}
