using UnityEngine;

// Token: 0x02000991 RID: 2449
namespace Match3.Scripts1
{
	[RequireComponent(typeof(RectTransform))]
	public class EnforeceUpdateOnRectTransform : MonoBehaviour
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x00127DC9 File Offset: 0x001261C9
		private void OnEnable()
		{
			this.UpdatePosition(this.rectTransform);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x00127DD8 File Offset: 0x001261D8
		private void UpdatePosition(RectTransform rect)
		{
			Vector3 position = rect.position;
			rect.position = new Vector3(position.x, position.y, 1E-05f);
			rect.position = position;
		}

		// Token: 0x0400638D RID: 25485
		public RectTransform rectTransform;
	}
}
