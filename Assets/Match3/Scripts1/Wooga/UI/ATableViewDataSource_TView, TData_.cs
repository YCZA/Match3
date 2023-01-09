using System;
using System.Linq;
using Match3.Scripts1.Wooga.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B3C RID: 2876
	public abstract class ATableViewDataSource<TView, TData> : ATableViewReusableCell, ITableViewDataSource where TView : ATableViewReusableCell, IDataView<TData>
	{
		// Token: 0x06004371 RID: 17265
		public abstract TData GetDataForIndex(int index);

		// Token: 0x06004372 RID: 17266
		public abstract int GetNumberOfCellsForTableView();

		// Token: 0x06004373 RID: 17267
		public abstract int GetReusableIdForIndex(int index);

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06004374 RID: 17268 RVA: 0x000C7694 File Offset: 0x000C5A94
		public override int reusableId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x000C7697 File Offset: 0x000C5A97
		protected virtual void Awake()
		{
			this.InitIfNeeded();
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x000C76A0 File Offset: 0x000C5AA0
		protected void InitIfNeeded()
		{
			if (!this.isInitialized)
			{
				this.tableView.DataSource = this;
				this.tableView.Reload();
				Array.ForEach<TView>(this.prototypeCells, delegate(TView cell)
				{
					cell.gameObject.SetActive(false);
				});
				this.isInitialized = true;
			}
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x000C7700 File Offset: 0x000C5B00
		public ATableViewReusableCell GetCellForIndex(int index, Transform parent)
		{
			int reusableIdForIndex = this.GetReusableIdForIndex(index);
			TView tview = this.CreatePrototypeCell(reusableIdForIndex);
			if (tview)
			{
				tview.transform.SetParent(parent, false);
				tview.Show(this.GetDataForIndex(index));
				//eli key point 显示关卡按钮(应该不只是关卡按钮)
				tview.gameObject.SetActive(true);
				return tview;
			}
			return null;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x000C7770 File Offset: 0x000C5B70
		public LayoutElement GetLayoutElementForIndex(int index)
		{
			if (this.useOptimizedAcess)
			{
				if (this.__layouts == null)
				{
					this.__layouts = new LayoutElement[this.GetMaxReusableId() + 1];
					foreach (TView tview in this.prototypeCells)
					{
						this.__layouts[tview.reusableId] = tview.GetComponent<LayoutElement>();
					}
				}
				return this.__layouts[this.GetReusableIdForIndex(index)];
			}
			TView tview2 = this.FindPrototypeCell(this.GetReusableIdForIndex(index));
			return tview2.GetComponent<LayoutElement>();
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x000C7814 File Offset: 0x000C5C14
		private TView FindPrototypeCell(int reusableId)
		{
			if (this.useOptimizedAcess)
			{
				if (this.__views == null)
				{
					this.__views = new TView[this.GetMaxReusableId() + 1];
					foreach (TView tview in this.prototypeCells)
					{
						this.__views[tview.reusableId] = tview;
					}
				}
				return this.__views[reusableId];
			}
			return Array.Find<TView>(this.prototypeCells, (TView cell) => cell.reusableId == reusableId);
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x000C78BC File Offset: 0x000C5CBC
		private TView CreatePrototypeCell(int reusableId)
		{
			TView tview = this.tableView.GetReusableCell(reusableId) as TView;
			if (tview)
			{
				return tview;
			}
			TView tview2 = this.FindPrototypeCell(reusableId);
			if (tview2)
			{
				TView tview3 = global::UnityEngine.Object.Instantiate<TView>(tview2);
				this.onCellCreated.Dispatch(tview3);
				return tview3;
			}
			return (TView)((object)null);
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x000C7928 File Offset: 0x000C5D28
		private int GetMaxReusableId()
		{
			int num = this.prototypeCells.Max((TView p) => p.reusableId);
			if (num >= 256)
			{
				throw new Exception(string.Format("Maximum reusableId in {0} is higher than {1}", base.name, 256));
			}
			return num;
		}

		// Token: 0x04006BEA RID: 27626
		private const int MAX_REUSABLE_UD = 256;

		// Token: 0x04006BEB RID: 27627
		private TView[] __views;

		// Token: 0x04006BEC RID: 27628
		private LayoutElement[] __layouts;

		// Token: 0x04006BED RID: 27629
		public ATableView tableView;

		// Token: 0x04006BEE RID: 27630
		[Tooltip("This will assume max reusaebleId to be 256")]
		public bool useOptimizedAcess;

		// Token: 0x04006BEF RID: 27631
		public TView[] prototypeCells;

		// Token: 0x04006BF0 RID: 27632
		public readonly Signal<TView> onCellCreated = new Signal<TView>();

		// Token: 0x04006BF1 RID: 27633
		protected bool isInitialized;
	}
}
