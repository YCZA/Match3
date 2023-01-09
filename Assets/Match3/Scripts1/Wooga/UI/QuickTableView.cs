using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.Wooga.UI
{
	// Token: 0x02000B42 RID: 2882
	[RequireComponent(typeof(ScrollRect))]
	public class QuickTableView : ATableView
	{
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600438B RID: 17291 RVA: 0x00158A48 File Offset: 0x00156E48
		private RectTransform content
		{
			get
			{
				return (!this.overrideContent) ? this.scrollRect.content : this.overrideContent;
			}
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x00158A70 File Offset: 0x00156E70
		protected void Init()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.layoutGroup = this.content.GetComponent<LayoutGroup>();
			this.gridLayout = this.content.GetComponent<GridLayoutGroup>();
			this.originalPadding = this.DuplicatePadding(this.layoutGroup.padding);
			this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.HandlePositionChanged));
			this.cursorStart = ((!this.overrideContent) ? this.originalPadding : this.scrollRect.content.GetComponent<LayoutGroup>().padding);
			this.viewport = ((!this.scrollRect.viewport) ? ((RectTransform)this.scrollRect.transform) : this.scrollRect.viewport);
			this.isInitialized = true;
		}

		private void HandlePositionChanged(Vector2 position)
		{
			if (base.DataSource == null)
			{
				return;
			}
			this.visibleIds.Clear();
			this.redundantIds.Clear();
			
			RectOffset padding = this.ProcessCells(position, base.DataSource.GetNumberOfCellsForTableView());
			foreach (int item in this.usedCells.Keys)
			{
				if (!this.visibleIds.Contains(item))
				{
					this.redundantIds.Add(item);
				}
			}
			foreach (int key in this.redundantIds)
			{
				ATableViewReusableCell atableViewReusableCell = this.usedCells[key];
				base.AddReusableCellToPool(atableViewReusableCell);
				atableViewReusableCell.gameObject.SetActive(false);
				this.usedCells.Remove(key);
			}

			foreach (int num in this.visibleIds)
			{
				// eli key point 关卡列表滑动时执行
				// eli key point 建筑商店列表滑动时执行
				ATableViewReusableCell atableViewReusableCell2 = (!this.usedCells.ContainsKey(num)) ? base.DataSource.GetCellForIndex(num, this.content) : this.usedCells[num];
				atableViewReusableCell2.transform.SetAsLastSibling();
				this.usedCells[num] = atableViewReusableCell2;
			}
			this.layoutGroup.padding = padding;
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x00158D24 File Offset: 0x00157124
		private RectOffset ProcessCells(Vector2 position, int numCells)
		{
			VerticalLayoutGroup component = this.scrollRect.content.GetComponent<VerticalLayoutGroup>();
			if (component)
			{
				return this.ProcessCells(component, position, numCells);
			}
			HorizontalLayoutGroup component2 = this.scrollRect.content.GetComponent<HorizontalLayoutGroup>();
			if (component2)
			{
				return this.ProcessCells(component2, position, numCells);
			}
			GridLayoutGroup component3 = this.scrollRect.content.GetComponent<GridLayoutGroup>();
			if (component3)
			{
				return this.ProcessCells(component3, position, numCells);
			}
			return this.layoutGroup.padding;
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00158DB0 File Offset: 0x001571B0
		private RectOffset ProcessCells(VerticalLayoutGroup layoutGroup, Vector2 position, int numCells)
		{
			int num = 0;
			int cursorStartTop = this.cursorStart.top;
			RectOffset rectOffset = this.DuplicatePadding(this.originalPadding);
			float lengthOfLongSide = (AUiAdjuster.SimilarOrientation != ScreenOrientation.Portrait) ? this.viewport.rect.width : this.viewport.rect.height;
			float positionY = Mathf.Max(this.scrollRect.content.rect.height - lengthOfLongSide, 0f) * (1f - position.y);
			for (int i = 0; i < numCells; i++)
			{
				LayoutElement layoutElementForIndex = base.DataSource.GetLayoutElementForIndex(i);
				int preferredHeight = (int)layoutElementForIndex.preferredHeight;
				if (num != 0)
				{
					if (num != 1)
					{
						if (num == 2)
						{
							rectOffset.bottom += preferredHeight;
						}
					}
					else if ((float)cursorStartTop > positionY + lengthOfLongSide + EXTRA_PADDING_BOTTOM)
					{
						rectOffset.bottom += preferredHeight;
						num++;
					}
					else
					{
						// 添加可见块
						this.visibleIds.Add(i);
					}
				}
				else if ((float)(cursorStartTop + preferredHeight) + EXTRA_PADDING_TOP >= positionY)
				{
					// 添加最右边的可见块(从右边开始添加)
					this.visibleIds.Add(i);
					num++;
				}
				else
				{
					rectOffset.top += preferredHeight;
				}
				cursorStartTop += preferredHeight;
			}
			return rectOffset;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x00158F20 File Offset: 0x00157320
		private RectOffset ProcessCells(HorizontalLayoutGroup layoutGroup, Vector2 position, int numCells)
		{
			int num = 0;
			int num2 = this.cursorStart.left;
			RectOffset rectOffset = this.DuplicatePadding(this.originalPadding);
			float width = this.viewport.rect.width;
			float num3 = Mathf.Max(this.scrollRect.content.rect.width - width, 0f) * position.x;
			for (int i = 0; i < numCells; i++)
			{
				LayoutElement layoutElementForIndex = base.DataSource.GetLayoutElementForIndex(i);
				int num4 = (int)layoutElementForIndex.preferredWidth;
				if (num != 0)
				{
					if (num != 1)
					{
						if (num == 2)
						{
							rectOffset.right += num4;
						}
					}
					else if ((float)num2 > num3 + width)
					{
						rectOffset.right += num4;
						num++;
					}
					else
					{
						this.visibleIds.Add(i);
					}
				}
				else if ((float)(num2 + num4) >= num3)
				{
					this.visibleIds.Add(i);
					num++;
				}
				else
				{
					rectOffset.left += num4;
				}
				num2 += num4;
			}
			return rectOffset;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0015905C File Offset: 0x0015745C
		private RectOffset ProcessCells(GridLayoutGroup layoutGroup, Vector2 position, int numCells)
		{
			int num = 0;
			int num2 = this.cursorStart.top;
			RectOffset rectOffset = this.DuplicatePadding(this.originalPadding);
			float height = this.viewport.rect.height;
			if (base.DataSource.GetNumberOfCellsForTableView() > 0)
			{
				LayoutElement layoutElementForIndex = base.DataSource.GetLayoutElementForIndex(0);
				if (layoutElementForIndex != null)
				{
					float num3 = layoutElementForIndex.preferredWidth;
					if (this.gridLayout != null)
					{
						num3 += this.gridLayout.spacing.x;
					}
					this.rowSize = (int)(this.viewport.rect.width / num3);
				}
			}
			float num4 = Mathf.Max(this.scrollRect.content.rect.height - height, 0f) * (1f - position.y);
			for (int i = 0; i < numCells; i++)
			{
				LayoutElement layoutElementForIndex2 = base.DataSource.GetLayoutElementForIndex(i);
				bool flag = i % this.rowSize == this.rowSize - 1;
				bool flag2 = i % this.rowSize == 0;
				int num5 = (int)layoutElementForIndex2.preferredHeight;
				if (num != 0)
				{
					if (num != 1)
					{
						if (num == 2)
						{
							if (flag2)
							{
								rectOffset.bottom += num5;
							}
						}
					}
					else if (flag2 && (float)num2 > num4 + height)
					{
						rectOffset.bottom += num5;
						num++;
					}
					else
					{
						this.visibleIds.Add(i);
					}
				}
				else if (flag2)
				{
					if ((float)(num2 + num5) >= num4)
					{
						this.visibleIds.Add(i);
						num++;
					}
					else
					{
						rectOffset.top += num5;
					}
				}
				if (flag)
				{
					num2 += num5;
				}
			}
			return rectOffset;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00159257 File Offset: 0x00157657
		private RectOffset DuplicatePadding(RectOffset padding)
		{
			return new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00159278 File Offset: 0x00157678
		public override void Reload()
		{
			if (base.DataSource == null)
			{
				return;
			}
			if (!this.isInitialized)
			{
				this.Init();
			}
			else
			{
				foreach (ATableViewReusableCell atableViewReusableCell in this.usedCells.Values)
				{
					base.AddReusableCellToPool(atableViewReusableCell);
					atableViewReusableCell.gameObject.SetActive(false);
				}
				this.usedCells.Clear();
			}
			Vector2 normalizedPosition = this.scrollRect.normalizedPosition;
			if (this.shouldScrollToTopOnReload)
			{
				this.scrollRect.normalizedPosition = Vector2.one;
			}
			if (this.shouldRefreshCanvasesOnReload)
			{
				Canvas.ForceUpdateCanvases();
			}
			// 修复建筑商店列表在某些分辨率下不正常的问题(需要上下滑动一下才能显示出来)，不知道为什么原来使用Vector2.zero
			// this.HandlePositionChanged((!this.shouldScrollToTopOnReload) ? normalizedPosition : Vector2.zero);
			this.HandlePositionChanged((!this.shouldScrollToTopOnReload) ? normalizedPosition : Vector2.one);
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00159368 File Offset: 0x00157768
		public override IEnumerable<ATableViewReusableCell> GetVisibleCells(RectTransform viewport, RectTransform slider)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0015936F File Offset: 0x0015776F
		public override IEnumerable<ATableViewReusableCell> GetAllCells()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04006BF8 RID: 27640
		private const float EXTRA_PADDING_BOTTOM = 3000f;

		// Token: 0x04006BF9 RID: 27641
		private const float EXTRA_PADDING_TOP = 1500f;

		// Token: 0x04006BFA RID: 27642
		private ScrollRect scrollRect;

		// Token: 0x04006BFB RID: 27643
		private RectOffset originalPadding;

		// Token: 0x04006BFC RID: 27644
		private RectOffset cursorStart;

		// Token: 0x04006BFD RID: 27645
		private RectTransform viewport;

		// Token: 0x04006BFE RID: 27646
		private LayoutGroup layoutGroup;

		// Token: 0x04006BFF RID: 27647
		private GridLayoutGroup gridLayout;

		// Token: 0x04006C00 RID: 27648
		private readonly Dictionary<int, ATableViewReusableCell> usedCells = new Dictionary<int, ATableViewReusableCell>();

		// Token: 0x04006C01 RID: 27649
		private readonly List<int> visibleIds = new List<int>();

		// Token: 0x04006C02 RID: 27650
		private readonly List<int> redundantIds = new List<int>();

		// Token: 0x04006C03 RID: 27651
		private bool isInitialized;

		// Token: 0x04006C04 RID: 27652
		public int rowSize = 1;

		// Token: 0x04006C05 RID: 27653
		public RectTransform overrideContent;

		// Token: 0x04006C06 RID: 27654
		[SerializeField]
		private bool shouldRefreshCanvasesOnReload;

		// Token: 0x04006C07 RID: 27655
		[SerializeField]
		private bool shouldScrollToTopOnReload = true;
	}
}
