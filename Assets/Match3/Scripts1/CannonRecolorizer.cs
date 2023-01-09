using UnityEngine;

// Token: 0x020004D9 RID: 1241
namespace Match3.Scripts1
{
	public class CannonRecolorizer : MonoBehaviour
	{
		// Token: 0x0600229A RID: 8858 RVA: 0x00099360 File Offset: 0x00097760
		public void SetColor(Color color)
		{
			this.RayColorMeshRenderer.sharedMaterial.SetColor("_MainColorB", color);
			this.RayIconColorMeshRenderer.sharedMaterial.SetColor("_MainColorB", color);
			var go1 = this.DropsColor.main;
			go1.startColor = color;
			var go2 = this.CenterGlowColor.main;
			go1.startColor = color;
		}

		// Token: 0x04004E30 RID: 20016
		public MeshRenderer RayColorMeshRenderer;

		// Token: 0x04004E31 RID: 20017
		public MeshRenderer RayIconColorMeshRenderer;

		// Token: 0x04004E32 RID: 20018
		public ParticleSystem CenterGlowColor;

		// Token: 0x04004E33 RID: 20019
		public ParticleSystem DropsColor;
	}
}
