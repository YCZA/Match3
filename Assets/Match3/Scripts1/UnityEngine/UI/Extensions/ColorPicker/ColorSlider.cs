using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8E RID: 2958
	[RequireComponent(typeof(Slider))]
	public class ColorSlider : MonoBehaviour
	{
		// Token: 0x0600454E RID: 17742 RVA: 0x0015EBFC File Offset: 0x0015CFFC
		private void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x0015EC6C File Offset: 0x0015D06C
		private void OnDestroy()
		{
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SliderChanged));
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0015ECD0 File Offset: 0x0015D0D0
		private void ColorChanged(Color newColor)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.R:
				this.slider.normalizedValue = newColor.r;
				break;
			case ColorValues.G:
				this.slider.normalizedValue = newColor.g;
				break;
			case ColorValues.B:
				this.slider.normalizedValue = newColor.b;
				break;
			case ColorValues.A:
				this.slider.normalizedValue = newColor.a;
				break;
			}
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x0015ED68 File Offset: 0x0015D168
		private void HSVChanged(float hue, float saturation, float value)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.Hue:
				this.slider.normalizedValue = hue;
				break;
			case ColorValues.Saturation:
				this.slider.normalizedValue = saturation;
				break;
			case ColorValues.Value:
				this.slider.normalizedValue = value;
				break;
			}
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x0015EDD4 File Offset: 0x0015D1D4
		private void SliderChanged(float newValue)
		{
			if (this.listen)
			{
				newValue = this.slider.normalizedValue;
				this.ColorPicker.AssignColor(this.type, newValue);
			}
			this.listen = true;
		}

		// Token: 0x04006CD2 RID: 27858
		public ColorPickerControl ColorPicker;

		// Token: 0x04006CD3 RID: 27859
		public ColorValues type;

		// Token: 0x04006CD4 RID: 27860
		private Slider slider;

		// Token: 0x04006CD5 RID: 27861
		private bool listen = true;
	}
}
