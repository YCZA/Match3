using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000702 RID: 1794
namespace Match3.Scripts1
{
	public class UiScrollToRotation : MonoBehaviour
	{
		// Token: 0x06002C6F RID: 11375 RVA: 0x000CCB4C File Offset: 0x000CAF4C
		private void Update()
		{
			this.target.rotation = Quaternion.Euler(this.scrollRect.content.localPosition.y * 0.33f, 0f, 0f);
		}

		// Token: 0x040055B5 RID: 21941
		private const float SCROLL_SENSIVITY = 0.33f;

		// Token: 0x040055B6 RID: 21942
		public ScrollRect scrollRect;

		// Token: 0x040055B7 RID: 21943
		public Transform target;
	}
}
