using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BA7 RID: 2983
	[AddComponentMenu("UI/Extensions/Radial Slider")]
	[RequireComponent(typeof(Image))]
	public class RadialSlider : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x0016225D File Offset: 0x0016065D
		// (set) Token: 0x060045DF RID: 17887 RVA: 0x00162270 File Offset: 0x00160670
		public float Angle
		{
			get
			{
				return this.RadialImage.fillAmount * 360f;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value / 360f);
				}
				else
				{
					this.UpdateRadialImage(value / 360f);
				}
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060045E0 RID: 17888 RVA: 0x0016229C File Offset: 0x0016069C
		// (set) Token: 0x060045E1 RID: 17889 RVA: 0x001622A9 File Offset: 0x001606A9
		public float Value
		{
			get
			{
				return this.RadialImage.fillAmount;
			}
			set
			{
				if (this.LerpToTarget)
				{
					this.StartLerp(value);
				}
				else
				{
					this.UpdateRadialImage(value);
				}
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x001622C9 File Offset: 0x001606C9
		// (set) Token: 0x060045E3 RID: 17891 RVA: 0x001622D1 File Offset: 0x001606D1
		public Color EndColor
		{
			get
			{
				return this.m_endColor;
			}
			set
			{
				this.m_endColor = value;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x001622DA File Offset: 0x001606DA
		// (set) Token: 0x060045E5 RID: 17893 RVA: 0x001622E2 File Offset: 0x001606E2
		public Color StartColor
		{
			get
			{
				return this.m_startColor;
			}
			set
			{
				this.m_startColor = value;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x001622EB File Offset: 0x001606EB
		// (set) Token: 0x060045E7 RID: 17895 RVA: 0x001622F3 File Offset: 0x001606F3
		public bool LerpToTarget
		{
			get
			{
				return this.m_lerpToTarget;
			}
			set
			{
				this.m_lerpToTarget = value;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x001622FC File Offset: 0x001606FC
		// (set) Token: 0x060045E9 RID: 17897 RVA: 0x00162304 File Offset: 0x00160704
		public AnimationCurve LerpCurve
		{
			get
			{
				return this.m_lerpCurve;
			}
			set
			{
				this.m_lerpCurve = value;
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x0016233E File Offset: 0x0016073E
		public bool LerpInProgress
		{
			get
			{
				return this.lerpInProgress;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x00162348 File Offset: 0x00160748
		public Image RadialImage
		{
			get
			{
				if (this.m_image == null)
				{
					this.m_image = base.GetComponent<Image>();
					this.m_image.type = Image.Type.Filled;
					this.m_image.fillMethod = Image.FillMethod.Radial360;
					this.m_image.fillOrigin = 3;
					this.m_image.fillAmount = 0f;
				}
				return this.m_image;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x001623AC File Offset: 0x001607AC
		// (set) Token: 0x060045ED RID: 17901 RVA: 0x001623B4 File Offset: 0x001607B4
		public RadialSlider.RadialSliderValueChangedEvent onValueChanged
		{
			get
			{
				return this._onValueChanged;
			}
			set
			{
				this._onValueChanged = value;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x001623BD File Offset: 0x001607BD
		// (set) Token: 0x060045EF RID: 17903 RVA: 0x001623C5 File Offset: 0x001607C5
		public RadialSlider.RadialSliderTextValueChangedEvent onTextValueChanged
		{
			get
			{
				return this._onTextValueChanged;
			}
			set
			{
				this._onTextValueChanged = value;
			}
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x001623D0 File Offset: 0x001607D0
		private void Awake()
		{
			if (this.LerpCurve != null && this.LerpCurve.length > 0)
			{
				this.m_lerpTime = this.LerpCurve[this.LerpCurve.length - 1].time;
			}
			else
			{
				this.m_lerpTime = 1f;
			}
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00162430 File Offset: 0x00160830
		private void Update()
		{
			if (this.isPointerDown)
			{
				this.m_targetAngle = this.GetAngleFromMousePoint();
				if (!this.lerpInProgress)
				{
					if (!this.LerpToTarget)
					{
						this.UpdateRadialImage(this.m_targetAngle);
						this.NotifyValueChanged();
					}
					else
					{
						if (this.isPointerReleased)
						{
							this.StartLerp(this.m_targetAngle);
						}
						this.isPointerReleased = false;
					}
				}
			}
			if (this.lerpInProgress && this.Value != this.m_lerpTargetAngle)
			{
				this.m_currentLerpTime += Time.deltaTime;
				float num = this.m_currentLerpTime / this.m_lerpTime;
				if (this.LerpCurve != null && this.LerpCurve.length > 0)
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, this.LerpCurve.Evaluate(num)));
				}
				else
				{
					this.UpdateRadialImage(Mathf.Lerp(this.m_startAngle, this.m_lerpTargetAngle, num));
				}
			}
			if (this.m_currentLerpTime >= this.m_lerpTime || this.Value == this.m_lerpTargetAngle)
			{
				this.lerpInProgress = false;
				this.UpdateRadialImage(this.m_lerpTargetAngle);
				this.NotifyValueChanged();
			}
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00162572 File Offset: 0x00160972
		private void StartLerp(float targetAngle)
		{
			if (!this.lerpInProgress)
			{
				this.m_startAngle = this.Value;
				this.m_lerpTargetAngle = targetAngle;
				this.m_currentLerpTime = 0f;
				this.lerpInProgress = true;
			}
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x001625A4 File Offset: 0x001609A4
		private float GetAngleFromMousePoint()
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, global::UnityEngine.Input.mousePosition, this.m_eventCamera, out this.m_localPos);
			return (Mathf.Atan2(-this.m_localPos.y, this.m_localPos.x) * 180f / 3.1415927f + 180f) / 360f;
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x0016260C File Offset: 0x00160A0C
		private void UpdateRadialImage(float targetAngle)
		{
			this.RadialImage.fillAmount = targetAngle;
			this.RadialImage.color = Color.Lerp(this.m_startColor, this.m_endColor, targetAngle);
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00162638 File Offset: 0x00160A38
		private void NotifyValueChanged()
		{
			this._onValueChanged.Invoke((int)(this.m_targetAngle * 360f));
			this._onTextValueChanged.Invoke(((int)(this.m_targetAngle * 360f)).ToString());
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00162683 File Offset: 0x00160A83
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00162691 File Offset: 0x00160A91
		public void OnPointerDown(PointerEventData eventData)
		{
			this.m_eventCamera = eventData.enterEventCamera;
			this.isPointerDown = true;
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x001626A6 File Offset: 0x00160AA6
		public void OnPointerUp(PointerEventData eventData)
		{
			this.isPointerDown = false;
			this.isPointerReleased = true;
		}

		// Token: 0x04006D5F RID: 27999
		private bool isPointerDown;

		// Token: 0x04006D60 RID: 28000
		private bool isPointerReleased;

		// Token: 0x04006D61 RID: 28001
		private bool lerpInProgress;

		// Token: 0x04006D62 RID: 28002
		private Vector2 m_localPos;

		// Token: 0x04006D63 RID: 28003
		private float m_targetAngle;

		// Token: 0x04006D64 RID: 28004
		private float m_lerpTargetAngle;

		// Token: 0x04006D65 RID: 28005
		private float m_startAngle;

		// Token: 0x04006D66 RID: 28006
		private float m_currentLerpTime;

		// Token: 0x04006D67 RID: 28007
		private float m_lerpTime;

		// Token: 0x04006D68 RID: 28008
		private Camera m_eventCamera;

		// Token: 0x04006D69 RID: 28009
		private Image m_image;

		// Token: 0x04006D6A RID: 28010
		[SerializeField]
		[Tooltip("Radial Gradient Start Color")]
		private Color m_startColor = Color.green;

		// Token: 0x04006D6B RID: 28011
		[SerializeField]
		[Tooltip("Radial Gradient End Color")]
		private Color m_endColor = Color.red;

		// Token: 0x04006D6C RID: 28012
		[Tooltip("Move slider absolute or use Lerping?\nDragging only supported with absolute")]
		[SerializeField]
		private bool m_lerpToTarget;

		// Token: 0x04006D6D RID: 28013
		[Tooltip("Curve to apply to the Lerp\nMust be set to enable Lerp")]
		[SerializeField]
		private AnimationCurve m_lerpCurve;

		// Token: 0x04006D6E RID: 28014
		[Tooltip("Event fired when value of control changes, outputs an INT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderValueChangedEvent _onValueChanged = new RadialSlider.RadialSliderValueChangedEvent();

		// Token: 0x04006D6F RID: 28015
		[Tooltip("Event fired when value of control changes, outputs a TEXT angle value")]
		[SerializeField]
		private RadialSlider.RadialSliderTextValueChangedEvent _onTextValueChanged = new RadialSlider.RadialSliderTextValueChangedEvent();

		// Token: 0x02000BA8 RID: 2984
		[Serializable]
		public class RadialSliderValueChangedEvent : UnityEvent<int>
		{
		}

		// Token: 0x02000BA9 RID: 2985
		[Serializable]
		public class RadialSliderTextValueChangedEvent : UnityEvent<string>
		{
		}
	}
}
