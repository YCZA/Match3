using System.Collections.Generic;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000877 RID: 2167
	public class PopupResourcesCell : APopupCell<IEnumerable<MaterialAmount>>
	{
		// Token: 0x0600355A RID: 13658 RVA: 0x00100330 File Offset: 0x000FE730
		public override void Show(IEnumerable<MaterialAmount> data)
		{
			this.dataSource.Show(data);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x0010033E File Offset: 0x000FE73E
		public override bool CanPresent(IEnumerable<MaterialAmount> data)
		{
			return true;
		}

		// Token: 0x04005D3E RID: 23870
		public MaterialsDataSource dataSource;
	}
}
