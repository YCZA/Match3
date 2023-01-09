using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BFE RID: 3070
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroller")]
	public class UIVerticalScroller : MonoBehaviour
	{
		// Token: 0x06004821 RID: 18465 RVA: 0x001701E5 File Offset: 0x0016E5E5
		public UIVerticalScroller()
		{
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x001701F4 File Offset: 0x0016E5F4
		public UIVerticalScroller(RectTransform scrollingPanel, GameObject[] arrayOfElements, RectTransform center)
		{
			this._scrollingPanel = scrollingPanel;
			this._arrayOfElements = arrayOfElements;
			this._center = center;
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x00170218 File Offset: 0x0016E618
		public void Awake()
		{
			ScrollRect component = base.GetComponent<ScrollRect>();
			if (!this._scrollingPanel)
			{
				this._scrollingPanel = component.content;
			}
			if (!this._center)
			{
				global::UnityEngine.Debug.LogError("Please define the RectTransform for the Center viewport of the scrollable area");
			}
			if (this._arrayOfElements == null || this._arrayOfElements.Length == 0)
			{
				int childCount = component.content.childCount;
				if (childCount > 0)
				{
					this._arrayOfElements = new GameObject[childCount];
					for (int i = 0; i < childCount; i++)
					{
						this._arrayOfElements[i] = component.content.GetChild(i).gameObject;
					}
				}
			}
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x001702C4 File Offset: 0x0016E6C4
		public void Start()
		{
			if (this._arrayOfElements.Length < 1)
			{
				global::UnityEngine.Debug.Log("No child content found, exiting..");
				return;
			}
			this.elementLength = this._arrayOfElements.Length;
			this.distance = new float[this.elementLength];
			this.distReposition = new float[this.elementLength];
			this.deltaY = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)this.elementLength / 3f * 2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -this.deltaY);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
			for (int i = 0; i < this._arrayOfElements.Length; i++)
			{
				this.AddListener(this._arrayOfElements[i], i);
			}
			if (this.ScrollUpButton)
			{
				this.ScrollUpButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ScrollUp();
				});
			}
			if (this.ScrollDownButton)
			{
				this.ScrollDownButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.ScrollDown();
				});
			}
			if (this.StartingIndex > -1)
			{
				this.StartingIndex = ((this.StartingIndex <= this._arrayOfElements.Length) ? this.StartingIndex : (this._arrayOfElements.Length - 1));
				this.SnapToElement(this.StartingIndex);
			}
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0017044C File Offset: 0x0016E84C
		private void AddListener(GameObject button, int index)
		{
			button.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.DoSomething(index);
			});
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x00170489 File Offset: 0x0016E889
		private void DoSomething(int index)
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked.Invoke(index);
			}
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x001704A4 File Offset: 0x0016E8A4
		public void Update()
		{
			if (this._arrayOfElements.Length < 1)
			{
				return;
			}
			for (int i = 0; i < this.elementLength; i++)
			{
				this.distReposition[i] = this._center.GetComponent<RectTransform>().position.y - this._arrayOfElements[i].GetComponent<RectTransform>().position.y;
				this.distance[i] = Mathf.Abs(this.distReposition[i]);
				float num = Mathf.Max(0.7f, 1f / (1f + this.distance[i] / 200f));
				this._arrayOfElements[i].GetComponent<RectTransform>().transform.localScale = new Vector3(num, num, 1f);
			}
			float num2 = Mathf.Min(this.distance);
			for (int j = 0; j < this.elementLength; j++)
			{
				this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = false;
				if (num2 == this.distance[j])
				{
					this.minElementsNum = j;
					this._arrayOfElements[j].GetComponent<CanvasGroup>().interactable = true;
					this.result = this._arrayOfElements[j].GetComponentInChildren<Text>().text;
				}
			}
			this.ScrollingElements(-this._arrayOfElements[this.minElementsNum].GetComponent<RectTransform>().anchoredPosition.y);
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x00170618 File Offset: 0x0016EA18
		private void ScrollingElements(float position)
		{
			float y = Mathf.Lerp(this._scrollingPanel.anchoredPosition.y, position, Time.deltaTime * 1f);
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, y);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x00170671 File Offset: 0x0016EA71
		public string GetResults()
		{
			return this.result;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0017067C File Offset: 0x0016EA7C
		public void SnapToElement(int element)
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height * (float)element;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, -num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x001706D0 File Offset: 0x0016EAD0
		public void ScrollUp()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 b = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y - num);
			this._scrollingPanel.anchoredPosition = Vector2.Lerp(this._scrollingPanel.anchoredPosition, b, 1f);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00170750 File Offset: 0x0016EB50
		public void ScrollDown()
		{
			float num = this._arrayOfElements[0].GetComponent<RectTransform>().rect.height / 1.2f;
			Vector2 anchoredPosition = new Vector2(this._scrollingPanel.anchoredPosition.x, this._scrollingPanel.anchoredPosition.y + num);
			this._scrollingPanel.anchoredPosition = anchoredPosition;
		}

		// Token: 0x04006F06 RID: 28422
		[Tooltip("Scrollable area (content of desired ScrollRect)")]
		public RectTransform _scrollingPanel;

		// Token: 0x04006F07 RID: 28423
		[Tooltip("Elements to populate inside the scroller")]
		public GameObject[] _arrayOfElements;

		// Token: 0x04006F08 RID: 28424
		[Tooltip("Center display area (position of zoomed content)")]
		public RectTransform _center;

		// Token: 0x04006F09 RID: 28425
		[Tooltip("Select the item to be in center on start. (optional)")]
		public int StartingIndex = -1;

		// Token: 0x04006F0A RID: 28426
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject ScrollUpButton;

		// Token: 0x04006F0B RID: 28427
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject ScrollDownButton;

		// Token: 0x04006F0C RID: 28428
		[Tooltip("Event fired when a specific item is clicked, exposes index number of item. (optional)")]
		public UnityEvent<int> ButtonClicked;

		// Token: 0x04006F0D RID: 28429
		private float[] distReposition;

		// Token: 0x04006F0E RID: 28430
		private float[] distance;

		// Token: 0x04006F0F RID: 28431
		private int minElementsNum;

		// Token: 0x04006F10 RID: 28432
		private int elementLength;

		// Token: 0x04006F11 RID: 28433
		private float deltaY;

		// Token: 0x04006F12 RID: 28434
		private string result;
	}
}
