using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B9F RID: 2975
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Dropdown List")]
	public class DropDownList : MonoBehaviour
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060045A8 RID: 17832 RVA: 0x001613CB File Offset: 0x0015F7CB
		// (set) Token: 0x060045A9 RID: 17833 RVA: 0x001613D3 File Offset: 0x0015F7D3
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x001613DC File Offset: 0x0015F7DC
		// (set) Token: 0x060045AB RID: 17835 RVA: 0x001613E4 File Offset: 0x0015F7E4
		public float ScrollBarWidth
		{
			get
			{
				return this._scrollBarWidth;
			}
			set
			{
				this._scrollBarWidth = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x001613F3 File Offset: 0x0015F7F3
		// (set) Token: 0x060045AD RID: 17837 RVA: 0x001613FB File Offset: 0x0015F7FB
		public int ItemsToDisplay
		{
			get
			{
				return this._itemsToDisplay;
			}
			set
			{
				this._itemsToDisplay = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x0016140A File Offset: 0x0015F80A
		public void Start()
		{
			this.Initialize();
			if (this.SelectFirstItemOnStart && this.Items.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(0);
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00161440 File Offset: 0x0015F840
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._mainButton = new DropDownListButton(this._rectTransform.Find("MainButton").gameObject);
				this._overlayRT = this._rectTransform.Find("Overlay").GetComponent<RectTransform>();
				this._overlayRT.gameObject.SetActive(false);
				this._scrollPanelRT = this._overlayRT.Find("ScrollPanel").GetComponent<RectTransform>();
				this._scrollBarRT = this._scrollPanelRT.Find("Scrollbar").GetComponent<RectTransform>();
				this._slidingAreaRT = this._scrollBarRT.Find("SlidingArea").GetComponent<RectTransform>();
				this._itemsPanelRT = this._scrollPanelRT.Find("Items").GetComponent<RectTransform>();
				this._canvas = base.GetComponentInParent<Canvas>();
				this._canvasRT = this._canvas.GetComponent<RectTransform>();
				this._scrollRect = this._scrollPanelRT.GetComponent<ScrollRect>();
				this._scrollRect.scrollSensitivity = this._rectTransform.sizeDelta.y / 2f;
				this._scrollRect.movementType = ScrollRect.MovementType.Clamped;
				this._scrollRect.content = this._itemsPanelRT;
				this._itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this._itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				global::UnityEngine.Debug.LogException(exception);
				global::UnityEngine.Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Reference Exception");
				result = false;
			}
			this._panelItems = new List<DropDownListButton>();
			this.RebuildPanel();
			this.RedrawPanel();
			return result;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x001615FC File Offset: 0x0015F9FC
		private void RebuildPanel()
		{
			if (this.Items.Count == 0)
			{
				return;
			}
			int num = this._panelItems.Count;
			while (this._panelItems.Count < this.Items.Count)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this._itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				this._panelItems.Add(new DropDownListButton(gameObject));
				num++;
			}
			for (int i = 0; i < this._panelItems.Count; i++)
			{
				if (i < this.Items.Count)
				{
					DropDownListItem item = this.Items[i];
					this._panelItems[i].txt.text = item.Caption;
					if (item.IsDisabled)
					{
						this._panelItems[i].txt.color = this.disabledTextColor;
					}
					if (this._panelItems[i].btnImg != null)
					{
						this._panelItems[i].btnImg.sprite = null;
					}
					this._panelItems[i].img.sprite = item.Image;
					this._panelItems[i].img.color = ((!(item.Image == null)) ? ((!item.IsDisabled) ? Color.white : new Color(1f, 1f, 1f, 0.5f)) : new Color(1f, 1f, 1f, 0f));
					int ii = i;
					this._panelItems[i].btn.onClick.RemoveAllListeners();
					this._panelItems[i].btn.onClick.AddListener(delegate()
					{
						this.OnItemClicked(ii);
						if (item.OnSelect != null)
						{
							item.OnSelect();
						}
					});
				}
				this._panelItems[i].gameobject.SetActive(i < this.Items.Count);
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00161866 File Offset: 0x0015FC66
		private void OnItemClicked(int indx)
		{
			if (indx != this._selectedIndex && this.OnSelectionChanged != null)
			{
				this.OnSelectionChanged.Invoke(indx);
			}
			this._selectedIndex = indx;
			this.ToggleDropdownPanel(true);
			this.UpdateSelected();
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x001618A0 File Offset: 0x0015FCA0
		private void UpdateSelected()
		{
			this.SelectedItem = ((this._selectedIndex <= -1 || this._selectedIndex >= this.Items.Count) ? null : this.Items[this._selectedIndex]);
			if (this.SelectedItem == null)
			{
				return;
			}
			bool flag = this.SelectedItem.Image != null;
			if (flag)
			{
				this._mainButton.img.sprite = this.SelectedItem.Image;
				this._mainButton.img.color = Color.white;
			}
			else
			{
				this._mainButton.img.sprite = null;
			}
			this._mainButton.txt.text = this.SelectedItem.Caption;
			if (this.OverrideHighlighted)
			{
				for (int i = 0; i < this._itemsPanelRT.childCount; i++)
				{
					this._panelItems[i].btnImg.color = ((this._selectedIndex != i) ? new Color(0f, 0f, 0f, 0f) : this._mainButton.btn.colors.highlightedColor);
				}
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x001619F0 File Offset: 0x0015FDF0
		private void RedrawPanel()
		{
			float num = (this.Items.Count <= this.ItemsToDisplay) ? 0f : this._scrollBarWidth;
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._mainButton.rectTransform.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._mainButton.txt.rectTransform.offsetMax = new Vector2(4f, 0f);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this.Items.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this.Items.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00161C7C File Offset: 0x0016007C
		public void ToggleDropdownPanel(bool directClick)
		{
			this._overlayRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._scrollBarRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._isPanelActive = !this._isPanelActive;
			this._overlayRT.gameObject.SetActive(this._isPanelActive);
			if (this._isPanelActive)
			{
				base.transform.SetAsLastSibling();
			}
			else if (directClick)
			{
			}
		}

		// Token: 0x04006D2D RID: 27949
		public Color disabledTextColor;

		// Token: 0x04006D2F RID: 27951
		public List<DropDownListItem> Items;

		// Token: 0x04006D30 RID: 27952
		public bool OverrideHighlighted = true;

		// Token: 0x04006D31 RID: 27953
		private bool _isPanelActive;

		// Token: 0x04006D32 RID: 27954
		private bool _hasDrawnOnce;

		// Token: 0x04006D33 RID: 27955
		private DropDownListButton _mainButton;

		// Token: 0x04006D34 RID: 27956
		private RectTransform _rectTransform;

		// Token: 0x04006D35 RID: 27957
		private RectTransform _overlayRT;

		// Token: 0x04006D36 RID: 27958
		private RectTransform _scrollPanelRT;

		// Token: 0x04006D37 RID: 27959
		private RectTransform _scrollBarRT;

		// Token: 0x04006D38 RID: 27960
		private RectTransform _slidingAreaRT;

		// Token: 0x04006D39 RID: 27961
		private RectTransform _itemsPanelRT;

		// Token: 0x04006D3A RID: 27962
		private Canvas _canvas;

		// Token: 0x04006D3B RID: 27963
		private RectTransform _canvasRT;

		// Token: 0x04006D3C RID: 27964
		private ScrollRect _scrollRect;

		// Token: 0x04006D3D RID: 27965
		private List<DropDownListButton> _panelItems;

		// Token: 0x04006D3E RID: 27966
		private GameObject _itemTemplate;

		// Token: 0x04006D3F RID: 27967
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04006D40 RID: 27968
		private int _selectedIndex = -1;

		// Token: 0x04006D41 RID: 27969
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04006D42 RID: 27970
		public bool SelectFirstItemOnStart;

		// Token: 0x04006D43 RID: 27971
		public DropDownList.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x02000BA0 RID: 2976
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<int>
		{
		}
	}
}
