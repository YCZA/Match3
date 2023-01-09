using Match3.Scripts1.Wooga.UI;
using UnityEngine.UI;

namespace Match3.Scripts1.Puzzletown.Match3
{
	// Token: 0x020006F9 RID: 1785
	public class M3LevelSelectionItemPlaceholder : AListItemPlaceholder<LevelMapActiveView, M3_LevelMapItemState>, IDataView<LevelSelectionData>
	{
		// Token: 0x06002C53 RID: 11347 RVA: 0x000CC238 File Offset: 0x000CA638
		public void Show(LevelSelectionData data)
		{
			this.HideAttachedGraphic();
			M3_LevelMapItemState itemState = data.itemState;
			if (itemState != M3_LevelMapItemState.None)
			{
				if (itemState != M3_LevelMapItemState.Separator)
				{
					base.LoadListItem(data.itemState).Show(data);
				}
				else
				{
					LevelMapActiveView levelMapActiveView = base.LoadListItem(data.itemState);
					ATableViewReusableCell atableViewReusableCell = base.GetComponentsInParent<ATableViewReusableCell>(true)[0];
					levelMapActiveView.transform.SetParent(atableViewReusableCell.transform, false);
					levelMapActiveView.Show(data);
				}
			}
			else
			{
				base.Clear();
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000CC2C0 File Offset: 0x000CA6C0
		private void HideAttachedGraphic()
		{
			Graphic component = base.GetComponent<Graphic>();
			if (component)
			{
				component.enabled = false;
			}
		}
	}
}
