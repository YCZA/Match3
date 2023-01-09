using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BDA RID: 3034
	[AddComponentMenu("UI/Effects/Extensions/UIScreenEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIScreenEffect : MonoBehaviour
	{
		// Token: 0x06004724 RID: 18212 RVA: 0x0016A634 File Offset: 0x00168A34
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x0016A63C File Offset: 0x00168A3C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIScreen"));
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x0016A6C4 File Offset: 0x00168AC4
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04006E43 RID: 28227
		private MaskableGraphic mGraphic;
	}
}
