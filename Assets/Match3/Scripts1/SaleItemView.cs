using System.Collections.Generic;
using Match3.Scripts1.Puzzletown.Services;
using TMPro;
using UnityEngine;

// Token: 0x02000A1A RID: 2586
namespace Match3.Scripts1
{
	public class SaleItemView : MonoBehaviour
	{
		// Token: 0x06003E2F RID: 15919 RVA: 0x0013B510 File Offset: 0x00139910
		public void Show(Materials materials, ILocalizationService locaService)
		{
			bool flag = materials != null && materials.Count > 0;
			base.gameObject.SetActive(flag);
			if (!flag)
			{
				return;
			}
			bool active = materials.Count == 1 && materials[0].type == "UnlimitedLives";
			this.timeLabel.gameObject.SetActive(active);
			this.multiItem.SetActive(materials.Count > 1);
			if (materials.Count == 1)
			{
				this.singleItem.Show(materials[0]);
				this.itemName.text = materials[0].FormatName(locaService);
			}
			else if (materials.Count > 1)
			{
				this.singleItem.Hide();
				for (int i = 0; i < this.itemViews.Count; i++)
				{
					if (i < materials.Count)
					{
						this.itemViews[i].Show(materials[i]);
					}
					else
					{
						this.itemViews[i].Hide();
					}
				}
				this.itemName.text = locaService.GetText("ui.shared.material.boosts", new LocaParam[0]);
			}
		}

		// Token: 0x0400671A RID: 26394
		[SerializeField]
		private TextMeshProUGUI itemName;

		// Token: 0x0400671B RID: 26395
		[SerializeField]
		private MaterialAmountView singleItem;

		// Token: 0x0400671C RID: 26396
		[SerializeField]
		private TextMeshProUGUI timeLabel;

		// Token: 0x0400671D RID: 26397
		[Header("Multiple Items")]
		[SerializeField]
		private GameObject multiItem;

		// Token: 0x0400671E RID: 26398
		[SerializeField]
		private List<MaterialAmountView> itemViews;
	}
}
