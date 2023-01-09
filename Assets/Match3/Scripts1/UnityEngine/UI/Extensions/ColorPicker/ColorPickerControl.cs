using System;
using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8B RID: 2955
	public class ColorPickerControl : MonoBehaviour
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x0600452E RID: 17710 RVA: 0x0015E77F File Offset: 0x0015CB7F
		// (set) Token: 0x0600452F RID: 17711 RVA: 0x0015E7A0 File Offset: 0x0015CBA0
		public Color CurrentColor
		{
			get
			{
				return new Color(this._red, this._green, this._blue, this._alpha);
			}
			set
			{
				if (this.CurrentColor == value)
				{
					return;
				}
				this._red = value.r;
				this._green = value.g;
				this._blue = value.b;
				this._alpha = value.a;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x0015E7FF File Offset: 0x0015CBFF
		private void Start()
		{
			this.SendChangedEvent();
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x0015E807 File Offset: 0x0015CC07
		// (set) Token: 0x06004532 RID: 17714 RVA: 0x0015E80F File Offset: 0x0015CC0F
		public float H
		{
			get
			{
				return this._hue;
			}
			set
			{
				if (this._hue == value)
				{
					return;
				}
				this._hue = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x0015E831 File Offset: 0x0015CC31
		// (set) Token: 0x06004534 RID: 17716 RVA: 0x0015E839 File Offset: 0x0015CC39
		public float S
		{
			get
			{
				return this._saturation;
			}
			set
			{
				if (this._saturation == value)
				{
					return;
				}
				this._saturation = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06004535 RID: 17717 RVA: 0x0015E85B File Offset: 0x0015CC5B
		// (set) Token: 0x06004536 RID: 17718 RVA: 0x0015E863 File Offset: 0x0015CC63
		public float V
		{
			get
			{
				return this._brightness;
			}
			set
			{
				if (this._brightness == value)
				{
					return;
				}
				this._brightness = value;
				this.HSVChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x0015E885 File Offset: 0x0015CC85
		// (set) Token: 0x06004538 RID: 17720 RVA: 0x0015E88D File Offset: 0x0015CC8D
		public float R
		{
			get
			{
				return this._red;
			}
			set
			{
				if (this._red == value)
				{
					return;
				}
				this._red = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06004539 RID: 17721 RVA: 0x0015E8AF File Offset: 0x0015CCAF
		// (set) Token: 0x0600453A RID: 17722 RVA: 0x0015E8B7 File Offset: 0x0015CCB7
		public float G
		{
			get
			{
				return this._green;
			}
			set
			{
				if (this._green == value)
				{
					return;
				}
				this._green = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600453B RID: 17723 RVA: 0x0015E8D9 File Offset: 0x0015CCD9
		// (set) Token: 0x0600453C RID: 17724 RVA: 0x0015E8E1 File Offset: 0x0015CCE1
		public float B
		{
			get
			{
				return this._blue;
			}
			set
			{
				if (this._blue == value)
				{
					return;
				}
				this._blue = value;
				this.RGBChanged();
				this.SendChangedEvent();
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x0015E903 File Offset: 0x0015CD03
		// (set) Token: 0x0600453E RID: 17726 RVA: 0x0015E90B File Offset: 0x0015CD0B
		private float A
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (this._alpha == value)
				{
					return;
				}
				this._alpha = value;
				this.SendChangedEvent();
			}
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0015E928 File Offset: 0x0015CD28
		private void RGBChanged()
		{
			HsvColor hsvColor = HSVUtil.ConvertRgbToHsv(this.CurrentColor);
			this._hue = hsvColor.NormalizedH;
			this._saturation = hsvColor.NormalizedS;
			this._brightness = hsvColor.NormalizedV;
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0015E968 File Offset: 0x0015CD68
		private void HSVChanged()
		{
			Color color = HSVUtil.ConvertHsvToRgb((double)(this._hue * 360f), (double)this._saturation, (double)this._brightness, this._alpha);
			this._red = color.r;
			this._green = color.g;
			this._blue = color.b;
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x0015E9C3 File Offset: 0x0015CDC3
		private void SendChangedEvent()
		{
			this.onValueChanged.Invoke(this.CurrentColor);
			this.onHSVChanged.Invoke(this._hue, this._saturation, this._brightness);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x0015E9F4 File Offset: 0x0015CDF4
		public void AssignColor(ColorValues type, float value)
		{
			switch (type)
			{
			case ColorValues.R:
				this.R = value;
				break;
			case ColorValues.G:
				this.G = value;
				break;
			case ColorValues.B:
				this.B = value;
				break;
			case ColorValues.A:
				this.A = value;
				break;
			case ColorValues.Hue:
				this.H = value;
				break;
			case ColorValues.Saturation:
				this.S = value;
				break;
			case ColorValues.Value:
				this.V = value;
				break;
			}
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x0015EA84 File Offset: 0x0015CE84
		public float GetValue(ColorValues type)
		{
			switch (type)
			{
			case ColorValues.R:
				return this.R;
			case ColorValues.G:
				return this.G;
			case ColorValues.B:
				return this.B;
			case ColorValues.A:
				return this.A;
			case ColorValues.Hue:
				return this.H;
			case ColorValues.Saturation:
				return this.S;
			case ColorValues.Value:
				return this.V;
			default:
				throw new NotImplementedException(string.Empty);
			}
		}

		// Token: 0x04006CC4 RID: 27844
		private float _hue;

		// Token: 0x04006CC5 RID: 27845
		private float _saturation;

		// Token: 0x04006CC6 RID: 27846
		private float _brightness;

		// Token: 0x04006CC7 RID: 27847
		private float _red;

		// Token: 0x04006CC8 RID: 27848
		private float _green;

		// Token: 0x04006CC9 RID: 27849
		private float _blue;

		// Token: 0x04006CCA RID: 27850
		private float _alpha = 1f;

		// Token: 0x04006CCB RID: 27851
		public ColorChangedEvent onValueChanged = new ColorChangedEvent();

		// Token: 0x04006CCC RID: 27852
		public HSVChangedEvent onHSVChanged = new HSVChangedEvent();
	}
}
