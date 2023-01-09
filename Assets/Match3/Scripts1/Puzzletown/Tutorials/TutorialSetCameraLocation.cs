using System.Collections;
using Match3.Scripts1.Puzzletown.Flows;
using Wooga.Coroutines;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A83 RID: 2691
	[CreateAssetMenu(fileName = "ScriptTutorialSetCameraLocation", menuName = "Puzzletown/Tutorials/Create/TutorialSetCameraLocation")]
	public class TutorialSetCameraLocation : ATutorialScript
	{
		// Token: 0x06004033 RID: 16435 RVA: 0x0014A5E4 File Offset: 0x001489E4
		protected override IEnumerator ExecuteRoutine()
		{
			Vector3 positionToPanTo = this.islandPosition;
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
			{
				positionToPanTo = this.islandPositionLandscape;
			}
			Vector3 target = positionToPanTo;
			float time = this.panTime / 2f;
			IEnumerator flow = new PanCameraFlow(target, -100f, time, false).ExecuteRoutine();
			WooroutineRunner.StartCoroutine(flow, null);
			float startZoom = CameraInputController.current.Zoom;
			float targetZoom = CameraInputController.current.zoomLimit.y;
			if (AUiAdjuster.SimilarOrientation == ScreenOrientation.LandscapeLeft)
			{
				targetZoom = this.landscapeZoom;
			}
			float timePassedSoFar = 0f;
			float percentComplete = 0f;
			while (percentComplete < 1f)
			{
				timePassedSoFar += Time.deltaTime;
				percentComplete = timePassedSoFar / this.panTime;
				CameraInputController.current.Zoom = Mathf.Lerp(startZoom, targetZoom, this.zoomCurve.Evaluate(percentComplete));
				yield return null;
			}
			yield return null;
			CameraInputController.current.Zoom = targetZoom;
			yield break;
		}

		// Token: 0x040069DE RID: 27102
		public AnimationCurve zoomCurve;

		// Token: 0x040069DF RID: 27103
		public Vector3 islandPosition;

		// Token: 0x040069E0 RID: 27104
		public Vector3 islandPositionLandscape;

		// Token: 0x040069E1 RID: 27105
		public float landscapeZoom;

		// Token: 0x040069E2 RID: 27106
		public float panTime;
	}
}
