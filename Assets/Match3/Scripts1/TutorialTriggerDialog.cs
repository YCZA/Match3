using System.Collections;
using Match3.Scripts1.Puzzletown.Town;
using Match3.Scripts1.Puzzletown.Tutorials;
using UnityEngine;

// Token: 0x02000A94 RID: 2708
namespace Match3.Scripts1
{
	[CreateAssetMenu(fileName = "ScriptTutorialTriggerDialog", menuName = "Puzzletown/Tutorials/Create/TutorialTriggerDialog")]
	public class TutorialTriggerDialog : ATutorialScript
	{
		// Token: 0x0600407F RID: 16511 RVA: 0x0014E18C File Offset: 0x0014C58C
		protected override IEnumerator ExecuteRoutine()
		{
			yield return new WaitForEndOfFrame();
			WoogaDebug.Log(new object[]
			{
				"trigger story dialog: ",
				this.dialogTrigger
			});
			StoryController storyController = global::UnityEngine.Object.FindObjectOfType<StoryController>();
			while (storyController == null)
			{
				yield return new WaitForEndOfFrame();
				storyController = global::UnityEngine.Object.FindObjectOfType<StoryController>();
			}
			Debug.Log("Tutorial: ");
			yield return storyController.PlayStoryDialogImmediate(DialogueTrigger.tutorial, this.dialogTrigger);
			yield break;
		}

		// Token: 0x04006A25 RID: 27173
		public string dialogTrigger;
	}
}
