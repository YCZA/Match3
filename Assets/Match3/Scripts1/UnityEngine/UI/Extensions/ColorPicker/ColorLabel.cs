using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8A RID: 2954
	[RequireComponent(typeof(Text))]
	public class ColorLabel : MonoBehaviour
	{
		// Token: 0x06004526 RID: 17702 RVA: 0x0015E5B0 File Offset: 0x0015C9B0
		private void Awake()
		{
			this.label = base.GetComponent<Text>();
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x0015E5C0 File Offset: 0x0015C9C0
		private void OnEnable()
		{
			if (Application.isPlaying && this.picker != null)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0015E620 File Offset: 0x0015CA20
		private void OnDestroy()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0015E676 File Offset: 0x0015CA76
		private void ColorChanged(Color color)
		{
			this.UpdateValue();
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x0015E67E File Offset: 0x0015CA7E
		private void HSVChanged(float hue, float sateration, float value)
		{
			this.UpdateValue();
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x0015E688 File Offset: 0x0015CA88
		private void UpdateValue()
		{
			if (this.picker == null)
			{
				this.label.text = this.prefix + "-";
			}
			else
			{
				float value = this.minValue + this.picker.GetValue(this.type) * (this.maxValue - this.minValue);
				this.label.text = this.prefix + this.ConvertToDisplayString(value);
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x0015E70C File Offset: 0x0015CB0C
		private string ConvertToDisplayString(float value)
		{
			if (this.precision > 0)
			{
				return value.ToString("f " + this.precision);
			}
			return Mathf.FloorToInt(value).ToString();
		}

		// Token: 0x04006CBD RID: 27837
		public ColorPickerControl picker;

		// Token: 0x04006CBE RID: 27838
		public ColorValues type;

		// Token: 0x04006CBF RID: 27839
		public string prefix = "R: ";

		// Token: 0x04006CC0 RID: 27840
		public float minValue;

		// Token: 0x04006CC1 RID: 27841
		public float maxValue = 255f;

		// Token: 0x04006CC2 RID: 27842
		public int precision;

		// Token: 0x04006CC3 RID: 27843
		private Text label;
	}
}
