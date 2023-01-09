using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B99 RID: 2969
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/AutoComplete ComboBox")]
	public class AutoCompleteComboBox : MonoBehaviour
	{
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x0600457D RID: 17789 RVA: 0x00160056 File Offset: 0x0015E456
		// (set) Token: 0x0600457E RID: 17790 RVA: 0x0016005E File Offset: 0x0015E45E
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x0600457F RID: 17791 RVA: 0x00160067 File Offset: 0x0015E467
		// (set) Token: 0x06004580 RID: 17792 RVA: 0x0016006F File Offset: 0x0015E46F
		public string Text { get; private set; }

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06004581 RID: 17793 RVA: 0x00160078 File Offset: 0x0015E478
		// (set) Token: 0x06004582 RID: 17794 RVA: 0x00160080 File Offset: 0x0015E480
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

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06004583 RID: 17795 RVA: 0x0016008F File Offset: 0x0015E48F
		// (set) Token: 0x06004584 RID: 17796 RVA: 0x00160097 File Offset: 0x0015E497
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

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x001600A6 File Offset: 0x0015E4A6
		// (set) Token: 0x06004586 RID: 17798 RVA: 0x001600AE File Offset: 0x0015E4AE
		public bool InputColorMatching
		{
			get
			{
				return this._ChangeInputTextColorBasedOnMatchingItems;
			}
			set
			{
				this._ChangeInputTextColorBasedOnMatchingItems = value;
				if (this._ChangeInputTextColorBasedOnMatchingItems)
				{
					this.SetInputTextColor();
				}
			}
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x001600C8 File Offset: 0x0015E4C8
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x001600D1 File Offset: 0x0015E4D1
		public void Start()
		{
			if (this.SelectFirstItemOnStart && this.AvailableOptions.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(this.AvailableOptions[0]);
			}
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00160108 File Offset: 0x0015E508
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
			this._prunedPanelItems = new List<string>();
			this._panelItems = new List<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x001602E0 File Offset: 0x0015E6E0
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			this._prunedPanelItems.Clear();
			this.panelObjects.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
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
			this.SetInputTextColor();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x00160514 File Offset: 0x0015E914
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00160538 File Offset: 0x0015E938
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

		// Token: 0x0600458D RID: 17805 RVA: 0x001607B4 File Offset: 0x0015EBB4
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.PruneItems(currText);
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
			bool flag = this._panelItems.Contains(this.Text) != this._selectionIsValid;
			this._selectionIsValid = this._panelItems.Contains(this.Text);
			this.OnSelectionChanged.Invoke(this.Text, this._selectionIsValid);
			this.OnSelectionTextChanged.Invoke(this.Text);
			if (flag)
			{
				this.OnSelectionValidityChanged.Invoke(this._selectionIsValid);
			}
			this.SetInputTextColor();
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00160884 File Offset: 0x0015EC84
		private void SetInputTextColor()
		{
			if (this.InputColorMatching)
			{
				if (this._selectionIsValid)
				{
					this._mainInput.textComponent.color = this.ValidSelectionTextColor;
				}
				else if (this._panelItems.Count > 0)
				{
					this._mainInput.textComponent.color = this.MatchingItemsRemainingTextColor;
				}
				else
				{
					this._mainInput.textComponent.color = this.NoItemsRemainingTextColor;
				}
			}
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00160904 File Offset: 0x0015ED04
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

		// Token: 0x06004590 RID: 17808 RVA: 0x00160957 File Offset: 0x0015ED57
		private void PruneItems(string currText)
		{
			if (this.autocompleteSearchType == AutoCompleteSearchType.Linq)
			{
				this.PruneItemsLinq(currText);
			}
			else
			{
				this.PruneItemsArray(currText);
			}
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00160978 File Offset: 0x0015ED78
		private void PruneItemsLinq(string currText)
		{
			currText = currText.ToLower();
			string[] array = (from x in this._panelItems
			where !x.Contains(currText)
			select x).ToArray<string>();
			foreach (string text in array)
			{
				this.panelObjects[text].SetActive(false);
				this._panelItems.Remove(text);
				this._prunedPanelItems.Add(text);
			}
			string[] array3 = (from x in this._prunedPanelItems
			where x.Contains(currText)
			select x).ToArray<string>();
			foreach (string text2 in array3)
			{
				this.panelObjects[text2].SetActive(true);
				this._panelItems.Add(text2);
				this._prunedPanelItems.Remove(text2);
			}
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x00160A7C File Offset: 0x0015EE7C
		private void PruneItemsArray(string currText)
		{
			string value = currText.ToLower();
			for (int i = this._panelItems.Count - 1; i >= 0; i--)
			{
				string text = this._panelItems[i];
				if (!text.Contains(value))
				{
					this.panelObjects[this._panelItems[i]].SetActive(false);
					this._panelItems.RemoveAt(i);
					this._prunedPanelItems.Add(text);
				}
			}
			for (int j = this._prunedPanelItems.Count - 1; j >= 0; j--)
			{
				string text2 = this._prunedPanelItems[j];
				if (text2.Contains(value))
				{
					this.panelObjects[this._prunedPanelItems[j]].SetActive(true);
					this._prunedPanelItems.RemoveAt(j);
					this._panelItems.Add(text2);
				}
			}
		}

		// Token: 0x04006CF5 RID: 27893
		public Color disabledTextColor;

		// Token: 0x04006CF7 RID: 27895
		public List<string> AvailableOptions;

		// Token: 0x04006CF8 RID: 27896
		private bool _isPanelActive;

		// Token: 0x04006CF9 RID: 27897
		private bool _hasDrawnOnce;

		// Token: 0x04006CFA RID: 27898
		private InputField _mainInput;

		// Token: 0x04006CFB RID: 27899
		private RectTransform _inputRT;

		// Token: 0x04006CFC RID: 27900
		private RectTransform _rectTransform;

		// Token: 0x04006CFD RID: 27901
		private RectTransform _overlayRT;

		// Token: 0x04006CFE RID: 27902
		private RectTransform _scrollPanelRT;

		// Token: 0x04006CFF RID: 27903
		private RectTransform _scrollBarRT;

		// Token: 0x04006D00 RID: 27904
		private RectTransform _slidingAreaRT;

		// Token: 0x04006D01 RID: 27905
		private RectTransform _itemsPanelRT;

		// Token: 0x04006D02 RID: 27906
		private Canvas _canvas;

		// Token: 0x04006D03 RID: 27907
		private RectTransform _canvasRT;

		// Token: 0x04006D04 RID: 27908
		private ScrollRect _scrollRect;

		// Token: 0x04006D05 RID: 27909
		private List<string> _panelItems;

		// Token: 0x04006D06 RID: 27910
		private List<string> _prunedPanelItems;

		// Token: 0x04006D07 RID: 27911
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x04006D08 RID: 27912
		private GameObject itemTemplate;

		// Token: 0x04006D0A RID: 27914
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04006D0B RID: 27915
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04006D0C RID: 27916
		public bool SelectFirstItemOnStart;

		// Token: 0x04006D0D RID: 27917
		[SerializeField]
		[Tooltip("Change input text color based on matching items")]
		private bool _ChangeInputTextColorBasedOnMatchingItems;

		// Token: 0x04006D0E RID: 27918
		public Color ValidSelectionTextColor = Color.green;

		// Token: 0x04006D0F RID: 27919
		public Color MatchingItemsRemainingTextColor = Color.black;

		// Token: 0x04006D10 RID: 27920
		public Color NoItemsRemainingTextColor = Color.red;

		// Token: 0x04006D11 RID: 27921
		public AutoCompleteSearchType autocompleteSearchType = AutoCompleteSearchType.Linq;

		// Token: 0x04006D12 RID: 27922
		private bool _selectionIsValid;

		// Token: 0x04006D13 RID: 27923
		public AutoCompleteComboBox.SelectionTextChangedEvent OnSelectionTextChanged;

		// Token: 0x04006D14 RID: 27924
		public AutoCompleteComboBox.SelectionValidityChangedEvent OnSelectionValidityChanged;

		// Token: 0x04006D15 RID: 27925
		public AutoCompleteComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x02000B9A RID: 2970
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string, bool>
		{
		}

		// Token: 0x02000B9B RID: 2971
		[Serializable]
		public class SelectionTextChangedEvent : UnityEvent<string>
		{
		}

		// Token: 0x02000B9C RID: 2972
		[Serializable]
		public class SelectionValidityChangedEvent : UnityEvent<bool>
		{
		}
	}
}
