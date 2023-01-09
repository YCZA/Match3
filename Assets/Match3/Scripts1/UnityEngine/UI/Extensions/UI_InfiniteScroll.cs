using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C32 RID: 3122
	[AddComponentMenu("UI/Extensions/UI Infinite Scroll")]
	public class UI_InfiniteScroll : MonoBehaviour
	{
		// Token: 0x060049A2 RID: 18850 RVA: 0x001784A5 File Offset: 0x001768A5
		private void Awake()
		{
			if (!this.InitByUser)
			{
				this.Init();
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x001784B8 File Offset: 0x001768B8
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
				for (int i = 0; i < this._scrollRect.content.childCount; i++)
				{
					this.items.Add(this._scrollRect.content.GetChild(i).GetComponent<RectTransform>());
				}
				if (this._scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
				{
					this._verticalLayoutGroup = this._scrollRect.content.GetComponent<VerticalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
				{
					this._horizontalLayoutGroup = this._scrollRect.content.GetComponent<HorizontalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<GridLayoutGroup>() != null)
				{
					this._gridLayoutGroup = this._scrollRect.content.GetComponent<GridLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<ContentSizeFitter>() != null)
				{
					this._contentSizeFitter = this._scrollRect.content.GetComponent<ContentSizeFitter>();
				}
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
				if (this._isHorizontal && this._isVertical)
				{
					global::UnityEngine.Debug.LogError("UI_InfiniteScroll doesn't support scrolling in both directions, plase choose one direction (horizontal or vertical)");
				}
				this._itemCount = this._scrollRect.content.childCount;
			}
			else
			{
				global::UnityEngine.Debug.LogError("UI_InfiniteScroll => No ScrollRect component found");
			}
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00178678 File Offset: 0x00176A78
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._recordOffsetY = this.items[0].GetComponent<RectTransform>().anchoredPosition.y - this.items[1].GetComponent<RectTransform>().anchoredPosition.y;
				this._disableMarginY = this._recordOffsetY * (float)this._itemCount / 2f;
			}
			if (this._isHorizontal)
			{
				this._recordOffsetX = this.items[1].GetComponent<RectTransform>().anchoredPosition.x - this.items[0].GetComponent<RectTransform>().anchoredPosition.x;
				this._disableMarginX = this._recordOffsetX * (float)this._itemCount / 2f;
			}
			if (this._verticalLayoutGroup)
			{
				this._verticalLayoutGroup.enabled = false;
			}
			if (this._horizontalLayoutGroup)
			{
				this._horizontalLayoutGroup.enabled = false;
			}
			if (this._contentSizeFitter)
			{
				this._contentSizeFitter.enabled = false;
			}
			if (this._gridLayoutGroup)
			{
				this._gridLayoutGroup.enabled = false;
			}
			this._hasDisabledGridComponents = true;
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x001787CC File Offset: 0x00176BCC
		public void OnScroll(Vector2 pos)
		{
			if (!this._hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x > this._disableMarginX + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x - (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x < -this._disableMarginX)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x + (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
				if (this._isVertical)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y > this._disableMarginY + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y - (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y < -this._disableMarginY)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y + (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
			}
		}

		// Token: 0x04006FE3 RID: 28643
		[Tooltip("If false, will Init automatically, otherwise you need to call Init() method")]
		public bool InitByUser;

		// Token: 0x04006FE4 RID: 28644
		private ScrollRect _scrollRect;

		// Token: 0x04006FE5 RID: 28645
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04006FE6 RID: 28646
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04006FE7 RID: 28647
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04006FE8 RID: 28648
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04006FE9 RID: 28649
		private bool _isVertical;

		// Token: 0x04006FEA RID: 28650
		private bool _isHorizontal;

		// Token: 0x04006FEB RID: 28651
		private float _disableMarginX;

		// Token: 0x04006FEC RID: 28652
		private float _disableMarginY;

		// Token: 0x04006FED RID: 28653
		private bool _hasDisabledGridComponents;

		// Token: 0x04006FEE RID: 28654
		private List<RectTransform> items = new List<RectTransform>();

		// Token: 0x04006FEF RID: 28655
		private Vector2 _newAnchoredPosition = Vector2.zero;

		// Token: 0x04006FF0 RID: 28656
		private float _treshold = 100f;

		// Token: 0x04006FF1 RID: 28657
		private int _itemCount;

		// Token: 0x04006FF2 RID: 28658
		private float _recordOffsetX;

		// Token: 0x04006FF3 RID: 28659
		private float _recordOffsetY;
	}
}
