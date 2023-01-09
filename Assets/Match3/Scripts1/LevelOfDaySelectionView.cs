using System.Collections.Generic;
using Match3.Scripts1.UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020006E4 RID: 1764
namespace Match3.Scripts1
{
	public class LevelOfDaySelectionView : MonoBehaviour
	{
		// Token: 0x06002BCC RID: 11212 RVA: 0x000C90D6 File Offset: 0x000C74D6
		private void Awake()
		{
			if (this.selectButton != null)
			{
				this.selectButton.onClick.AddListener(new UnityAction(this.HandleSelect));
			}
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000C9105 File Offset: 0x000C7505
		private void HandleSelect()
		{
			this.HandleOnParent(PopupOperation.OK);
		}

		// Token: 0x040054E9 RID: 21737
		public TextMeshProUGUI dayNumberLabel;

		// Token: 0x040054EA RID: 21738
		public List<MaterialAmountView> materialAmountViews;

		// Token: 0x040054EB RID: 21739
		public Button selectButton;
	}
}
