using Match3.Scripts1.Town;
using Match3.Scripts1.Wooga.UI;

// Token: 0x02000965 RID: 2405
namespace Match3.Scripts1
{
	public abstract class ABuildingUiView : AOverheadUiView, IDataView<BuildingInstance>
	{
		// Token: 0x06003AA6 RID: 15014
		public abstract void Show(BuildingInstance building);
	}
}
