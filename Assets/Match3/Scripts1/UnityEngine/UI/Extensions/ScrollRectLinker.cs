using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2C RID: 3116
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectLinker")]
	public class ScrollRectLinker : MonoBehaviour
	{
		// Token: 0x0600497F RID: 18815 RVA: 0x00177A43 File Offset: 0x00175E43
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			if (this.controllingScrollRect != null)
			{
				this.controllingScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.MirrorPos));
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00177A80 File Offset: 0x00175E80
		private void MirrorPos(Vector2 scrollPos)
		{
			if (this.clamp)
			{
				this.scrollRect.normalizedPosition = new Vector2(Mathf.Clamp01(scrollPos.x), Mathf.Clamp01(scrollPos.y));
			}
			else
			{
				this.scrollRect.normalizedPosition = scrollPos;
			}
		}

		// Token: 0x04006FD4 RID: 28628
		public bool clamp = true;

		// Token: 0x04006FD5 RID: 28629
		[SerializeField]
		private ScrollRect controllingScrollRect;

		// Token: 0x04006FD6 RID: 28630
		private ScrollRect scrollRect;
	}
}
