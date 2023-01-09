using System.Collections.Generic;
using System.Linq;
using Match3.Scripts1.Wooga.UI;

// Token: 0x02000853 RID: 2131
namespace Match3.Scripts1
{
	public class ArrayDataSource<TView, TData> : ATableViewDataSource<TView, TData>, IEnumerableView<TData>, IDataView<IEnumerable<TData>> where TView : ATableViewReusableCell, IDataView<TData>
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x000C79E0 File Offset: 0x000C5DE0
		public virtual void Cleanup()
		{
			this.dataArray = ArrayDataSource<TView, TData>.emptyArray;
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000C79ED File Offset: 0x000C5DED
		public override int GetNumberOfCellsForTableView()
		{
			return this.dataArray.Length;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000C79F7 File Offset: 0x000C5DF7
		public override TData GetDataForIndex(int index)
		{
			return this.dataArray[index];
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000C7A05 File Offset: 0x000C5E05
		public virtual void Show(IEnumerable<TData> feed)
		{
			base.InitIfNeeded();
			this.dataArray = ((!feed.IsNullOrEmptyEnumerable()) ? feed.ToArray<TData>() : ArrayDataSource<TView, TData>.emptyArray);
			this.tableView.Reload();
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000C7A39 File Offset: 0x000C5E39
		public override int GetReusableIdForIndex(int index)
		{
			return 0;
		}

		// Token: 0x04005CA1 RID: 23713
		private static readonly TData[] emptyArray = new TData[0];

		// Token: 0x04005CA2 RID: 23714
		protected TData[] dataArray = ArrayDataSource<TView, TData>.emptyArray;
	}
}
