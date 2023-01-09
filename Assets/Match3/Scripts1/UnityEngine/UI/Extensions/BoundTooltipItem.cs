using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C12 RID: 3090
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Item")]
	public class BoundTooltipItem : MonoBehaviour
	{
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x001747B1 File Offset: 0x00172BB1
		public bool IsActive
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x001747BE File Offset: 0x00172BBE
		private void Awake()
		{
			BoundTooltipItem.instance = this;
			if (!this.TooltipText)
			{
				this.TooltipText = base.GetComponentInChildren<Text>();
			}
			this.HideTooltip();
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x001747E8 File Offset: 0x00172BE8
		public void ShowTooltip(string text, Vector3 pos)
		{
			if (this.TooltipText.text != text)
			{
				this.TooltipText.text = text;
			}
			base.transform.position = pos + this.ToolTipOffset;
			base.gameObject.SetActive(true);
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0017483A File Offset: 0x00172C3A
		public void HideTooltip()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x00174848 File Offset: 0x00172C48
		public static BoundTooltipItem Instance
		{
			get
			{
				if (BoundTooltipItem.instance == null)
				{
					BoundTooltipItem.instance = global::UnityEngine.Object.FindObjectOfType<BoundTooltipItem>();
				}
				return BoundTooltipItem.instance;
			}
		}

		// Token: 0x04006F74 RID: 28532
		public Text TooltipText;

		// Token: 0x04006F75 RID: 28533
		public Vector3 ToolTipOffset;

		// Token: 0x04006F76 RID: 28534
		private static BoundTooltipItem instance;
	}
}
