using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BC1 RID: 3009
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Extensions/UI_Knob")]
	public class UI_Knob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x0600469C RID: 18076 RVA: 0x00166351 File Offset: 0x00164751
		public void OnPointerDown(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0016635A File Offset: 0x0016475A
		public void OnPointerUp(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00166363 File Offset: 0x00164763
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0016636C File Offset: 0x0016476C
		public void OnPointerExit(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00166375 File Offset: 0x00164775
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.SetInitPointerData(eventData);
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x00166380 File Offset: 0x00164780
		private void SetInitPointerData(PointerEventData eventData)
		{
			this._initRotation = base.transform.rotation;
			this._currentVector = eventData.position - (Vector2)transform.position;
			this._initAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x001663E8 File Offset: 0x001647E8
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._canDrag)
			{
				this.SetInitPointerData(eventData);
				return;
			}
			this._currentVector = eventData.position - (Vector2)transform.position;
			this._currentAngle = Mathf.Atan2(this._currentVector.y, this._currentVector.x) * 57.29578f;
			Quaternion rhs = Quaternion.AngleAxis(this._currentAngle - this._initAngle, base.transform.forward);
			rhs.eulerAngles = new Vector3(0f, 0f, rhs.eulerAngles.z);
			Quaternion rotation = this._initRotation * rhs;
			if (this.direction == UI_Knob.Direction.CW)
			{
				this.knobValue = 1f - rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f - 360f * this.knobValue);
				}
			}
			else
			{
				this.knobValue = rotation.eulerAngles.z / 360f;
				if (this.snapToPosition)
				{
					this.SnapToPosition(ref this.knobValue);
					rotation.eulerAngles = new Vector3(0f, 0f, 360f * this.knobValue);
				}
			}
			if (Mathf.Abs(this.knobValue - this._previousValue) > 0.5f)
			{
				if (this.knobValue < 0.5f && this.loops > 1 && this._currentLoops < (float)(this.loops - 1))
				{
					this._currentLoops += 1f;
				}
				else if (this.knobValue > 0.5f && this._currentLoops >= 1f)
				{
					this._currentLoops -= 1f;
				}
				else
				{
					if (this.knobValue > 0.5f && this._currentLoops == 0f)
					{
						this.knobValue = 0f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
					if (this.knobValue < 0.5f && this._currentLoops == (float)(this.loops - 1))
					{
						this.knobValue = 1f;
						base.transform.localEulerAngles = Vector3.zero;
						this.SetInitPointerData(eventData);
						this.InvokeEvents(this.knobValue + this._currentLoops);
						return;
					}
				}
			}
			if (this.maxValue > 0f && this.knobValue + this._currentLoops > this.maxValue)
			{
				this.knobValue = this.maxValue;
				float z = (this.direction != UI_Knob.Direction.CW) ? (360f * this.maxValue) : (360f - 360f * this.maxValue);
				base.transform.localEulerAngles = new Vector3(0f, 0f, z);
				this.SetInitPointerData(eventData);
				this.InvokeEvents(this.knobValue);
				return;
			}
			base.transform.rotation = rotation;
			this.InvokeEvents(this.knobValue + this._currentLoops);
			this._previousValue = this.knobValue;
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00166770 File Offset: 0x00164B70
		private void SnapToPosition(ref float knobValue)
		{
			float num = 1f / (float)this.snapStepsPerLoop;
			float num2 = Mathf.Round(knobValue / num) * num;
			knobValue = num2;
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x0016679A File Offset: 0x00164B9A
		private void InvokeEvents(float value)
		{
			if (this.clampOutput01)
			{
				value /= (float)this.loops;
			}
			this.OnValueChanged.Invoke(value);
		}

		// Token: 0x04006DEA RID: 28138
		[Tooltip("Direction of rotation CW - clockwise, CCW - counterClockwise")]
		public UI_Knob.Direction direction;

		// Token: 0x04006DEB RID: 28139
		[HideInInspector]
		public float knobValue;

		// Token: 0x04006DEC RID: 28140
		[Tooltip("Max value of the knob, maximum RAW output value knob can reach, overrides snap step, IF set to 0 or higher than loops, max value will be set by loops")]
		public float maxValue;

		// Token: 0x04006DED RID: 28141
		[Tooltip("How many rotations knob can do, if higher than max value, the latter will limit max value")]
		public int loops = 1;

		// Token: 0x04006DEE RID: 28142
		[Tooltip("Clamp output value between 0 and 1, usefull with loops > 1")]
		public bool clampOutput01;

		// Token: 0x04006DEF RID: 28143
		[Tooltip("snap to position?")]
		public bool snapToPosition;

		// Token: 0x04006DF0 RID: 28144
		[Tooltip("Number of positions to snap")]
		public int snapStepsPerLoop = 10;

		// Token: 0x04006DF1 RID: 28145
		[Space(30f)]
		public KnobFloatValueEvent OnValueChanged;

		// Token: 0x04006DF2 RID: 28146
		private float _currentLoops;

		// Token: 0x04006DF3 RID: 28147
		private float _previousValue;

		// Token: 0x04006DF4 RID: 28148
		private float _initAngle;

		// Token: 0x04006DF5 RID: 28149
		private float _currentAngle;

		// Token: 0x04006DF6 RID: 28150
		private Vector2 _currentVector;

		// Token: 0x04006DF7 RID: 28151
		private Quaternion _initRotation;

		// Token: 0x04006DF8 RID: 28152
		private bool _canDrag;

		// Token: 0x02000BC2 RID: 3010
		public enum Direction
		{
			// Token: 0x04006DFA RID: 28154
			CW,
			// Token: 0x04006DFB RID: 28155
			CCW
		}
	}
}
