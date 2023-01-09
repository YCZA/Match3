using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B9D RID: 2973
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/ComboBox")]
	public class ComboBox : MonoBehaviour
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x00160BD8 File Offset: 0x0015EFD8
		// (set) Token: 0x06004598 RID: 17816 RVA: 0x00160BE0 File Offset: 0x0015EFE0
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06004599 RID: 17817 RVA: 0x00160BE9 File Offset: 0x0015EFE9
		// (set) Token: 0x0600459A RID: 17818 RVA: 0x00160BF1 File Offset: 0x0015EFF1
		public string Text { get; private set; }

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x00160BFA File Offset: 0x0015EFFA
		// (set) Token: 0x0600459C RID: 17820 RVA: 0x00160C02 File Offset: 0x0015F002
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

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x00160C11 File Offset: 0x0015F011
		// (set) Token: 0x0600459E RID: 17822 RVA: 0x00160C19 File Offset: 0x0015F019
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

		// Token: 0x0600459F RID: 17823 RVA: 0x00160C28 File Offset: 0x0015F028
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00160C34 File Offset: 0x0015F034
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._inputRT = this._rectTransform.Find("InputField").GetComponent<RectTransform>();
				this._mainInput = this._inputRT.GetComponent<InputField>();
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
				this.itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this.itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				global::UnityEngine.Debug.LogException(exception);
				global::UnityEngine.Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Refernece Exception");
				result = false;
			}
			this.panelObjects = new Dictionary<string, GameObject>();
			this._panelItems = this.AvailableOptions.ToList<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00160E08 File Offset: 0x0015F208
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			this._panelItems.Sort();
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
			this.panelObjects.Clear();
			int num = 0;
			while (list.Count < this.AvailableOptions.Count)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				list.Add(gameObject);
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetActive(i <= this.AvailableOptions.Count);
				if (i < this.AvailableOptions.Count)
				{
					list[i].name = string.Concat(new object[]
					{
						"Item ",
						i,
						" ",
						this._panelItems[i]
					});
					list[i].transform.Find("Text").GetComponent<Text>().text = this._panelItems[i];
					Button component = list[i].GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					string textOfItem = this._panelItems[i];
					component.onClick.AddListener(delegate()
					{
						this.OnItemClicked(textOfItem);
					});
					this.panelObjects[this._panelItems[i]] = list[i];
				}
			}
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00161034 File Offset: 0x0015F434
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x00161058 File Offset: 0x0015F458
		private void RedrawPanel()
		{
			float num = (this._panelItems.Count <= this.ItemsToDisplay) ? 0f : this._scrollBarWidth;
			this._scrollBarRT.gameObject.SetActive(this._panelItems.Count > this.ItemsToDisplay);
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._inputRT.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this._panelItems.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this._panelItems.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001612D4 File Offset: 0x0015F6D4
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.RedrawPanel();
			if (this._panelItems.Count == 0)
			{
				this._isPanelActive = true;
				this.ToggleDropdownPanel(false);
			}
			else if (!this._isPanelActive)
			{
				this.ToggleDropdownPanel(false);
			}
			this.OnSelectionChanged.Invoke(this.Text);
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x00161334 File Offset: 0x0015F734
		public void ToggleDropdownPanel(bool directClick)
		{
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

		// Token: 0x04006D16 RID: 27926
		public Color disabledTextColor;

		// Token: 0x04006D18 RID: 27928
		public List<string> AvailableOptions;

		// Token: 0x04006D19 RID: 27929
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04006D1A RID: 27930
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04006D1B RID: 27931
		public ComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x04006D1C RID: 27932
		private bool _isPanelActive;

		// Token: 0x04006D1D RID: 27933
		private bool _hasDrawnOnce;

		// Token: 0x04006D1E RID: 27934
		private InputField _mainInput;

		// Token: 0x04006D1F RID: 27935
		private RectTransform _inputRT;

		// Token: 0x04006D20 RID: 27936
		private RectTransform _rectTransform;

		// Token: 0x04006D21 RID: 27937
		private RectTransform _overlayRT;

		// Token: 0x04006D22 RID: 27938
		private RectTransform _scrollPanelRT;

		// Token: 0x04006D23 RID: 27939
		private RectTransform _scrollBarRT;

		// Token: 0x04006D24 RID: 27940
		private RectTransform _slidingAreaRT;

		// Token: 0x04006D25 RID: 27941
		private RectTransform _itemsPanelRT;

		// Token: 0x04006D26 RID: 27942
		private Canvas _canvas;

		// Token: 0x04006D27 RID: 27943
		private RectTransform _canvasRT;

		// Token: 0x04006D28 RID: 27944
		private ScrollRect _scrollRect;

		// Token: 0x04006D29 RID: 27945
		private List<string> _panelItems;

		// Token: 0x04006D2A RID: 27946
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x04006D2B RID: 27947
		private GameObject itemTemplate;

		// Token: 0x02000B9E RID: 2974
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string>
		{
		}
	}
}
