using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000B85 RID: 2949
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/BoxSlider")]
	public class BoxSlider : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement, IEventSystemHandler
	{
		// Token: 0x060044F7 RID: 17655 RVA: 0x0015DE54 File Offset: 0x0015C254
		protected BoxSlider()
		{
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060044F8 RID: 17656 RVA: 0x0015DE93 File Offset: 0x0015C293
		// (set) Token: 0x060044F9 RID: 17657 RVA: 0x0015DE9B File Offset: 0x0015C29B
		public RectTransform HandleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (BoxSlider.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x0015DEBA File Offset: 0x0015C2BA
		// (set) Token: 0x060044FB RID: 17659 RVA: 0x0015DEC2 File Offset: 0x0015C2C2
		public float MinValue
		{
			get
			{
				return this.m_MinValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x0015DEF3 File Offset: 0x0015C2F3
		// (set) Token: 0x060044FD RID: 17661 RVA: 0x0015DEFB File Offset: 0x0015C2FB
		public float MaxValue
		{
			get
			{
				return this.m_MaxValue;
			}
			set
			{
				if (BoxSlider.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060044FE RID: 17662 RVA: 0x0015DF2C File Offset: 0x0015C32C
		// (set) Token: 0x060044FF RID: 17663 RVA: 0x0015DF34 File Offset: 0x0015C334
		public bool WholeNumbers
		{
			get
			{
				return this.m_WholeNumbers;
			}
			set
			{
				if (BoxSlider.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.SetX(this.m_ValueX);
					this.SetY(this.m_ValueY);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06004500 RID: 17664 RVA: 0x0015DF65 File Offset: 0x0015C365
		// (set) Token: 0x06004501 RID: 17665 RVA: 0x0015DF84 File Offset: 0x0015C384
		public float ValueX
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueX);
				}
				return this.m_ValueX;
			}
			set
			{
				this.SetX(value);
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06004502 RID: 17666 RVA: 0x0015DF8D File Offset: 0x0015C38D
		// (set) Token: 0x06004503 RID: 17667 RVA: 0x0015DFC2 File Offset: 0x0015C3C2
		public float NormalizedValueX
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueX);
			}
			set
			{
				this.ValueX = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06004504 RID: 17668 RVA: 0x0015DFDC File Offset: 0x0015C3DC
		// (set) Token: 0x06004505 RID: 17669 RVA: 0x0015DFFB File Offset: 0x0015C3FB
		public float ValueY
		{
			get
			{
				if (this.WholeNumbers)
				{
					return Mathf.Round(this.m_ValueY);
				}
				return this.m_ValueY;
			}
			set
			{
				this.SetY(value);
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x0015E004 File Offset: 0x0015C404
		// (set) Token: 0x06004507 RID: 17671 RVA: 0x0015E039 File Offset: 0x0015C439
		public float NormalizedValueY
		{
			get
			{
				if (Mathf.Approximately(this.MinValue, this.MaxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.MinValue, this.MaxValue, this.ValueY);
			}
			set
			{
				this.ValueY = Mathf.Lerp(this.MinValue, this.MaxValue, value);
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06004508 RID: 17672 RVA: 0x0015E053 File Offset: 0x0015C453
		// (set) Token: 0x06004509 RID: 17673 RVA: 0x0015E05B File Offset: 0x0015C45B
		public BoxSlider.BoxSliderEvent OnValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x0600450A RID: 17674 RVA: 0x0015E064 File Offset: 0x0015C464
		private float StepSize
		{
			get
			{
				return (!this.WholeNumbers) ? ((this.MaxValue - this.MinValue) * 0.1f) : 1f;
			}
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x0015E08E File Offset: 0x0015C48E
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x0015E090 File Offset: 0x0015C490
		public void LayoutComplete()
		{
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x0015E092 File Offset: 0x0015C492
		public void GraphicUpdateComplete()
		{
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x0015E094 File Offset: 0x0015C494
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x0015E0ED File Offset: 0x0015C4ED
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x0015E110 File Offset: 0x0015C510
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.SetX(this.m_ValueX, false);
			this.SetY(this.m_ValueY, false);
			this.UpdateVisuals();
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x0015E13E File Offset: 0x0015C53E
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0015E154 File Offset: 0x0015C554
		private void UpdateCachedReferences()
		{
			if (this.m_HandleRect)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_HandleContainerRect = null;
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x0015E1BA File Offset: 0x0015C5BA
		private void SetX(float input)
		{
			this.SetX(input, true);
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x0015E1C4 File Offset: 0x0015C5C4
		private void SetX(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueX == num)
			{
				return;
			}
			this.m_ValueX = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(num, this.ValueY);
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0015E228 File Offset: 0x0015C628
		private void SetY(float input)
		{
			this.SetY(input, true);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0015E234 File Offset: 0x0015C634
		private void SetY(float input, bool sendCallback)
		{
			float num = Mathf.Clamp(input, this.MinValue, this.MaxValue);
			if (this.WholeNumbers)
			{
				num = Mathf.Round(num);
			}
			if (this.m_ValueY == num)
			{
				return;
			}
			this.m_ValueY = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(this.ValueX, num);
			}
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x0015E298 File Offset: 0x0015C698
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.UpdateVisuals();
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x0015E2A8 File Offset: 0x0015C6A8
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				int index = 0;
				float value = this.NormalizedValueX;
				one[0] = value;
				zero[index] = value;
				int index2 = 1;
				value = this.NormalizedValueY;
				one[1] = value;
				zero[index2] = value;
				this.m_HandleRect.anchorMin = zero;
				this.m_HandleRect.anchorMax = one;
			}
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x0015E340 File Offset: 0x0015C740
		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform handleContainerRect = this.m_HandleContainerRect;
			if (handleContainerRect != null && handleContainerRect.rect.size[0] > 0f)
			{
				Vector2 a;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(handleContainerRect, eventData.position, cam, out a))
				{
					return;
				}
				a -= handleContainerRect.rect.position;
				float normalizedValueX = Mathf.Clamp01((a - this.m_Offset)[0] / handleContainerRect.rect.size[0]);
				this.NormalizedValueX = normalizedValueX;
				float normalizedValueY = Mathf.Clamp01((a - this.m_Offset)[1] / handleContainerRect.rect.size[1]);
				this.NormalizedValueY = normalizedValueY;
			}
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x0015E42A File Offset: 0x0015C82A
		private bool CanDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x0015E450 File Offset: 0x0015C850
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera))
			{
				Vector2 offset;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out offset))
				{
					this.m_Offset = offset;
				}
				this.m_Offset.y = -this.m_Offset.y;
			}
			else
			{
				this.UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0015E4F7 File Offset: 0x0015C8F7
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x0015E513 File Offset: 0x0015C913
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x0015E51C File Offset: 0x0015C91C
		// Transform ICanvasElement.get_transform()
		// {
		// 	return base.transform;
		// }

		// Token: 0x0600451F RID: 17695 RVA: 0x0015E524 File Offset: 0x0015C924
		bool ICanvasElement.IsDestroyed()
		{
			return base.IsDestroyed();
		}

		// Token: 0x04006CA8 RID: 27816
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x04006CA9 RID: 27817
		[Space(6f)]
		[SerializeField]
		private float m_MinValue;

		// Token: 0x04006CAA RID: 27818
		[SerializeField]
		private float m_MaxValue = 1f;

		// Token: 0x04006CAB RID: 27819
		[SerializeField]
		private bool m_WholeNumbers;

		// Token: 0x04006CAC RID: 27820
		[SerializeField]
		private float m_ValueX = 1f;

		// Token: 0x04006CAD RID: 27821
		[SerializeField]
		private float m_ValueY = 1f;

		// Token: 0x04006CAE RID: 27822
		[Space(6f)]
		[SerializeField]
		private BoxSlider.BoxSliderEvent m_OnValueChanged = new BoxSlider.BoxSliderEvent();

		// Token: 0x04006CAF RID: 27823
		private Transform m_HandleTransform;

		// Token: 0x04006CB0 RID: 27824
		private RectTransform m_HandleContainerRect;

		// Token: 0x04006CB1 RID: 27825
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04006CB2 RID: 27826
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x02000B86 RID: 2950
		public enum Direction
		{
			// Token: 0x04006CB4 RID: 27828
			LeftToRight,
			// Token: 0x04006CB5 RID: 27829
			RightToLeft,
			// Token: 0x04006CB6 RID: 27830
			BottomToTop,
			// Token: 0x04006CB7 RID: 27831
			TopToBottom
		}

		// Token: 0x02000B87 RID: 2951
		[Serializable]
		public class BoxSliderEvent : UnityEvent<float, float>
		{
		}

		// Token: 0x02000B88 RID: 2952
		private enum Axis
		{
			// Token: 0x04006CB9 RID: 27833
			Horizontal,
			// Token: 0x04006CBA RID: 27834
			Vertical
		}
	}
}
