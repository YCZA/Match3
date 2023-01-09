using UnityEngine;

// Token: 0x02000977 RID: 2423
namespace Match3.Scripts1
{
	[RequireComponent(typeof(Camera))]
	public class ReplaceShader : MonoBehaviour
	{
		// Token: 0x06003B1A RID: 15130 RVA: 0x0011DD7C File Offset: 0x0011C17C
		protected virtual void Awake()
		{
			this.m_camera = base.GetComponent<Camera>();
			this.m_camera.SetReplacementShader(this.shader, null);
		}

		// Token: 0x040062F3 RID: 25331
		protected Camera m_camera;

		// Token: 0x040062F4 RID: 25332
		public Shader shader;
	}
}
