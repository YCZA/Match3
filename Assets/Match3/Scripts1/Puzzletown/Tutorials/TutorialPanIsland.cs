using System.Collections;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000A80 RID: 2688
	[CreateAssetMenu(fileName = "ScriptTutorialPanIsland", menuName = "Puzzletown/Tutorials/Create/TutorialPanIsland")]
	public class TutorialPanIsland : ATutorialScript
	{
		// Token: 0x0600402D RID: 16429 RVA: 0x0014A164 File Offset: 0x00148564
		protected override IEnumerator ExecuteRoutine()
		{
			// Debug.LogError("引导: 相机移动");
			CameraInputController.current.Zoom = CameraInputController.current.zoomLimit.y;
			yield return new WaitForSeconds(this.initalDelay);
			float timePassedSoFar = 0f;
			float percentComplete = 0f;
			while (percentComplete < 1f)
			{
				timePassedSoFar += Time.deltaTime;
				percentComplete = timePassedSoFar / this.zoomDuration;
				CameraInputController.current.Zoom = Mathf.Lerp(CameraInputController.current.zoomLimit.y, CameraInputController.current.CamDistancePreferred, this.zoomCurve.Evaluate(percentComplete));
				yield return null;
			}
			yield break;
		}

		// Token: 0x040069D8 RID: 27096
		public AnimationCurve zoomCurve;

		// Token: 0x040069D9 RID: 27097
		public float initalDelay;

		// Token: 0x040069DA RID: 27098
		public float zoomDuration;
	}
}
