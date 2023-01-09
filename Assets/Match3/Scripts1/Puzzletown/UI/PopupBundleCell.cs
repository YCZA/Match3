using System.Linq;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x02000861 RID: 2145
	public class PopupBundleCell : APopupCell<Bundle>
	{
		// Token: 0x060034FA RID: 13562 RVA: 0x000FE3C6 File Offset: 0x000FC7C6
		public override void Show(Bundle data)
		{
			this.dataSource.Show(data.materials);
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000FE3D9 File Offset: 0x000FC7D9
		public override bool CanPresent(Bundle data)
		{
			return data.materials.Count<MaterialAmount>() > 4;
		}

		// Token: 0x04005CEC RID: 23788
		public MaterialsDataSource dataSource;
	}
}
