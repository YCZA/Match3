using System.Collections;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A87 RID: 2695
	[CreateAssetMenu(fileName = "ScriptShowArrow", menuName = "Puzzletown/Tutorials/Create/ShowArrow")]
	public class TutorialShowArrow : ATutorialScript
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x0014AAE4 File Offset: 0x00148EE4
		protected override IEnumerator ExecuteRoutine()
		{
			this.overlay.backgroundImage.enabled = false;
			this.overlay.roundHighlight.gameObject.SetActive(false);
			this.overlay.maskClick.gameObject.SetActive(false);
			Button button = this.step.HighlightObject.GetComponent<Button>();
			TutorialArrow arrow = this.overlay.tutorialArrow;
			RectTransform highlight = this.overlay.roundHighlight.GetComponent<RectTransform>();
			global::UnityEngine.Object.Destroy(highlight.GetComponent<Image>());
			highlight.gameObject.SetActive(true);
			arrow.transform.SetParent(button.transform.parent);
			AwaitSignal wait = this.step.HighlightObject.GetComponent<Button>().onClick.Await();
			bool wasArrowDeactivated = false;
			while (wait.keepWaiting && arrow)
			{
				arrow.gameObject.SetActive(BuildingLocation.Selected == null);
				if (wasArrowDeactivated && arrow.gameObject.activeInHierarchy)
				{
					yield return null;
					arrow.Enable(ArrowPosition.top, highlight, button.transform.parent);
				}
				wasArrowDeactivated = !arrow.gameObject.activeSelf;
				yield return null;
			}
			if (arrow)
			{
				global::UnityEngine.Object.Destroy(arrow.gameObject);
			}
			yield break;
		}
	}
}
