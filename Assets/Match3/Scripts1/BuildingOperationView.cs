using Match3.Scripts1.Wooga.UI;

// Token: 0x0200098D RID: 2445
namespace Match3.Scripts1
{
	public class BuildingOperationView : ATableViewReusableCell, IDataView<BuildingOperation>
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x001278E5 File Offset: 0x00125CE5
		public void Show(BuildingOperation op)
		{
			this.button.operation = op;
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x001278F3 File Offset: 0x00125CF3
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04006385 RID: 25477
		public BuildingControlButton button;
	}
}
