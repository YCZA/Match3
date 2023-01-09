using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.Wooga.Legal
{
	// Token: 0x02000B5B RID: 2907
	[RequireComponent(typeof(TMP_Text))]
	public class TextMeshProLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060043FB RID: 17403 RVA: 0x0015AB37 File Offset: 0x00158F37
		private void Start()
		{
			this.label = base.GetComponent<TMP_Text>();
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0015AB48 File Offset: 0x00158F48
		public void OnPointerClick(PointerEventData eventData)
		{
			int num = TMP_TextUtilities.FindIntersectingLink(this.label, global::UnityEngine.Input.mousePosition, null);
			if (num != -1)
			{
				TMP_LinkInfo tmp_LinkInfo = this.label.textInfo.linkInfo[num];
				if (TextMeshProLinkHandler.LinkHandler == null)
				{
					Application.OpenURL(tmp_LinkInfo.GetLinkID());
				}
				else
				{
					TextMeshProLinkHandler.LinkHandler(tmp_LinkInfo.GetLinkID());
				}
			}
		}

		// Token: 0x04006C53 RID: 27731
		public static Action<string> LinkHandler;

		// Token: 0x04006C54 RID: 27732
		private TMP_Text label;
	}
}
