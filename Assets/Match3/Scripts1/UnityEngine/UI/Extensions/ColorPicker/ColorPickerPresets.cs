using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8C RID: 2956
	public class ColorPickerPresets : MonoBehaviour
	{
		// Token: 0x06004545 RID: 17733 RVA: 0x0015EAFB File Offset: 0x0015CEFB
		private void Awake()
		{
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x0015EB1C File Offset: 0x0015CF1C
		public void CreatePresetButton()
		{
			for (int i = 0; i < this.presets.Length; i++)
			{
				if (!this.presets[i].activeSelf)
				{
					this.presets[i].SetActive(true);
					this.presets[i].GetComponent<Image>().color = this.picker.CurrentColor;
					break;
				}
			}
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0015EB84 File Offset: 0x0015CF84
		public void PresetSelect(Image sender)
		{
			this.picker.CurrentColor = sender.color;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x0015EB97 File Offset: 0x0015CF97
		private void ColorChanged(Color color)
		{
			this.createPresetImage.color = color;
		}

		// Token: 0x04006CCD RID: 27853
		public ColorPickerControl picker;

		// Token: 0x04006CCE RID: 27854
		public GameObject[] presets;

		// Token: 0x04006CCF RID: 27855
		public Image createPresetImage;
	}
}
