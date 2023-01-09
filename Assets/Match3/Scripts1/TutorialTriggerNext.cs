using System.Collections;
using Match3.Scripts1.Puzzletown.Tutorials;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "ScriptTriggerNextTutorial", menuName = "Puzzletown/Tutorials/Create/TriggerNextTutorial")]
	public class TutorialTriggerNext : ATutorialScript
	{
		// Token: 0x06004041 RID: 16449 RVA: 0x0014B07C File Offset: 0x0014947C
		protected override IEnumerator ExecuteRoutine()
		{
			TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
			tutorialRunner.PlayTutorial(this.id);
			yield break;
		}

		// Token: 0x040069EE RID: 27118
		public string id;
	}
}
