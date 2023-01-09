using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B37 RID: 2871
	public abstract class ATableView : MonoBehaviour
	{
		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x0600435B RID: 17243 RVA: 0x00158466 File Offset: 0x00156866
		// (set) Token: 0x0600435C RID: 17244 RVA: 0x0015846E File Offset: 0x0015686E
		public ITableViewDataSource DataSource { get; set; }

		// Token: 0x0600435D RID: 17245
		public abstract void Reload();

		// Token: 0x0600435E RID: 17246
		public abstract IEnumerable<ATableViewReusableCell> GetVisibleCells(RectTransform viewport, RectTransform slider);

		// Token: 0x0600435F RID: 17247
		public abstract IEnumerable<ATableViewReusableCell> GetAllCells();

		// Token: 0x06004360 RID: 17248 RVA: 0x00158478 File Offset: 0x00156878
		public ATableViewReusableCell GetReusableCell(int id)
		{
			if (this.reusableCells.ContainsKey(id) && this.reusableCells[id].Count > 0)
			{
				ATableViewReusableCell result = this.reusableCells[id][this.reusableCells[id].Count - 1];
				this.reusableCells[id].RemoveAt(this.reusableCells[id].Count - 1);
				return result;
			}
			return null;
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x001584FC File Offset: 0x001568FC
		protected void AddReusableCellToPool(ATableViewReusableCell cell)
		{
			if (!this.reusableCells.ContainsKey(cell.reusableId))
			{
				this.reusableCells[cell.reusableId] = new List<ATableViewReusableCell>();
			}
			this.reusableCells[cell.reusableId].Add(cell);
		}

		// Token: 0x04006BE4 RID: 27620
		private Dictionary<int, List<ATableViewReusableCell>> reusableCells = new Dictionary<int, List<ATableViewReusableCell>>();
	}
}
