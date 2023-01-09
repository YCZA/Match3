using UnityEngine;
using UnityEngine.UI;

namespace Match3.Scripts1.UnityEngine.UI.Extensions
{
	// Token: 0x02000BD8 RID: 3032
	[AddComponentMenu("UI/Effects/Extensions/UILinearDodgeEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UILinearDodgeEffect : MonoBehaviour
	{
		// Token: 0x0600471C RID: 18204 RVA: 0x0016A4F3 File Offset: 0x001688F3
		private void Start()
		{
			this.SetMaterial();
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0016A4FC File Offset: 0x001688FC
		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UILinearDodge"));
				}
			}
			else
			{
				global::UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0016A584 File Offset: 0x00168984
		public void OnValidate()
		{
			this.SetMaterial();
		}

		// Token: 0x04006E41 RID: 28225
		private MaskableGraphic mGraphic;
	}
}
