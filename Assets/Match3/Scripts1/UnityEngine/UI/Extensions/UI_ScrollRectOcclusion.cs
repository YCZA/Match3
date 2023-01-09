using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000C33 RID: 3123
	[AddComponentMenu("UI/Extensions/UI Scrollrect Occlusion")]
	public class UI_ScrollRectOcclusion : MonoBehaviour
	{
		// Token: 0x060049A7 RID: 18855 RVA: 0x00178AF4 File Offset: 0x00176EF4
		private void Awake()
		{
			if (this.InitByUser)
			{
				return;
			}
			this.Init();
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x00178B08 File Offset: 0x00176F08
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
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
			}
			else
			{
				global::UnityEngine.Debug.LogError("UI_ScrollRectOcclusion => No ScrollRect component found");
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00178C88 File Offset: 0x00177088
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._disableMarginY = this._scrollRect.GetComponent<RectTransform>().rect.height / 2f + this.items[0].sizeDelta.y;
			}
			if (this._isHorizontal)
			{
				this._disableMarginX = this._scrollRect.GetComponent<RectTransform>().rect.width / 2f + this.items[0].sizeDelta.x;
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
			this.hasDisabledGridComponents = true;
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x00178DA0 File Offset: 0x001771A0
		public void OnScroll(Vector2 pos)
		{
			if (!this.hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isVertical && this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
					{
						this.items[i].gameObject.SetActive(false);
					}
					else
					{
						this.items[i].gameObject.SetActive(true);
					}
				}
				else
				{
					if (this._isVertical)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
					if (this._isHorizontal)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
				}
			}
		}

		// Token: 0x04006FF4 RID: 28660
		public bool InitByUser;

		// Token: 0x04006FF5 RID: 28661
		private ScrollRect _scrollRect;

		// Token: 0x04006FF6 RID: 28662
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04006FF7 RID: 28663
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04006FF8 RID: 28664
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04006FF9 RID: 28665
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04006FFA RID: 28666
		private bool _isVertical;

		// Token: 0x04006FFB RID: 28667
		private bool _isHorizontal;

		// Token: 0x04006FFC RID: 28668
		private float _disableMarginX;

		// Token: 0x04006FFD RID: 28669
		private float _disableMarginY;

		// Token: 0x04006FFE RID: 28670
		private bool hasDisabledGridComponents;

		// Token: 0x04006FFF RID: 28671
		private List<RectTransform> items = new List<RectTransform>();
	}
}
