using System.Collections;
using Match3.Scripts1.Puzzletown.Services;
using Wooga.Coroutines;
using Wooga.UnityFramework;
using Match3.Scripts2.PlayerData;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A88 RID: 2696
	[CreateAssetMenu(fileName = "ScriptShowNameDialog", menuName = "Puzzletown/Tutorials/Create/ShowNameDialog")]
	public class TutorialShowNameDialog : ATutorialScript
	{
		// Token: 0x0600403D RID: 16445 RVA: 0x0014AD68 File Offset: 0x00149168
		protected override IEnumerator ExecuteRoutine()
		{
			TutorialOverlayRoot tutorialOverlay = global::UnityEngine.Object.FindObjectOfType<TutorialOverlayRoot>();
			if (tutorialOverlay != null)
			{
				tutorialOverlay.maskHighlight.gameObject.SetActive(false);
				tutorialOverlay.backgroundImage.color = tutorialOverlay.backgroundColor;
			}
			yield return ServiceLocator.Instance.Inject(this);
			Wooroutine<EnterNameDialogRoot> nameDialog = SceneManager.Instance.LoadScene<EnterNameDialogRoot>(null);
			yield return nameDialog;
			nameDialog.ReturnValue.backgroundImage.gameObject.SetActive(false);
			nameDialog.ReturnValue.nameInput.ActivateInputField();
			yield return nameDialog.ReturnValue.onDestroyed;
			this.trackingService.TrackFunnelEvent("037_tell_elsie_your_name_ok", 37, this.gameStateService.PlayerName);
			yield break;
		}

		// Token: 0x040069E8 RID: 27112
		[WaitForService(true, true)]
		private TrackingService trackingService;

		// Token: 0x040069E9 RID: 27113
		[WaitForService(true, true)]
		private GameStateService gameStateService;
	}
}
