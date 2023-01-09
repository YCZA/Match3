using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B89 RID: 2953
	[RequireComponent(typeof(Image))]
	public class ColorImage : MonoBehaviour
	{
		// Token: 0x06004522 RID: 17698 RVA: 0x0015E53C File Offset: 0x0015C93C
		private void Awake()
		{
			this.image = base.GetComponent<Image>();
			this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0015E566 File Offset: 0x0015C966
		private void OnDestroy()
		{
			this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x0015E584 File Offset: 0x0015C984
		private void ColorChanged(Color newColor)
		{
			this.image.color = newColor;
		}

		// Token: 0x04006CBB RID: 27835
		public ColorPickerControl picker;

		// Token: 0x04006CBC RID: 27836
		private Image image;
	}
}
