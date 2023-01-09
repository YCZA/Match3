using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD9 RID: 3033
	[AddComponentMenu("UI/Effects/Extensions/UIMultiplyEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIMultiplyEffect : MonoBehaviour
	{
		// Token: 0x06004720 RID: 18208 RVA: 0x0016A594 File Offset: 0x00168994
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0016A59C File Offset: 0x0016899C
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIMultiply"));
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0016A624 File Offset: 0x00168A24
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04006E42 RID: 28226
		private MaskableGraphic mGraphic;
	}
}
