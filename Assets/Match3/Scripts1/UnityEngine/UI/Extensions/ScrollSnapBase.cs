using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BF6 RID: 3062
	public class ScrollSnapBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollSnap, IEventSystemHandler
	{
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060047D3 RID: 18387 RVA: 0x0016D1AD File Offset: 0x0016B5AD
		// (set) Token: 0x060047D4 RID: 18388 RVA: 0x0016D1B8 File Offset: 0x0016B5B8
		public int CurrentPage
		{
			get
			{
				return this._currentPage;
			}
			internal set
			{
				if ((value != this._currentPage && value >= 0 && value < this._screensContainer.childCount) || (value == 0 && this._screensContainer.childCount == 0))
				{
					this._previousPage = this._currentPage;
					this._currentPage = value;
					if (this.MaskArea)
					{
						this.UpdateVisible();
					}
					if (!this._lerp)
					{
						this.ScreenChange();
					}
					this.OnCurrentScreenChange(this._currentPage);
				}
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060047D5 RID: 18389 RVA: 0x0016D245 File Offset: 0x0016B645
		// (set) Token: 0x060047D6 RID: 18390 RVA: 0x0016D24D File Offset: 0x0016B64D
		public ScrollSnapBase.SelectionChangeStartEvent OnSelectionChangeStartEvent
		{
			get
			{
				return this.m_OnSelectionChangeStartEvent;
			}
			set
			{
				this.m_OnSelectionChangeStartEvent = value;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060047D7 RID: 18391 RVA: 0x0016D256 File Offset: 0x0016B656
		// (set) Token: 0x060047D8 RID: 18392 RVA: 0x0016D25E File Offset: 0x0016B65E
		public ScrollSnapBase.SelectionPageChangedEvent OnSelectionPageChangedEvent
		{
			get
			{
				return this.m_OnSelectionPageChangedEvent;
			}
			set
			{
				this.m_OnSelectionPageChangedEvent = value;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060047D9 RID: 18393 RVA: 0x0016D267 File Offset: 0x0016B667
		// (set) Token: 0x060047DA RID: 18394 RVA: 0x0016D26F File Offset: 0x0016B66F
		public ScrollSnapBase.SelectionChangeEndEvent OnSelectionChangeEndEvent
		{
			get
			{
				return this.m_OnSelectionChangeEndEvent;
			}
			set
			{
				this.m_OnSelectionChangeEndEvent = value;
			}
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0016D278 File Offset: 0x0016B678
		private void Awake()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			}
			if (this._scroll_rect.horizontalScrollbar && this._scroll_rect.horizontal)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper = this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper.ss = this;
			}
			if (this._scroll_rect.verticalScrollbar && this._scroll_rect.vertical)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper2 = this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper2.ss = this;
			}
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			if (this.StartingScreen < 0)
			{
				this.StartingScreen = 0;
			}
			this._screensContainer = this._scroll_rect.content;
			this.InitialiseChildObjects();
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
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0016D3D0 File Offset: 0x0016B7D0
		internal void InitialiseChildObjects()
		{
			if (this.ChildObjects != null && this.ChildObjects.Length > 0)
			{
				if (this._screensContainer.transform.childCount > 0)
				{
					global::UnityEngine.Debug.LogError("ScrollRect Content has children, this is not supported when using managed Child Objects\n Either remove the ScrollRect Content children or clear the ChildObjects array");
					return;
				}
				this.InitialiseChildObjectsFromArray();
			}
			else
			{
				this.InitialiseChildObjectsFromScene();
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0016D428 File Offset: 0x0016B828
		internal void InitialiseChildObjectsFromScene()
		{
			int childCount = this._screensContainer.childCount;
			this.ChildObjects = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				this.ChildObjects[i] = this._screensContainer.transform.GetChild(i).gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0016D4B0 File Offset: 0x0016B8B0
		internal void InitialiseChildObjectsFromArray()
		{
			int num = this.ChildObjects.Length;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.ChildObjects[i]);
				if (this.UseParentTransform)
				{
					RectTransform component = gameObject.GetComponent<RectTransform>();
					component.rotation = this._screensContainer.rotation;
					component.localScale = this._screensContainer.localScale;
					component.position = this._screensContainer.position;
				}
				gameObject.transform.SetParent(this._screensContainer.transform);
				this.ChildObjects[i] = gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0016D57C File Offset: 0x0016B97C
		internal void UpdateVisible()
		{
			if (!this.MaskArea || this.ChildObjects == null || this.ChildObjects.Length < 1 || this._screensContainer.childCount < 1)
			{
				return;
			}
			this._maskSize = ((!this._isVertical) ? this.MaskArea.rect.width : this.MaskArea.rect.height);
			this._halfNoVisibleItems = (int)Math.Round((double)(this._maskSize / (this._childSize * this.MaskBuffer)), MidpointRounding.AwayFromZero) / 2;
			this._bottomItem = (this._topItem = 0);
			for (int i = this._halfNoVisibleItems + 1; i > 0; i--)
			{
				this._bottomItem = ((this._currentPage - i >= 0) ? i : 0);
				if (this._bottomItem > 0)
				{
					break;
				}
			}
			for (int j = this._halfNoVisibleItems + 1; j > 0; j--)
			{
				this._topItem = ((this._screensContainer.childCount - this._currentPage - j >= 0) ? j : 0);
				if (this._topItem > 0)
				{
					break;
				}
			}
			for (int k = this.CurrentPage - this._bottomItem; k < this.CurrentPage + this._topItem; k++)
			{
				try
				{
					this.ChildObjects[k].SetActive(true);
				}
				catch
				{
					global::UnityEngine.Debug.Log("Failed to setactive child [" + k + "]");
				}
			}
			if (this._currentPage > this._halfNoVisibleItems)
			{
				this.ChildObjects[this.CurrentPage - this._bottomItem].SetActive(false);
			}
			if (this._screensContainer.childCount - this._currentPage > this._topItem)
			{
				this.ChildObjects[this.CurrentPage + this._topItem].SetActive(false);
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0016D7A8 File Offset: 0x0016BBA8
		public void NextScreen()
		{
			if (this._currentPage < this._screens - 1)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage + 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x0016D808 File Offset: 0x0016BC08
		public void PreviousScreen()
		{
			if (this._currentPage > 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage - 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0016D860 File Offset: 0x0016BC60
		public void GoToScreen(int screenIndex)
		{
			if (screenIndex <= this._screens - 1 && screenIndex >= 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = screenIndex;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x0016D8BC File Offset: 0x0016BCBC
		internal int GetPageforPosition(Vector3 pos)
		{
			return (!this._isVertical) ? ((int)Math.Round((double)((this._scrollStartPosition - pos.x) / this._childSize))) : ((int)Math.Round((double)((this._scrollStartPosition - pos.y) / this._childSize)));
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0016D914 File Offset: 0x0016BD14
		internal bool IsRectSettledOnaPage(Vector3 pos)
		{
			return (!this._isVertical) ? (-((pos.x - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.x - this._scrollStartPosition) / this._childSize))))) : (-((pos.y - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.y - this._scrollStartPosition) / this._childSize)))));
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0016D9A0 File Offset: 0x0016BDA0
		internal void GetPositionforPage(int page, ref Vector3 target)
		{
			this._childPos = -this._childSize * (float)page;
			if (this._isVertical)
			{
				target.y = this._childPos + this._scrollStartPosition;
			}
			else
			{
				target.x = this._childPos + this._scrollStartPosition;
			}
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x0016D9F3 File Offset: 0x0016BDF3
		internal void ScrollToClosestElement()
		{
			this._lerp = true;
			this.CurrentPage = this.GetPageforPosition(this._screensContainer.localPosition);
			this.GetPositionforPage(this._currentPage, ref this._lerp_target);
			this.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0016DA31 File Offset: 0x0016BE31
		internal void OnCurrentScreenChange(int currentScreen)
		{
			this.ChangeBulletsInfo(currentScreen);
			this.ToggleNavigationButtons(currentScreen);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0016DA44 File Offset: 0x0016BE44
		private void ChangeBulletsInfo(int targetScreen)
		{
			if (this.Pagination)
			{
				for (int i = 0; i < this.Pagination.transform.childCount; i++)
				{
					this.Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (targetScreen == i);
				}
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x0016DAAC File Offset: 0x0016BEAC
		private void ToggleNavigationButtons(int targetScreen)
		{
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().interactable = (targetScreen > 0);
			}
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().interactable = (targetScreen < this._screensContainer.transform.childCount - 1);
			}
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x0016DB14 File Offset: 0x0016BF14
		private void OnValidate()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.GetComponent<ScrollRect>();
			}
			if (!this._scroll_rect.horizontal && !this._scroll_rect.vertical)
			{
				global::UnityEngine.Debug.LogError("ScrollRect has to have a direction, please select either Horizontal OR Vertical with the appropriate control.");
			}
			if (this._scroll_rect.horizontal && this._scroll_rect.vertical)
			{
				global::UnityEngine.Debug.LogError("ScrollRect has to be unidirectional, only use either Horizontal or Vertical on the ScrollRect, NOT both.");
			}
			int childCount = base.gameObject.GetComponent<ScrollRect>().content.childCount;
			if (childCount != 0 || this.ChildObjects != null)
			{
				int num = (this.ChildObjects != null && this.ChildObjects.Length != 0) ? this.ChildObjects.Length : childCount;
				if (this.StartingScreen > num - 1)
				{
					this.StartingScreen = num - 1;
				}
				if (this.StartingScreen < 0)
				{
					this.StartingScreen = 0;
				}
			}
			if (this.MaskBuffer <= 0f)
			{
				this.MaskBuffer = 1f;
			}
			if (this.PageStep < 0f)
			{
				this.PageStep = 0f;
			}
			if (this.PageStep > 8f)
			{
				this.PageStep = 9f;
			}
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0016DC5B File Offset: 0x0016C05B
		public void StartScreenChange()
		{
			if (!this._moveStarted)
			{
				this._moveStarted = true;
				this.OnSelectionChangeStartEvent.Invoke();
			}
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0016DC7A File Offset: 0x0016C07A
		internal void ScreenChange()
		{
			this.OnSelectionPageChangedEvent.Invoke(this._currentPage);
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0016DC8D File Offset: 0x0016C08D
		internal void EndScreenChange()
		{
			this.OnSelectionChangeEndEvent.Invoke(this._currentPage);
			this._settled = true;
			this._moveStarted = false;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0016DCAE File Offset: 0x0016C0AE
		public Transform CurrentPageObject()
		{
			return this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0016DCC1 File Offset: 0x0016C0C1
		public void CurrentPageObject(out Transform returnObject)
		{
			returnObject = this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0016DCD6 File Offset: 0x0016C0D6
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._pointerDown = true;
			this._settled = false;
			this.StartScreenChange();
			this._startPosition = this._screensContainer.localPosition;
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0016DCFD File Offset: 0x0016C0FD
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0016DD08 File Offset: 0x0016C108
		int IScrollSnap.CurrentPage()
		{
			int pageforPosition = this.GetPageforPosition(this._screensContainer.localPosition);
			this.CurrentPage = pageforPosition;
			return pageforPosition;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0016DD2F File Offset: 0x0016C12F
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0016DD38 File Offset: 0x0016C138
		public void ChangePage(int page)
		{
			this.GoToScreen(page);
		}

		// Token: 0x04006ECE RID: 28366
		internal Rect panelDimensions;

		// Token: 0x04006ECF RID: 28367
		internal RectTransform _screensContainer;

		// Token: 0x04006ED0 RID: 28368
		internal bool _isVertical;

		// Token: 0x04006ED1 RID: 28369
		internal int _screens = 1;

		// Token: 0x04006ED2 RID: 28370
		internal float _scrollStartPosition;

		// Token: 0x04006ED3 RID: 28371
		internal float _childSize;

		// Token: 0x04006ED4 RID: 28372
		private float _childPos;

		// Token: 0x04006ED5 RID: 28373
		private float _maskSize;

		// Token: 0x04006ED6 RID: 28374
		internal Vector2 _childAnchorPoint;

		// Token: 0x04006ED7 RID: 28375
		internal ScrollRect _scroll_rect;

		// Token: 0x04006ED8 RID: 28376
		internal Vector3 _lerp_target;

		// Token: 0x04006ED9 RID: 28377
		internal bool _lerp;

		// Token: 0x04006EDA RID: 28378
		internal bool _pointerDown;

		// Token: 0x04006EDB RID: 28379
		internal bool _settled = true;

		// Token: 0x04006EDC RID: 28380
		internal Vector3 _startPosition = default(Vector3);

		// Token: 0x04006EDD RID: 28381
		[Tooltip("The currently active page")]
		internal int _currentPage;

		// Token: 0x04006EDE RID: 28382
		internal int _previousPage;

		// Token: 0x04006EDF RID: 28383
		internal int _halfNoVisibleItems;

		// Token: 0x04006EE0 RID: 28384
		internal bool _moveStarted;

		// Token: 0x04006EE1 RID: 28385
		private int _bottomItem;

		// Token: 0x04006EE2 RID: 28386
		private int _topItem;

		// Token: 0x04006EE3 RID: 28387
		[Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
		[SerializeField]
		public int StartingScreen;

		// Token: 0x04006EE4 RID: 28388
		[Tooltip("The distance between two pages based on page height, by default pages are next to each other")]
		[SerializeField]
		[Range(0f, 8f)]
		public float PageStep = 1f;

		// Token: 0x04006EE5 RID: 28389
		[Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
		public GameObject Pagination;

		// Token: 0x04006EE6 RID: 28390
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject PrevButton;

		// Token: 0x04006EE7 RID: 28391
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject NextButton;

		// Token: 0x04006EE8 RID: 28392
		[Tooltip("Transition speed between pages. (optional)")]
		public float transitionSpeed = 7.5f;

		// Token: 0x04006EE9 RID: 28393
		[Tooltip("Fast Swipe makes swiping page next / previous (optional)")]
		public bool UseFastSwipe;

		// Token: 0x04006EEA RID: 28394
		[Tooltip("Offset for how far a swipe has to travel to initiate a page change (optional)")]
		public int FastSwipeThreshold = 100;

		// Token: 0x04006EEB RID: 28395
		[Tooltip("Speed at which the ScrollRect will keep scrolling before slowing down and stopping (optional)")]
		public int SwipeVelocityThreshold = 100;

		// Token: 0x04006EEC RID: 28396
		[Tooltip("The visible bounds area, controls which items are visible/enabled. *Note Should use a RectMask. (optional)")]
		public RectTransform MaskArea;

		// Token: 0x04006EED RID: 28397
		[Tooltip("Pixel size to buffer arround Mask Area. (optional)")]
		public float MaskBuffer = 1f;

		// Token: 0x04006EEE RID: 28398
		[Tooltip("By default the container will lerp to the start when enabled in the scene, this option overrides this and forces it to simply jump without lerping")]
		public bool JumpOnEnable;

		// Token: 0x04006EEF RID: 28399
		[Tooltip("By default the container will return to the original starting page when enabled, this option overrides this behaviour and stays on the current selection")]
		public bool RestartOnEnable;

		// Token: 0x04006EF0 RID: 28400
		[Tooltip("(Experimental)\nBy default, child array objects will use the parent transform\nHowever you can disable this for some interesting effects")]
		public bool UseParentTransform = true;

		// Token: 0x04006EF1 RID: 28401
		[Tooltip("Scroll Snap children. (optional)\nEither place objects in the scene as children OR\nPrefabs in this array, NOT BOTH")]
		public GameObject[] ChildObjects;

		// Token: 0x04006EF2 RID: 28402
		[SerializeField]
		[Tooltip("Event fires when a user starts to change the selection")]
		private ScrollSnapBase.SelectionChangeStartEvent m_OnSelectionChangeStartEvent = new ScrollSnapBase.SelectionChangeStartEvent();

		// Token: 0x04006EF3 RID: 28403
		[SerializeField]
		[Tooltip("Event fires as the page changes, while dragging or jumping")]
		private ScrollSnapBase.SelectionPageChangedEvent m_OnSelectionPageChangedEvent = new ScrollSnapBase.SelectionPageChangedEvent();

		// Token: 0x04006EF4 RID: 28404
		[SerializeField]
		[Tooltip("Event fires when the page settles after a user has dragged")]
		private ScrollSnapBase.SelectionChangeEndEvent m_OnSelectionChangeEndEvent = new ScrollSnapBase.SelectionChangeEndEvent();

		// Token: 0x02000BF7 RID: 3063
		[Serializable]
		public class SelectionChangeStartEvent : UnityEvent
		{
		}

		// Token: 0x02000BF8 RID: 3064
		[Serializable]
		public class SelectionPageChangedEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000BF9 RID: 3065
		[Serializable]
		public class SelectionChangeEndEvent : UnityEvent<int>
		{
		}
	}
}
