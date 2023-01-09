using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B24 RID: 2852
namespace Match3.Scripts1
{
	public class EventSystemRoot : APtSceneRoot
	{
		// Token: 0x060042EE RID: 17134 RVA: 0x00156464 File Offset: 0x00154864
		private void Start()
		{
			float num = (Screen.dpi != 0f) ? Screen.dpi : 240f;
			this.eventSystem.pixelDragThreshold = (int)(0.15f * num);
		}

		// Token: 0x04006BA0 RID: 27552
		public const float DEFAULT_DPI = 240f;

		// Token: 0x04006BA1 RID: 27553
		private const float DRAG_THRESHOLD_IN_INCHES = 0.15f;

		// Token: 0x04006BA2 RID: 27554
		public EventSystem eventSystem;

		public static bool isUsedByTutorial = true;
	}
}
