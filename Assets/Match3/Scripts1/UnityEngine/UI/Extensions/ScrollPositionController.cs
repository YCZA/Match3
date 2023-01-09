using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BED RID: 3053
	public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x060047A8 RID: 18344 RVA: 0x0016E5F0 File Offset: 0x0016C9F0
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.pointerStartLocalPosition = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out this.pointerStartLocalPosition);
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.dragging = true;
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x0016E648 File Offset: 0x0016CA48
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.dragging)
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewport, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			Vector2 vector = a - this.pointerStartLocalPosition;
			float num = ((this.directionOfRecognize != ScrollPositionController.ScrollDirection.Horizontal) ? vector.y : (-vector.x)) / this.GetViewportSize() * this.scrollSensitivity + this.dragStartScrollPosition;
			float num2 = this.CalculateOffset(num);
			num += num2;
			if (this.movementType == ScrollPositionController.MovementType.Elastic && num2 != 0f)
			{
				num -= this.RubberDelta(num2, this.scrollSensitivity);
			}
			this.UpdatePosition(num);
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0016E709 File Offset: 0x0016CB09
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.dragging = false;
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0016E720 File Offset: 0x0016CB20
		private float GetViewportSize()
		{
			return (this.directionOfRecognize != ScrollPositionController.ScrollDirection.Horizontal) ? this.viewport.rect.size.y : this.viewport.rect.size.x;
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0016E774 File Offset: 0x0016CB74
		private float CalculateOffset(float position)
		{
			if (this.movementType == ScrollPositionController.MovementType.Unrestricted)
			{
				return 0f;
			}
			if (position < 0f)
			{
				return -position;
			}
			if (position > (float)(this.dataCount - 1))
			{
				return (float)(this.dataCount - 1) - position;
			}
			return 0f;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0016E7C0 File Offset: 0x0016CBC0
		private void UpdatePosition(float position)
		{
			this.currentScrollPosition = position;
			if (this.OnUpdatePosition != null)
			{
				this.OnUpdatePosition.Invoke(this.currentScrollPosition);
			}
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0016E7E5 File Offset: 0x0016CBE5
		private float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0016E810 File Offset: 0x0016CC10
		public void SetDataCount(int dataCont)
		{
			this.dataCount = dataCont;
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0016E81C File Offset: 0x0016CC1C
		private void Update()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float num = this.CalculateOffset(this.currentScrollPosition);
			if (this.autoScrolling)
			{
				float num2 = Mathf.Clamp01((Time.unscaledTime - this.autoScrollStartTime) / Mathf.Max(this.autoScrollDuration, float.Epsilon));
				float position = Mathf.Lerp(this.dragStartScrollPosition, this.autoScrollPosition, this.EaseInOutCubic(0f, 1f, num2));
				this.UpdatePosition(position);
				if (Mathf.Approximately(num2, 1f))
				{
					this.autoScrolling = false;
					if (this.OnItemSelected != null)
					{
						this.OnItemSelected.Invoke(Mathf.RoundToInt(this.GetLoopPosition(this.autoScrollPosition, this.dataCount)));
					}
				}
			}
			else if (!this.dragging && (num != 0f || this.velocity != 0f))
			{
				float num3 = this.currentScrollPosition;
				if (this.movementType == ScrollPositionController.MovementType.Elastic && num != 0f)
				{
					float num4 = this.velocity;
					num3 = Mathf.SmoothDamp(this.currentScrollPosition, this.currentScrollPosition + num, ref num4, this.elasticity, float.PositiveInfinity, unscaledDeltaTime);
					this.velocity = num4;
				}
				else if (this.inertia)
				{
					this.velocity *= Mathf.Pow(this.decelerationRate, unscaledDeltaTime);
					if (Mathf.Abs(this.velocity) < 0.001f)
					{
						this.velocity = 0f;
					}
					num3 += this.velocity * unscaledDeltaTime;
					if (this.snap.Enable && Mathf.Abs(this.velocity) < this.snap.VelocityThreshold)
					{
						this.ScrollTo(Mathf.RoundToInt(this.currentScrollPosition), this.snap.Duration);
					}
				}
				else
				{
					this.velocity = 0f;
				}
				if (this.velocity != 0f)
				{
					if (this.movementType == ScrollPositionController.MovementType.Clamped)
					{
						num = this.CalculateOffset(num3);
						num3 += num;
					}
					this.UpdatePosition(num3);
				}
			}
			if (!this.autoScrolling && this.dragging && this.inertia)
			{
				float b = (this.currentScrollPosition - this.prevScrollPosition) / unscaledDeltaTime;
				this.velocity = Mathf.Lerp(this.velocity, b, unscaledDeltaTime * 10f);
			}
			if (this.currentScrollPosition != this.prevScrollPosition)
			{
				this.prevScrollPosition = this.currentScrollPosition;
			}
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x0016EAA0 File Offset: 0x0016CEA0
		public void ScrollTo(int index, float duration)
		{
			this.velocity = 0f;
			this.autoScrolling = true;
			this.autoScrollDuration = duration;
			this.autoScrollStartTime = Time.unscaledTime;
			this.dragStartScrollPosition = this.currentScrollPosition;
			this.autoScrollPosition = ((this.movementType != ScrollPositionController.MovementType.Unrestricted) ? ((float)index) : this.CalculateClosestPosition(index));
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0016EAFC File Offset: 0x0016CEFC
		private float CalculateClosestPosition(int index)
		{
			float num = this.GetLoopPosition((float)index, this.dataCount) - this.GetLoopPosition(this.currentScrollPosition, this.dataCount);
			if (Mathf.Abs(num) > (float)this.dataCount * 0.5f)
			{
				num = Mathf.Sign(-num) * ((float)this.dataCount - Mathf.Abs(num));
			}
			return num + this.currentScrollPosition;
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0016EB62 File Offset: 0x0016CF62
		private float GetLoopPosition(float position, int length)
		{
			if (position < 0f)
			{
				position = (float)(length - 1) + (position + 1f) % (float)length;
			}
			else if (position > (float)(length - 1))
			{
				position %= (float)length;
			}
			return position;
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0016EB98 File Offset: 0x0016CF98
		private float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end * 0.5f * value * value * value + start;
			}
			value -= 2f;
			return end * 0.5f * (value * value * value + 2f) + start;
		}

		// Token: 0x04006E8C RID: 28300
		[SerializeField]
		private RectTransform viewport;

		// Token: 0x04006E8D RID: 28301
		[SerializeField]
		private ScrollPositionController.ScrollDirection directionOfRecognize;

		// Token: 0x04006E8E RID: 28302
		[SerializeField]
		private ScrollPositionController.MovementType movementType = ScrollPositionController.MovementType.Elastic;

		// Token: 0x04006E8F RID: 28303
		[SerializeField]
		private float elasticity = 0.1f;

		// Token: 0x04006E90 RID: 28304
		[SerializeField]
		private float scrollSensitivity = 1f;

		// Token: 0x04006E91 RID: 28305
		[SerializeField]
		private bool inertia = true;

		// Token: 0x04006E92 RID: 28306
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private float decelerationRate = 0.03f;

		// Token: 0x04006E93 RID: 28307
		[SerializeField]
		[Tooltip("Only used when inertia is enabled")]
		private ScrollPositionController.Snap snap = new ScrollPositionController.Snap
		{
			Enable = true,
			VelocityThreshold = 0.5f,
			Duration = 0.3f
		};

		// Token: 0x04006E94 RID: 28308
		[SerializeField]
		private int dataCount;

		// Token: 0x04006E95 RID: 28309
		[Tooltip("Event that fires when the position of an item changes")]
		public ScrollPositionController.UpdatePositionEvent OnUpdatePosition;

		// Token: 0x04006E96 RID: 28310
		[Tooltip("Event that fires when an item is selected/focused")]
		public ScrollPositionController.ItemSelectedEvent OnItemSelected;

		// Token: 0x04006E97 RID: 28311
		private Vector2 pointerStartLocalPosition;

		// Token: 0x04006E98 RID: 28312
		private float dragStartScrollPosition;

		// Token: 0x04006E99 RID: 28313
		private float currentScrollPosition;

		// Token: 0x04006E9A RID: 28314
		private bool dragging;

		// Token: 0x04006E9B RID: 28315
		private float velocity;

		// Token: 0x04006E9C RID: 28316
		private float prevScrollPosition;

		// Token: 0x04006E9D RID: 28317
		private bool autoScrolling;

		// Token: 0x04006E9E RID: 28318
		private float autoScrollDuration;

		// Token: 0x04006E9F RID: 28319
		private float autoScrollStartTime;

		// Token: 0x04006EA0 RID: 28320
		private float autoScrollPosition;

		// Token: 0x02000BEE RID: 3054
		[Serializable]
		public class UpdatePositionEvent : UnityEvent<float>
		{
		}

		// Token: 0x02000BEF RID: 3055
		[Serializable]
		public class ItemSelectedEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000BF0 RID: 3056
		[Serializable]
		private struct Snap
		{
			// Token: 0x04006EA1 RID: 28321
			public bool Enable;

			// Token: 0x04006EA2 RID: 28322
			public float VelocityThreshold;

			// Token: 0x04006EA3 RID: 28323
			public float Duration;
		}

		// Token: 0x02000BF1 RID: 3057
		private enum ScrollDirection
		{
			// Token: 0x04006EA5 RID: 28325
			Vertical,
			// Token: 0x04006EA6 RID: 28326
			Horizontal
		}

		// Token: 0x02000BF2 RID: 3058
		private enum MovementType
		{
			// Token: 0x04006EA8 RID: 28328
			Unrestricted,
			// Token: 0x04006EA9 RID: 28329
			Elastic,
			// Token: 0x04006EAA RID: 28330
			Clamped
		}
	}
}
