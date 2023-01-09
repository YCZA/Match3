using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD6 RID: 3030
	[AddComponentMenu("UI/Effects/Extensions/UIAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UIAdditiveEffect : MonoBehaviour
	{
		// Token: 0x06004712 RID: 18194 RVA: 0x0016A320 File Offset: 0x00168720
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x0016A328 File Offset: 0x00168728
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UIAdditive"));
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0016A3B0 File Offset: 0x001687B0
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04006E3A RID: 28218
		private MaskableGraphic mGraphic;
	}
}
