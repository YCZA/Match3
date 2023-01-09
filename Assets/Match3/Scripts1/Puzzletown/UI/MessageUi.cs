using TMPro;
using UnityEngine;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000994 RID: 2452
	public class MessageUi : UiSimpleView<BuildingMessageType>, ICategorised<BuildingMessageType>
	{
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x00127ED3 File Offset: 0x001262D3
		// (set) Token: 0x06003B9D RID: 15261 RVA: 0x00127EE0 File Offset: 0x001262E0
		public string text
		{
			get
			{
				return this.label.text;
			}
			set
			{
				this.label.text = value;
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x00127EEE File Offset: 0x001262EE
		public BuildingMessageType GetCategory()
		{
			return this.state;
		}

		// Token: 0x04006397 RID: 25495
		[SerializeField]
		private TMP_Text label;
	}
}
