using System;
using System.Collections.Generic;
using Wooga.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B43 RID: 2883
	[RequireComponent(typeof(ScrollRect))]
	public class TableView : ATableView
	{
		// Token: 0x06004397 RID: 17303 RVA: 0x0015938C File Offset: 0x0015778C
		protected virtual void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.content = this.scrollRect.content;
			this.visHelper = new VisibilityHelper(this.scrollRect.content.parent.GetComponent<RectTransform>());
			this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.HandlePositionChanged));
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x001593F2 File Offset: 0x001577F2
		public override void Reload()
		{
			this.CreateVirtualCells();
			this.SetupVirtualCells();
			Canvas.ForceUpdateCanvases();
			this.HandlePositionChanged(Vector2.zero);
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x00159410 File Offset: 0x00157810
		public override IEnumerable<ATableViewReusableCell> GetVisibleCells(RectTransform viewport, RectTransform slider)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x00159417 File Offset: 0x00157817
		public override IEnumerable<ATableViewReusableCell> GetAllCells()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x00159420 File Offset: 0x00157820
		private void SetupVirtualCells()
		{
			int numberOfCellsForTableView = base.DataSource.GetNumberOfCellsForTableView();
			for (int i = 0; i < this.virtualCells.Count; i++)
			{
				if (this.virtualCells[i].IsActive)
				{
					ATableViewReusableCell componentInChildren = this.virtualCells[i].GetComponentInChildren<ATableViewReusableCell>();
					if (componentInChildren)
					{
						base.AddReusableCellToPool(componentInChildren);
					}
				}
				this.virtualCells[i].gameObject.SetActive(i < numberOfCellsForTableView);
				this.virtualCells[i].IsActive = false;
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x001594BC File Offset: 0x001578BC
		private void CreateVirtualCells()
		{
			int numberOfCellsForTableView = base.DataSource.GetNumberOfCellsForTableView();
			for (int i = this.virtualCells.Count; i < numberOfCellsForTableView; i++)
			{
				VirtualTableCell virtualTableCell = new GameObject("Cell - " + i)
				{
					layer = this.content.gameObject.layer
				}.AddComponent<VirtualTableCell>();
				virtualTableCell.transform.SetParent(this.content, false);
				virtualTableCell.index = i;
				LayoutElement layoutElementForIndex = base.DataSource.GetLayoutElementForIndex(i);
				if (layoutElementForIndex)
				{
					ComponentCopier.CopyLayoutElement(layoutElementForIndex, virtualTableCell.gameObject);
				}
				this.virtualCells.Add(virtualTableCell);
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x00159574 File Offset: 0x00157974
		private void HandlePositionChanged(Vector2 position)
		{
			foreach (VirtualTableCell virtualTableCell in this.virtualCells)
			{
				if (!virtualTableCell.gameObject.activeSelf)
				{
					break;
				}
				if (virtualTableCell.IsActive && !this.visHelper.IsVisible(virtualTableCell.RectTransform))
				{
					this.HandleBecameVisible(virtualTableCell, false);
				}
			}
			foreach (VirtualTableCell virtualTableCell2 in this.virtualCells)
			{
				if (!virtualTableCell2.gameObject.activeSelf)
				{
					break;
				}
				if (!virtualTableCell2.IsActive && this.visHelper.IsVisible(virtualTableCell2.RectTransform))
				{
					this.HandleBecameVisible(virtualTableCell2, true);
				}
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0015968C File Offset: 0x00157A8C
		private void HandleBecameVisible(VirtualTableCell cell, bool state)
		{
			cell.IsActive = state;
			if (state)
			{
				this.ActivateCell(cell);
			}
			else
			{
				this.DeactivateCell(cell);
			}
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x001596AE File Offset: 0x00157AAE
		private void DeactivateCell(VirtualTableCell cell)
		{
			base.AddReusableCellToPool(cell.DataCell);
			cell.DataCell = null;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x001596C4 File Offset: 0x00157AC4
		private void ActivateCell(VirtualTableCell cell)
		{
			ATableViewReusableCell cellForIndex = base.DataSource.GetCellForIndex(cell.index, cell.transform);
			RectTransform component = cellForIndex.GetComponent<RectTransform>();
			component.sizeDelta = cell.GetComponent<RectTransform>().sizeDelta;
			RectTransform rectTransform = component;
			Vector2 zero = Vector2.zero;
			component.anchorMax = zero;
			rectTransform.anchorMin = zero;
			component.pivot = Vector2.zero;
			component.anchoredPosition = Vector2.zero;
			cellForIndex.gameObject.SetActive(true);
			cellForIndex.index = cell.index;
			cell.DataCell = cellForIndex;
		}

		// Token: 0x04006C08 RID: 27656
		private List<VirtualTableCell> virtualCells = new List<VirtualTableCell>();

		// Token: 0x04006C09 RID: 27657
		private RectTransform content;

		// Token: 0x04006C0A RID: 27658
		private ScrollRect scrollRect;

		// Token: 0x04006C0B RID: 27659
		private VisibilityHelper visHelper;
	}
}
