using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDB RID: 3035
	[AddComponentMenu("UI/Effects/Extensions/UISoftAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UISoftAdditiveEffect : MonoBehaviour
	{
		// Token: 0x06004728 RID: 18216 RVA: 0x0016A6D4 File Offset: 0x00168AD4
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0016A6DC File Offset: 0x00168ADC
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UISoftAdditive"));
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0016A764 File Offset: 0x00168B64
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04006E44 RID: 28228
		private MaskableGraphic mGraphic;
	}
}
