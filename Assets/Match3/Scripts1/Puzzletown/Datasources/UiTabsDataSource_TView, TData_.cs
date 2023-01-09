using Match3.Scripts1.Wooga.UI;

namespace Match3.Scripts1.Puzzletown.Datasources
{
	// Token: 0x02000A2F RID: 2607
	public class UiTabsDataSource<TView, TData> : ArrayDataSource<TView, TData> where TView : ATableViewReusableCell, IDataView<TData> where TData : IUiTabStateGetter
	{
		// Token: 0x06003EAE RID: 16046 RVA: 0x00136400 File Offset: 0x00134800
		public override int GetReusableIdForIndex(int index)
		{
			TData dataForIndex = this.GetDataForIndex(index);
			return (int)dataForIndex.GetTabState();
		}
	}
}
