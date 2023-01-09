using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B96 RID: 2966
	[RequireComponent(typeof(BoxSlider), typeof(RawImage))]
	[ExecuteInEditMode]
	public class SVBoxSlider : MonoBehaviour
	{
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x0015FC5B File Offset: 0x0015E05B
		public RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0015FC68 File Offset: 0x0015E068
		private void Awake()
		{
			this.slider = base.GetComponent<BoxSlider>();
			this.image = base.GetComponent<RawImage>();
			this.RegenerateSVTexture();
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x0015FC88 File Offset: 0x0015E088
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.slider.OnValueChanged.AddListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x0015FCE8 File Offset: 0x0015E0E8
		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.slider.OnValueChanged.RemoveListener(new UnityAction<float, float>(this.SliderChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0015FD3E File Offset: 0x0015E13E
		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x0015FD66 File Offset: 0x0015E166
		private void SliderChanged(float saturation, float value)
		{
			if (this.listen)
			{
				this.picker.AssignColor(ColorValues.Saturation, saturation);
				this.picker.AssignColor(ColorValues.Value, value);
			}
			this.listen = true;
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x0015FD94 File Offset: 0x0015E194
		private void HSVChanged(float h, float s, float v)
		{
			if (this.lastH != h)
			{
				this.lastH = h;
				this.RegenerateSVTexture();
			}
			if (s != this.slider.NormalizedValueX)
			{
				this.listen = false;
				this.slider.NormalizedValueX = s;
			}
			if (v != this.slider.NormalizedValueY)
			{
				this.listen = false;
				this.slider.NormalizedValueY = v;
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x0015FE04 File Offset: 0x0015E204
		private void RegenerateSVTexture()
		{
			double h = (double)((!(this.picker != null)) ? 0f : (this.picker.H * 360f));
			if (this.image.texture != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
			Texture2D texture2D = new Texture2D(100, 100)
			{
				hideFlags = HideFlags.DontSave
			};
			for (int i = 0; i < 100; i++)
			{
				Color32[] array = new Color32[100];
				for (int j = 0; j < 100; j++)
				{
					array[j] = HSVUtil.ConvertHsvToRgb(h, (double)((float)i / 100f), (double)((float)j / 100f), 1f);
				}
				texture2D.SetPixels32(i, 0, 1, 100, array);
			}
			texture2D.Apply();
			this.image.texture = texture2D;
		}

		// Token: 0x04006CE9 RID: 27881
		public ColorPickerControl picker;

		// Token: 0x04006CEA RID: 27882
		private BoxSlider slider;

		// Token: 0x04006CEB RID: 27883
		private RawImage image;

		// Token: 0x04006CEC RID: 27884
		private float lastH = -1f;

		// Token: 0x04006CED RID: 27885
		private bool listen = true;
	}
}
