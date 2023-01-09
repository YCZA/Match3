using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Match3.Scripts1.Puzzletown.Match3;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A7F RID: 2687
	[CreateAssetMenu(fileName = "ScriptTutorialOpenLODStart", menuName = "Puzzletown/Tutorials/Create/TutorialOpenLODStart")]
	public class TutorialOpenLODStart : ATutorialScript
	{
		// Token: 0x0600402B RID: 16427 RVA: 0x00149FDC File Offset: 0x001483DC
		protected override IEnumerator ExecuteRoutine()
		{
			new CoreGameFlow().Start(new CoreGameFlow.Input(0, false, null, LevelPlayMode.LevelOfTheDay));
			M3_LevelOfDayStartRoot lodStartScreen = global::UnityEngine.Object.FindObjectOfType<M3_LevelOfDayStartRoot>();
			while (lodStartScreen == null)
			{
				yield return null;
				lodStartScreen = global::UnityEngine.Object.FindObjectOfType<M3_LevelOfDayStartRoot>();
			}
			float waitTime = 1.5f;
			Animation animation = lodStartScreen.GetComponentInChildren<Animation>();
			if (animation && animation.GetClip("OpenLevelOfTheDay") != null)
			{
				waitTime = animation["OpenLevelOfTheDay"].length + 0.5f;
			}
			else
			{
				WoogaDebug.LogWarning(new object[]
				{
					"Animation not found OpenLevelOfTheDay"
				});
			}
			yield return new WaitForSeconds(waitTime);
			yield break;
		}

		// Token: 0x040069D7 RID: 27095
		public const string OPEN_ANIMATION_NAME = "OpenLevelOfTheDay";
	}
}
