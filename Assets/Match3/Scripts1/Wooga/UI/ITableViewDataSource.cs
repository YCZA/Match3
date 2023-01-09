using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B40 RID: 2880
	public interface ITableViewDataSource
	{
		// Token: 0x06004380 RID: 17280
		int GetNumberOfCellsForTableView();

		// Token: 0x06004381 RID: 17281
		ATableViewReusableCell GetCellForIndex(int index, Transform parent);

		// Token: 0x06004382 RID: 17282
		LayoutElement GetLayoutElementForIndex(int index);
	}
}
