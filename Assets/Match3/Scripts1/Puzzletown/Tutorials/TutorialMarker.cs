using UnityEngine;

namespace Match3.Scripts1.Puzzletown.Tutorials
{
	// Token: 0x02000AA1 RID: 2721
	public class TutorialMarker : MonoBehaviour
	{
		// Token: 0x060040AA RID: 16554 RVA: 0x0014F9BC File Offset: 0x0014DDBC
		private void OnEnable()
		{
			TutorialRunner tutorialRunner = global::UnityEngine.Object.FindObjectOfType<TutorialRunner>();
			if (tutorialRunner)
			{
				tutorialRunner.onMarkerEnabled.Dispatch(this.id);
			}
		}

		// Token: 0x04006A51 RID: 27217
		public string id;
	}
}
