using System.Collections;
using Match3.Scripts1.Puzzletown.UI;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A72 RID: 2674
	[CreateAssetMenu(fileName = "ScriptTutorialCancelChallenge", menuName = "Puzzletown/Tutorials/Create/TutorialCancelChallenge")]
	public class TutorialCancelChallenge : ATutorialScript
	{
		// Token: 0x06004007 RID: 16391 RVA: 0x00148734 File Offset: 0x00146B34
		protected override IEnumerator ExecuteRoutine()
		{
			yield return ServiceLocator.Instance.Inject(this);
			ChallengeV2Root challengeRoot = global::UnityEngine.Object.FindObjectOfType<ChallengeV2Root>();
			if (challengeRoot != null && this.gameStateService.Challenges.CurrentChallenges != null && this.gameStateService.Challenges.CurrentChallenges.Count > 0)
			{
				TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
				if (tutorialRunner != null)
				{
					tutorialRunner.currentTutorial.Finish();
				}
			}
			yield return null;
			yield break;
		}

		// Token: 0x040069C1 RID: 27073
		[WaitForService(true, true)]
		private GameStateService gameStateService;
	}
}
