using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BFD RID: 3069
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Extensions/Tile Size Fitter")]
	public class TileSizeFitter : UIBehaviour, ILayoutSelfController, ILayoutController
	{
		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06004814 RID: 18452 RVA: 0x0016FF86 File Offset: 0x0016E386
		// (set) Token: 0x06004815 RID: 18453 RVA: 0x0016FF8E File Offset: 0x0016E38E
		public Vector2 Border
		{
			get
			{
				return this.m_Border;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_Border, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06004816 RID: 18454 RVA: 0x0016FFA7 File Offset: 0x0016E3A7
		// (set) Token: 0x06004817 RID: 18455 RVA: 0x0016FFAF File Offset: 0x0016E3AF
		public Vector2 TileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Vector2>(ref this.m_TileSize, value))
				{
					this.SetDirty();
				}
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004818 RID: 18456 RVA: 0x0016FFC8 File Offset: 0x0016E3C8
		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0016FFED File Offset: 0x0016E3ED
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetDirty();
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0016FFFB File Offset: 0x0016E3FB
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00170019 File Offset: 0x0016E419
		protected override void OnRectTransformDimensionsChange()
		{
			this.UpdateRect();
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x00170024 File Offset: 0x0016E424
		private void UpdateRect()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.m_Tracker.Clear();
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY);
			this.rectTransform.anchorMin = Vector2.zero;
			this.rectTransform.anchorMax = Vector2.one;
			this.rectTransform.anchoredPosition = Vector2.zero;
			this.m_Tracker.Add(this, this.rectTransform, DrivenTransformProperties.SizeDelta);
			Vector2 a = this.GetParentSize() - this.Border;
			if (this.TileSize.x > 0.001f)
			{
				a.x -= Mathf.Floor(a.x / this.TileSize.x) * this.TileSize.x;
			}
			else
			{
				a.x = 0f;
			}
			if (this.TileSize.y > 0.001f)
			{
				a.y -= Mathf.Floor(a.y / this.TileSize.y) * this.TileSize.y;
			}
			else
			{
				a.y = 0f;
			}
			this.rectTransform.sizeDelta = -a;
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x00170190 File Offset: 0x0016E590
		private Vector2 GetParentSize()
		{
			RectTransform rectTransform = this.rectTransform.parent as RectTransform;
			if (!rectTransform)
			{
				return Vector2.zero;
			}
			return rectTransform.rect.size;
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x001701CD File Offset: 0x0016E5CD
		public virtual void SetLayoutHorizontal()
		{
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x001701CF File Offset: 0x0016E5CF
		public virtual void SetLayoutVertical()
		{
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x001701D1 File Offset: 0x0016E5D1
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateRect();
		}

		// Token: 0x04006F02 RID: 28418
		[SerializeField]
		private Vector2 m_Border = Vector2.zero;

		// Token: 0x04006F03 RID: 28419
		[SerializeField]
		private Vector2 m_TileSize = Vector2.zero;

		// Token: 0x04006F04 RID: 28420
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x04006F05 RID: 28421
		private DrivenRectTransformTracker m_Tracker;
	}
}
