using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.UI
{
	// Token: 0x020009F2 RID: 2546
	public class UiToggleGraphic : AVisibleGraphic, IDataView<UiToggleState>, IEditorDescription
	{
		// Token: 0x06003D6F RID: 15727 RVA: 0x0013658C File Offset: 0x0013498C
		public void Show(UiToggleState value)
		{
			base.SetVisibility(this.state == value);
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0013659D File Offset: 0x0013499D
		public string GetEditorDescription()
		{
			return this.state.ToString();
		}

		// Token: 0x04006644 RID: 26180
		public UiToggleState state;
	}
}
