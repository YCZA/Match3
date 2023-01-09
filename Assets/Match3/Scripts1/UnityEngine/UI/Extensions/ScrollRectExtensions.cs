using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C2B RID: 3115
	public static class ScrollRectExtensions
	{
		// Token: 0x0600497C RID: 18812 RVA: 0x00177A06 File Offset: 0x00175E06
		public static void ScrollToTop(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00177A1D File Offset: 0x00175E1D
		public static void ScrollToBottom(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 0f);
		}
	}
}
