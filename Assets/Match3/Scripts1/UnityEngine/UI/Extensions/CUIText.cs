using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BC9 RID: 3017
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Text")]
	public class CUIText : CUIGraphic
	{
		// Token: 0x060046D4 RID: 18132 RVA: 0x001683DD File Offset: 0x001667DD
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Text>();
			}
			base.ReportSet();
		}
	}
}
