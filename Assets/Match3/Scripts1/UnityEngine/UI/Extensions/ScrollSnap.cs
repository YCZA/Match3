using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BF3 RID: 3059
	[ExecuteInEditMode]
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scroll Snap")]
	public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollSnap, IEventSystemHandler
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060047B8 RID: 18360 RVA: 0x0016EC4C File Offset: 0x0016D04C
		// (remove) Token: 0x060047B9 RID: 18361 RVA: 0x0016EC84 File Offset: 0x0016D084
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event ScrollSnap.PageSnapChange onPageChange;

		// Token: 0x060047BA RID: 18362 RVA: 0x0016ECBC File Offset: 0x0016D0BC
		private void Start()
		{
			this._lerp = false;
			this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			this._scrollRectTransform = base.gameObject.GetComponent<RectTransform>();
			this._listContainerTransform = this._scroll_rect.content;
			this._listContainerRectTransform = this._listContainerTransform.GetComponent<RectTransform>();
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			this.PageChanged(this.CurrentPage());
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.NextScreen();
				});
			}
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.PreviousScreen();
				});
			}
			if (this._scroll_rect.horizontalScrollbar != null && this._scroll_rect.horizontal)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper = this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper.ss = this;
			}
			if (this._scroll_rect.verticalScrollbar != null && this._scroll_rect.vertical)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper2 = this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper2.ss = this;
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x0016EE14 File Offset: 0x0016D214
		public void UpdateListItemsSize()
		{
			float num;
			float num2;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._scrollRectTransform.rect.width / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.width / (float)this._itemsCount;
			}
			else
			{
				num = this._scrollRectTransform.rect.height / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.height / (float)this._itemsCount;
			}
			this._itemSize = num;
			if (this.LinkScrolrectScrollSensitivity)
			{
				this._scroll_rect.scrollSensitivity = this._itemSize;
			}
			if (this.AutoLayoutItems && num2 != num && this._itemsCount > 0)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					IEnumerator enumerator = this._listContainerTransform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							GameObject gameObject = ((Transform)obj).gameObject;
							if (gameObject.activeInHierarchy)
							{
								LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
								if (layoutElement == null)
								{
									layoutElement = gameObject.AddComponent<LayoutElement>();
								}
								layoutElement.minWidth = this._itemSize;
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				else
				{
					IEnumerator enumerator2 = this._listContainerTransform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							GameObject gameObject2 = ((Transform)obj2).gameObject;
							if (gameObject2.activeInHierarchy)
							{
								LayoutElement layoutElement2 = gameObject2.GetComponent<LayoutElement>();
								if (layoutElement2 == null)
								{
									layoutElement2 = gameObject2.AddComponent<LayoutElement>();
								}
								layoutElement2.minHeight = this._itemSize;
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x0016F030 File Offset: 0x0016D430
		public void UpdateListItemPositions()
		{
			if (!this._listContainerRectTransform.rect.size.Equals(this._listContainerCachedSize))
			{
				int num = 0;
				IEnumerator enumerator = this._listContainerTransform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						if (((Transform)obj).gameObject.activeInHierarchy)
						{
							num++;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this._itemsCount = 0;
				Array.Resize<Vector3>(ref this._pageAnchorPositions, num);
				if (num > 0)
				{
					this._pages = Mathf.Max(num - this.ItemsVisibleAtOnce + 1, 1);
					if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
					{
						this._scroll_rect.horizontalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.x;
						this._scroll_rect.horizontalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.x;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int i = 0; i < this._pages; i++)
						{
							this._pageAnchorPositions[i] = new Vector3(this._listContainerMaxPosition - this._itemSize * (float)i, this._listContainerTransform.localPosition.y, this._listContainerTransform.localPosition.z);
						}
					}
					else
					{
						this._scroll_rect.verticalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.y;
						this._scroll_rect.verticalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.y;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int j = 0; j < this._pages; j++)
						{
							this._pageAnchorPositions[j] = new Vector3(this._listContainerTransform.localPosition.x, this._listContainerMinPosition + this._itemSize * (float)j, this._listContainerTransform.localPosition.z);
						}
					}
					this.UpdateScrollbar(this.LinkScrolbarSteps);
					this._startingPage = Mathf.Min(this._startingPage, this._pages);
					this.ResetPage();
				}
				if (this._itemsCount != num)
				{
					this.PageChanged(this.CurrentPage());
				}
				this._itemsCount = num;
				this._listContainerCachedSize.Set(this._listContainerRectTransform.rect.size.x, this._listContainerRectTransform.rect.size.y);
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0016F344 File Offset: 0x0016D744
		public void ResetPage()
		{
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				this._scroll_rect.horizontalNormalizedPosition = ((this._pages <= 1) ? 0f : ((float)this._startingPage / (float)(this._pages - 1)));
			}
			else
			{
				this._scroll_rect.verticalNormalizedPosition = ((this._pages <= 1) ? 0f : ((float)(this._pages - this._startingPage - 1) / (float)(this._pages - 1)));
			}
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0016F3D0 File Offset: 0x0016D7D0
		private void UpdateScrollbar(bool linkSteps)
		{
			if (linkSteps)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					if (this._scroll_rect.horizontalScrollbar != null)
					{
						this._scroll_rect.horizontalScrollbar.numberOfSteps = this._pages;
					}
				}
				else if (this._scroll_rect.verticalScrollbar != null)
				{
					this._scroll_rect.verticalScrollbar.numberOfSteps = this._pages;
				}
			}
			else if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				if (this._scroll_rect.horizontalScrollbar != null)
				{
					this._scroll_rect.horizontalScrollbar.numberOfSteps = 0;
				}
			}
			else if (this._scroll_rect.verticalScrollbar != null)
			{
				this._scroll_rect.verticalScrollbar.numberOfSteps = 0;
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0016F4B0 File Offset: 0x0016D8B0
		private void LateUpdate()
		{
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			if (this._lerp)
			{
				this.UpdateScrollbar(false);
				this._listContainerTransform.localPosition = Vector3.Lerp(this._listContainerTransform.localPosition, this._lerpTarget, 7.5f * Time.deltaTime);
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 0.001f)
				{
					this._listContainerTransform.localPosition = this._lerpTarget;
					this._lerp = false;
					this.UpdateScrollbar(this.LinkScrolbarSteps);
				}
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 10f)
				{
					this.PageChanged(this.CurrentPage());
				}
			}
			if (this._fastSwipeTimer)
			{
				this._fastSwipeCounter++;
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0016F590 File Offset: 0x0016D990
		public void NextScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() < this._pages - 1)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() + 1];
				this.PageChanged(this.CurrentPage() + 1);
			}
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x0016F5EC File Offset: 0x0016D9EC
		public void PreviousScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() > 0)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() - 1];
				this.PageChanged(this.CurrentPage() - 1);
			}
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x0016F640 File Offset: 0x0016DA40
		private void NextScreenCommand()
		{
			if (this._pageOnDragStart < this._pages - 1)
			{
				int num = Mathf.Min(this._pages - 1, this._pageOnDragStart + this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x0016F6A0 File Offset: 0x0016DAA0
		private void PrevScreenCommand()
		{
			if (this._pageOnDragStart > 0)
			{
				int num = Mathf.Max(0, this._pageOnDragStart - this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0016F6F4 File Offset: 0x0016DAF4
		public int CurrentPage()
		{
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._listContainerMaxPosition - this._listContainerTransform.localPosition.x;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			else
			{
				num = this._listContainerTransform.localPosition.y - this._listContainerMinPosition;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			float f = num / this._itemSize;
			return Mathf.Clamp(Mathf.RoundToInt(f), 0, this._pages);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0016F786 File Offset: 0x0016DB86
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x0016F78F File Offset: 0x0016DB8F
		public void ChangePage(int page)
		{
			if (0 <= page && page < this._pages)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[page];
				this.PageChanged(page);
			}
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x0016F7CC File Offset: 0x0016DBCC
		private void PageChanged(int currentPage)
		{
			this._startingPage = currentPage;
			if (this.NextButton)
			{
				this.NextButton.interactable = (currentPage < this._pages - 1);
			}
			if (this.PrevButton)
			{
				this.PrevButton.interactable = (currentPage > 0);
			}
			if (this.onPageChange != null)
			{
				this.onPageChange(currentPage);
			}
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x0016F83C File Offset: 0x0016DC3C
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.UpdateScrollbar(false);
			this._fastSwipeCounter = 0;
			this._fastSwipeTimer = true;
			this._positionOnDragStart = eventData.position;
			this._pageOnDragStart = this.CurrentPage();
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0016F870 File Offset: 0x0016DC70
		public void OnEndDrag(PointerEventData eventData)
		{
			this._startDrag = true;
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._positionOnDragStart.x - eventData.position.x;
			}
			else
			{
				num = -this._positionOnDragStart.y + eventData.position.y;
			}
			if (this.UseFastSwipe)
			{
				this.fastSwipe = false;
				this._fastSwipeTimer = false;
				if (this._fastSwipeCounter <= this._fastSwipeTarget && Math.Abs(num) > (float)this.FastSwipeThreshold)
				{
					this.fastSwipe = true;
				}
				if (this.fastSwipe)
				{
					if (num > 0f)
					{
						this.NextScreenCommand();
					}
					else
					{
						this.PrevScreenCommand();
					}
				}
				else
				{
					this._lerp = true;
					this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				}
			}
			else
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
			}
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x0016F98B File Offset: 0x0016DD8B
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
			if (this._startDrag)
			{
				this.OnBeginDrag(eventData);
				this._startDrag = false;
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x0016F9AD File Offset: 0x0016DDAD
		public void StartScreenChange()
		{
		}

		// Token: 0x04006EAB RID: 28331
		private ScrollRect _scroll_rect;

		// Token: 0x04006EAC RID: 28332
		private RectTransform _scrollRectTransform;

		// Token: 0x04006EAD RID: 28333
		private Transform _listContainerTransform;

		// Token: 0x04006EAE RID: 28334
		private int _pages;

		// Token: 0x04006EAF RID: 28335
		private int _startingPage;

		// Token: 0x04006EB0 RID: 28336
		private Vector3[] _pageAnchorPositions;

		// Token: 0x04006EB1 RID: 28337
		private Vector3 _lerpTarget;

		// Token: 0x04006EB2 RID: 28338
		private bool _lerp;

		// Token: 0x04006EB3 RID: 28339
		private float _listContainerMinPosition;

		// Token: 0x04006EB4 RID: 28340
		private float _listContainerMaxPosition;

		// Token: 0x04006EB5 RID: 28341
		private float _listContainerSize;

		// Token: 0x04006EB6 RID: 28342
		private RectTransform _listContainerRectTransform;

		// Token: 0x04006EB7 RID: 28343
		private Vector2 _listContainerCachedSize;

		// Token: 0x04006EB8 RID: 28344
		private float _itemSize;

		// Token: 0x04006EB9 RID: 28345
		private int _itemsCount;

		// Token: 0x04006EBA RID: 28346
		private bool _startDrag = true;

		// Token: 0x04006EBB RID: 28347
		private Vector3 _positionOnDragStart = default(Vector3);

		// Token: 0x04006EBC RID: 28348
		private int _pageOnDragStart;

		// Token: 0x04006EBD RID: 28349
		private bool _fastSwipeTimer;

		// Token: 0x04006EBE RID: 28350
		private int _fastSwipeCounter;

		// Token: 0x04006EBF RID: 28351
		private int _fastSwipeTarget = 10;

		// Token: 0x04006EC0 RID: 28352
		[Tooltip("Button to go to the next page. (optional)")]
		public Button NextButton;

		// Token: 0x04006EC1 RID: 28353
		[Tooltip("Button to go to the previous page. (optional)")]
		public Button PrevButton;

		// Token: 0x04006EC2 RID: 28354
		[Tooltip("Number of items visible in one page of scroll frame.")]
		[Range(1f, 100f)]
		public int ItemsVisibleAtOnce = 1;

		// Token: 0x04006EC3 RID: 28355
		[Tooltip("Sets minimum width of list items to 1/itemsVisibleAtOnce.")]
		public bool AutoLayoutItems = true;

		// Token: 0x04006EC4 RID: 28356
		[Tooltip("If you wish to update scrollbar numberOfSteps to number of active children on list.")]
		public bool LinkScrolbarSteps;

		// Token: 0x04006EC5 RID: 28357
		[Tooltip("If you wish to update scrollrect sensitivity to size of list element.")]
		public bool LinkScrolrectScrollSensitivity;

		// Token: 0x04006EC6 RID: 28358
		public bool UseFastSwipe = true;

		// Token: 0x04006EC7 RID: 28359
		public int FastSwipeThreshold = 100;

		// Token: 0x04006EC9 RID: 28361
		public ScrollSnap.ScrollDirection direction;

		// Token: 0x04006ECA RID: 28362
		private bool fastSwipe;

		// Token: 0x02000BF4 RID: 3060
		public enum ScrollDirection
		{
			// Token: 0x04006ECC RID: 28364
			Horizontal,
			// Token: 0x04006ECD RID: 28365
			Vertical
		}

		// Token: 0x02000BF5 RID: 3061
		// (Invoke) Token: 0x060047CF RID: 18383
		public delegate void PageSnapChange(int page);
	}
}
