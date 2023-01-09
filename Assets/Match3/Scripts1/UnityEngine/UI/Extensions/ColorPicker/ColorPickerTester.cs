using UnityEngine;

namespace Match3.Scripts1.UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x02000B8D RID: 2957
	public class ColorPickerTester : MonoBehaviour
	{
		// Token: 0x0600454A RID: 17738 RVA: 0x0015EBAD File Offset: 0x0015CFAD
		private void Awake()
		{
			this.pickerRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x0015EBBB File Offset: 0x0015CFBB
		private void Start()
		{
			this.picker.onValueChanged.AddListener(delegate(Color color)
			{
				this.pickerRenderer.material.color = color;
			});
		}

		// Token: 0x04006CD0 RID: 27856
		public Renderer pickerRenderer;

		// Token: 0x04006CD1 RID: 27857
		public ColorPickerControl picker;
	}
}
