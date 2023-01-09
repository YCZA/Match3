using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8F RID: 2959
	[RequireComponent(typeof(RawImage))]
	[ExecuteInEditMode]
	public class ColorSliderImage : MonoBehaviour
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x0015EE0F File Offset: 0x0015D20F
		private RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x0015EE1C File Offset: 0x0015D21C
		private void Awake()
		{
			this.image = base.GetComponent<RawImage>();
			if (this.image)
			{
				this.RegenerateTexture();
			}
			else
			{
				global::UnityEngine.Debug.LogWarning("Missing RawImage on object [" + base.name + "]");
			}
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x0015EE6C File Offset: 0x0015D26C
		private void OnEnable()
		{
			if (this.picker != null && Application.isPlaying)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x0015EECC File Offset: 0x0015D2CC
		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x0015EF22 File Offset: 0x0015D322
		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x0015EF4C File Offset: 0x0015D34C
		private void ColorChanged(Color newColor)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			}
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x0015EF98 File Offset: 0x0015D398
		private void ColorChanged(float hue, float saturation, float value)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			}
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x0015EFE4 File Offset: 0x0015D3E4
		private void RegenerateTexture()
		{
			if (!this.picker)
			{
				global::UnityEngine.Debug.LogWarning("Missing Picker on object [" + base.name + "]");
			}
			Color32 color = (!(this.picker != null)) ? Color.black : this.picker.CurrentColor;
			float num = (!(this.picker != null)) ? 0f : this.picker.H;
			float num2 = (!(this.picker != null)) ? 0f : this.picker.S;
			float num3 = (!(this.picker != null)) ? 0f : this.picker.V;
			bool flag = this.direction == Slider.Direction.BottomToTop || this.direction == Slider.Direction.TopToBottom;
			bool flag2 = this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.RightToLeft;
			int num4;
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.A:
				num4 = 255;
				break;
			case ColorValues.Hue:
				num4 = 360;
				break;
			case ColorValues.Saturation:
			case ColorValues.Value:
				num4 = 100;
				break;
			default:
				throw new NotImplementedException(string.Empty);
			}
			Texture2D texture2D;
			if (flag)
			{
				texture2D = new Texture2D(1, num4);
			}
			else
			{
				texture2D = new Texture2D(num4, 1);
			}
			texture2D.hideFlags = HideFlags.DontSave;
			Color32[] array = new Color32[num4];
			switch (this.type)
			{
			case ColorValues.R:
			{
				byte b = 0;
				while ((int)b < num4)
				{
					array[(!flag2) ? ((int)b) : (num4 - 1 - (int)b)] = new Color32(b, color.g, color.b, byte.MaxValue);
					b += 1;
				}
				break;
			}
			case ColorValues.G:
			{
				byte b2 = 0;
				while ((int)b2 < num4)
				{
					array[(!flag2) ? ((int)b2) : (num4 - 1 - (int)b2)] = new Color32(color.r, b2, color.b, byte.MaxValue);
					b2 += 1;
				}
				break;
			}
			case ColorValues.B:
			{
				byte b3 = 0;
				while ((int)b3 < num4)
				{
					array[(!flag2) ? ((int)b3) : (num4 - 1 - (int)b3)] = new Color32(color.r, color.g, b3, byte.MaxValue);
					b3 += 1;
				}
				break;
			}
			case ColorValues.A:
			{
				byte b4 = 0;
				while ((int)b4 < num4)
				{
					array[(!flag2) ? ((int)b4) : (num4 - 1 - (int)b4)] = new Color32(b4, b4, b4, byte.MaxValue);
					b4 += 1;
				}
				break;
			}
			case ColorValues.Hue:
				for (int i = 0; i < num4; i++)
				{
					array[(!flag2) ? i : (num4 - 1 - i)] = HSVUtil.ConvertHsvToRgb((double)i, 1.0, 1.0, 1f);
				}
				break;
			case ColorValues.Saturation:
				for (int j = 0; j < num4; j++)
				{
					array[(!flag2) ? j : (num4 - 1 - j)] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)((float)j / (float)num4), (double)num3, 1f);
				}
				break;
			case ColorValues.Value:
				for (int k = 0; k < num4; k++)
				{
					array[(!flag2) ? k : (num4 - 1 - k)] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)num2, (double)((float)k / (float)num4), 1f);
				}
				break;
			default:
				throw new NotImplementedException(string.Empty);
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			if (this.image.texture != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
			this.image.texture = texture2D;
			switch (this.direction)
			{
			case Slider.Direction.LeftToRight:
			case Slider.Direction.RightToLeft:
				this.image.uvRect = new Rect(0f, 0f, 1f, 2f);
				break;
			case Slider.Direction.BottomToTop:
			case Slider.Direction.TopToBottom:
				this.image.uvRect = new Rect(0f, 0f, 2f, 1f);
				break;
			}
		}

		// Token: 0x04006CD6 RID: 27862
		public ColorPickerControl picker;

		// Token: 0x04006CD7 RID: 27863
		public ColorValues type;

		// Token: 0x04006CD8 RID: 27864
		public Slider.Direction direction;

		// Token: 0x04006CD9 RID: 27865
		private RawImage image;
	}
}
