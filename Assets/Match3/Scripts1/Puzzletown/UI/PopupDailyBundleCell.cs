using System.Linq;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000867 RID: 2151
	public class PopupDailyBundleCell : APopupCell<Bundle>
	{
		// Token: 0x06003514 RID: 13588 RVA: 0x000FE5DE File Offset: 0x000FC9DE
		public override void Show(Bundle data)
		{
			this.dataSource.Show(data.materials);
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000FE5F1 File Offset: 0x000FC9F1
		public override bool CanPresent(Bundle data)
		{
			return data.materials.Count<MaterialAmount>() <= 4;
		}

		// Token: 0x04005CF2 RID: 23794
		public MaterialsDataSource dataSource;
	}
}
