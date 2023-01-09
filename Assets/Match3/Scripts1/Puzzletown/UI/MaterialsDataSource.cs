using System;
using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020006D2 RID: 1746
	public class MaterialsDataSource : ArrayDataSource<MaterialAmountView, MaterialAmount>
	{
		// Token: 0x06002B7A RID: 11130 RVA: 0x000C7A5C File Offset: 0x000C5E5C
		public override int GetReusableIdForIndex(int index)
		{
			MaterialAmount dataForIndex = this.GetDataForIndex(index);
			foreach (MaterialAmountView materialAmountView in this.prototypeCells)
			{
				if (materialAmountView.manager.GetSimilar(dataForIndex.type, false))
				{
					return materialAmountView.reusableId;
				}
			}
			return (this.prototypeCells.Length <= 0) ? 0 : this.prototypeCells[0].reusableId;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000C7AD5 File Offset: 0x000C5ED5
		public override void Show(IEnumerable<MaterialAmount> materials)
		{
			base.Show(materials);
			this.isInitialized = true;
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000C7AE8 File Offset: 0x000C5EE8
		public void ShowOrRefreshExisting(IEnumerable<MaterialAmount> materials)
		{
			if (!this.isInitialized)
			{
				this.Show(materials);
				return;
			}
			MaterialAmountView[] componentsInChildren = this.tableView.GetComponentsInChildren<MaterialAmountView>();
			using (IEnumerator<MaterialAmount> enumerator = materials.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MaterialAmount mat = enumerator.Current;
					MaterialAmountView materialAmountView = Array.Find<MaterialAmountView>(componentsInChildren, (MaterialAmountView v) => v.Data.type == mat.type);
					if (materialAmountView)
					{
						materialAmountView.Show(mat);
					}
					else
					{
						WoogaDebug.LogWarningFormatted("MaterialAmountView for {0} was not found in {1}", new object[]
						{
							mat.type,
							this.tableView.name
						});
					}
				}
			}
		}

		// Token: 0x04005495 RID: 21653
		private new bool isInitialized;
	}
}
